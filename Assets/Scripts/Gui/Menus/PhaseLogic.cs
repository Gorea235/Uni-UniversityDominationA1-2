using System;
using System.Collections;
using Manager;
using Map;
using Map.Hex;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Gui
{
    public abstract class PhaseLogic : MonoBehaviour, IMenu
    {
        #region Private Fields

        // ======= camera movement fields =======
        /// <summary>
        /// The minimum length of Vector that the mouse has to move
        /// before the camera will start panning.
        /// This will still need tuning.
        /// By using a property getter, it allows for per-device settings.
        /// </summary>
        /// <value>The minimum length of vector before the camera will start panning.</value>
        float _minMouseMoveLength { get; } = 5f;
        /// <summary>
        /// The mouse position when the panning starts.
        /// </summary>
        Vector3 _mousePanStartPos;
        /// <summary>
        /// The camera position when the panning starts.
        /// </summary>
        Vector3 _cameraPanStartPos;
        /// <summary>
        /// The amount to scale mouse movements by when applying to
        /// camera position.
        /// </summary>
        float _cameraPanScale = 0.05f;
        /// <summary>
        /// Whether the camera is currently panning.
        /// </summary>
        bool _cameraIsPanning = false;
        /// <summary>
        /// Whether the pointer is currently over a UI element.
        /// </summary>
        bool _pointerWasOverGameObject = false;

        #endregion

        #region Protected Properties

        /// <summary>
        /// The main game manager.
        /// </summary>
        protected MainManager Main { get; private set; }

        /// <summary>
        /// Gets or sets whether to skip mouse click checking for
        /// the current frame.
        /// </summary>
        /// <value><c>true</c> if skip current frame mouse click; otherwise, <c>false</c>.</value>
        protected bool SkipCurrentFrameMouseClick { get; set; }

        /// <summary>
        /// Gets or sets whether the Update method should
        /// check for screen clicks.
        /// </summary>
        /// <value><c>true</c> if do screen click check; otherwise, <c>false</c>.</value>
        protected bool DoScreenClickCheck { get; set; } = true;

        bool _doCameraUpdate = true;
        /// <summary>
        /// Gets or sets whether the Update method should process camera
        /// panning or not.
        /// </summary>
        /// <value><c>true</c> if do camera update; otherwise, <c>false</c>.</value>
        protected bool DoCameraUpdate
        {
            get
            {
                return _doCameraUpdate;
            }
            set
            {
                _doCameraUpdate = value;
                _cameraIsPanning &= value; // if value is false, stop panning
            }
        }

        /// <summary>
        /// The currently selected sector.
        /// </summary>
        protected Sector SelectedSector { get; private set; }
        /// <summary>
        /// The currently selected range.
        /// </summary>
        protected HashSet<Sector> SelectedRange { get; private set; } = new HashSet<Sector>();
        protected HighlightLevels SelectedRangeHighlight
        {
            set
            {
                foreach (Sector sector in SelectedRange)
                    if (sector != SelectedSector)
                        sector.Highlight = value;
            }
        }

        #endregion

        #region Public Properties

        public abstract bool IsEnabled { get; set; }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Called when the mouse left button has been clicked.
        /// </summary>
        /// <param name="position">Mouse position when it was clicked.</param>
        protected abstract void OnMouseLeftClick(Vector3 position);

        #endregion

        #region MonoBehaviour

        protected virtual void Awake()
        {
            Main = GameObject.Find("Manager").GetComponent<MainManager>();
        }


        /// <summary>
        /// Does generic Update process.
        /// Implementing classes that override this method should call it
        /// via <c>base.Update()</c> to ensure things run smoothly.
        /// Items processed by this method can be enabled and disabled via
        /// the protected properties provided by this class.
        /// </summary>
        protected virtual void Update()
        {
            // collect current game state
            bool mouseLeftDown = Input.GetMouseButtonDown(0);
            bool mouseLeftState = Input.GetMouseButton(0);
            bool mouseLeftUp = Input.GetMouseButtonUp(0);
            Vector3 mousePosition = Input.mousePosition;

            // since the test only works on mouse down (for touch), we preseve the starting value
            if (mouseLeftDown)
                _pointerWasOverGameObject = EventSystem.current.IsPointerOverGameObject();

            // screen click processing
            if (DoScreenClickCheck && !SkipCurrentFrameMouseClick)
            {
                // if the mouse was released and not over a UI element, and we were not panning
                // the camera, fire the click method
                if (mouseLeftUp && !_pointerWasOverGameObject && !_cameraIsPanning)
                    OnMouseLeftClick(mousePosition);
            }

            // camera panning processing
            if (DoCameraUpdate)
            {
                // if the main mouse button is pressed down this frame,
                // set the starting positions of the mouse and camera
                if (mouseLeftDown)
                {
                    _mousePanStartPos = mousePosition;
                    _cameraPanStartPos = Camera.main.transform.position;
                }

                // if the main mouse button is pressed down, do panning
                // checks and processing
                if (mouseLeftState)
                {
                    // if not panning and mouse move length is larger than
                    // min move length
                    if (!_cameraIsPanning && (mousePosition - _mousePanStartPos).magnitude > _minMouseMoveLength &&
                        !_pointerWasOverGameObject)
                    {
                        _cameraIsPanning = true;
                    }

                    // if the camera is currently being panned, to
                    // processing
                    if (_cameraIsPanning)
                    {
                        Vector3 move = (mousePosition - _mousePanStartPos) * _cameraPanScale;
                        Camera.main.transform.position = _cameraPanStartPos - move;
                    }
                }

                // if the main mouse button was released during this frame,
                // stop panning
                if (mouseLeftUp)
                {
                    _cameraIsPanning = false;
                }
            }

            // we clear the check after all processing is done
            if (mouseLeftUp)
                _pointerWasOverGameObject = false;

            // reset per-frame properties
            SkipCurrentFrameMouseClick = false;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get the coordinate that is under the given screen position.
        /// </summary>
        /// <param name="position">The position to look at.</param>
        /// <returns>The coordinate at the screen position.</returns>
        protected Coord? GetSectorAtScreen(Vector3 position)
        {
            Ray rayFromCamera = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;
            if (Physics.Raycast(rayFromCamera, out hit))
                return Layout.Default.PixelToHex(hit.collider.transform.position).Round();
            return null;
        }

        /// <summary>
        /// Select the sector at the given coordinate. It will only select the sector
        /// if it contains a unit that the current player owns.
        /// </summary>
        /// <param name="coord">The coordinate of the sector to select. If null, will deselect.</param>
        protected void SelectSector(Coord? coord)
        {
            if (SelectedSector != null)
                SelectedSector.Highlight = HighlightLevels.None;
            SelectedSector = null;
            if (coord.HasValue && Main.GameContext.Map.Grid.IsTraversable(coord.Value) &&
                Main.GameContext.Map.Grid[coord.Value].OccupyingUnit != null &&
                Main.GameContext.Map.Grid[coord.Value].OccupyingUnit.Owner.Equals(Main.GameContext.CurrentPlayer))
            {
                SelectedSector = Main.GameContext.Map.Grid[coord.Value];
                SelectedSector.Highlight = HighlightLevels.Bright;
            }
        }

        /// <summary>
        /// Selects the range around the given coordinate, exlcuding the given one.
        /// </summary>
        /// <param name="coord">The starting coordinate. If null, will deselect.</param>
        /// <param name="range">The size of the range.</param>
        protected void SelectRangeAround(Coord? coord, int range)
        {
            SelectedRangeHighlight = HighlightLevels.None;
            if (coord.HasValue)
            {
                HashSet<Coord> coordRange = Main.GameContext.Map.Grid.GetRange(coord.Value, range);
                coordRange.Remove(coord.Value);
                SelectedRange.Clear();
                foreach (Coord c in coordRange)
                    if (Main.GameContext.Map.Grid.IsTraversable(c))
                        SelectedRange.Add(Main.GameContext.Map.Grid[c]);
                SelectedRangeHighlight = HighlightLevels.Dimmed;
            }
        }

		public void OpenMenu(string MenuName)
		{
			var target = GameObject.Find (MenuName);
			target.SetActive (true);
		}

		public void CloseMenu(string MenuName)
		{
			var target = GameObject.Find (MenuName);
			target.SetActive (false);
		}
        #endregion
    }
}
