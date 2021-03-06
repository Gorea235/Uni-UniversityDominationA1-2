using System;
using System.Collections;
using Manager;
using Map;
using Map.Hex;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gui
{
    public abstract class PhaseLogic : MonoBehaviour, IMenu
    {
        #region Unity Bindings

        public GameObject sharedPanel;

        // mana
        public GameObject manaPanel;
        public GameObject manaPanelMask;
        public GameObject manaPanelText;

        #endregion

        #region Private Fields

        const string manaPanelTextFormat = " {0}";

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
        /// Whether the pointer was over a UI element when it was clicked.
        /// </summary>
        bool _pointerWasOverGameObject = false;

        #endregion

        #region Protected Properties

        /// <summary>
        /// The main game manager.
        /// </summary>
        protected MainManager Main { get; private set; }

        /// <summary>
        /// A shorthand for the <see cref="MainManager.GameContext"/> object.
        /// </summary>
        protected Context Gc { get { return Main.GameContext; } }

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
        /// The currently selected unit.
        /// </summary>
        protected Sector SelectedUnit { get; private set; }
        /// <summary>
        /// The currently selected range.
        /// </summary>
        protected HashSet<Sector> SelectedRange { get; private set; } = new HashSet<Sector>();
        /// <summary>
        /// A shorthand for setting the <see cref="SelectedRange"/> highlight.
        /// </summary>
        protected HighlightLevel SelectedRangeHighlight
        {
            set
            {
                foreach (Sector sector in SelectedRange)
                    if (sector != SelectedUnit)
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

        /// <summary>
        /// Used to fetch the <see cref="MainManager"/> class to allow access to the
        /// <see cref="Context"/> object.
        /// </summary>
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

        #region Helpers

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
        /// Tests whether the given sector contains a unit or not.
        /// </summary>
        /// <param name="sector">The sector to test.</param>
        /// <param name="requireOwned">Whether to restrict valid units to owned ones only.</param>
        /// <returns>Whether the sector contains a unit.</returns>
        protected bool ContainsUnit(Sector sector, bool requireOwned) => sector.OccupyingUnit != null &&
            (!requireOwned || sector.OccupyingUnit.Owner.Equals(Gc.CurrentPlayer));

        /// <summary>
        /// Tests whether the sector at the given coordinate contains a unit or not.
        /// </summary>
        /// <param name="coord">The coordinate to test.</param>
        /// <param name="requireOwned">Whether to restrict valid units to owned ones only.</param>
        /// <returns>Whether the sector at the given coordinate contains a unit.</returns>
        protected bool ContainsUnit(Coord coord, bool requireOwned) => Gc.Map.Grid.IsTraversable(coord) &&
            ContainsUnit(Gc.Map.Grid[coord], requireOwned);

        /// <summary>
        /// Tests whether the currently selected sector contains a unit in.
        /// </summary>
        /// <param name="excludeOwned">Whether to exclude owned units.</param>
        /// <returns>Whether the currently selected sector contains a unit.</returns>
        protected bool SelectedSectorContainsUnit(bool excludeOwned) => ContainsUnit(SelectedSector, false) &&
            (!excludeOwned || !SelectedSector.OccupyingUnit.Owner.Equals(Gc.CurrentPlayer));

        /// <summary>
        /// Select the sector at the given coordinate. If unit is true, t will only select the sector
        /// if it contains a unit that the current player owns.
        /// </summary>
        /// <param name="coord">The coordinate of the sector to select. If null, will deselect.</param>
        /// <param name="unit">Whether to force the selection regarless of if an owned unit is present.</param>
        protected void SelectSector(Coord? coord, bool unit = false)
        {
            if (unit)
                SelectedUnit = null;
            SelectedSector = null;
            if (coord.HasValue && Gc.Map.Grid.IsTraversable(coord.Value))
            {
                Sector selection = Gc.Map.Grid[coord.Value];
                if (unit)
                {
                    if (ContainsUnit(selection, true))
                        SelectedUnit = selection;
                }
                else
                    SelectedSector = selection;
            }
        }

        /// <summary>
        /// A shorthand for <c>SelectSector(coord, true)</c>.
        /// </summary>
        /// <param name="coord"></param>
        protected void SelectUnit(Coord? coord) => SelectSector(coord, true);

        /// <summary>
        /// Selects the range around the given coordinate, exlcuding the given one.
        /// </summary>
        /// <param name="coord">The starting coordinate. If null, will deselect.</param>
        /// <param name="range">The size of the range.</param>
        protected void SelectRangeAround(Coord? coord, int range)
        {
            SelectedRange.Clear();
            if (coord.HasValue)
            {
                HashSet<Coord> coordRange = Gc.Map.Grid.GetRange(coord.Value, range);
                coordRange.Remove(coord.Value);
                foreach (Coord c in coordRange)
                    if (Gc.Map.Grid.IsTraversable(c))
                        SelectedRange.Add(Gc.Map.Grid[c]);
            }
        }

        /// <summary>
        /// Reset the hightlights on all currently selected sectors.
        /// </summary>
        protected void ClearHighlights()
        {
            if (SelectedUnit != null)
                SelectedUnit.Highlight = HighlightLevel.None;
            SelectedRangeHighlight = HighlightLevel.None;
        }

        /// <summary>
        /// Does the owned unit selection cycle.
        /// Cycle:
        ///     Clear current selection highlights,
        ///     Select sector at coord,
        ///     if selected, select the range using the range function.
        /// </summary>
        /// <param name="coord">The coordinate to centre the selection at.</param>
        /// <param name="range">The range function used to calculated the selection range from the given sector.</param>
        protected void DoUnitSelection(Coord? coord, Func<Sector, int> range)
        {
            ClearHighlights();
            SelectUnit(coord);
            if (SelectedUnit != null)
            {
                SelectedUnit.Highlight = HighlightLevel.Bright;
                SelectRangeAround(coord, range(SelectedUnit));
                SelectedRangeHighlight = HighlightLevel.Dimmed;
            }
        }

        /// <summary>
        /// Does the owned unit selection cycle using the mouse position, rather than the coordinate.
        /// </summary>
        /// <param name="position">The position that was clicked.</param>
        /// <param name="range">The range function used to calculated the selection range from the given sector.</param>
        protected void DoUnitSelection(Vector3 position,
                                         Func<Sector, int> range) => DoUnitSelection(GetSectorAtScreen(position), range);

        /// <summary>
        /// Updates the mana bar with the current players mana.
        /// </summary>
        protected void UpdateMana()
        {
            float manaPercent = (float)Gc.CurrentPlayer.Mana / Gc.CurrentPlayer.MaxMana;
            manaPanelMask.transform.localScale = new Vector3(manaPercent, 1, 1);
            manaPanelText.GetComponent<Text>().text = string.Format(manaPanelTextFormat, Gc.CurrentPlayer.Mana);
        }

        #endregion
    }
}
