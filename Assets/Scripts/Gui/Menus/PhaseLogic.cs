using System;
using System.Collections;
using Manager;
using Map;
using Map.Hex;
using UnityEngine;
using System.Collections.Generic;

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
        /// Whether the camera is currently panning
        /// </summary>
        bool _cameraIsPanning = false;

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

        protected Sector SelectedSector { get; private set; }

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

            // screen click processing
            if (DoScreenClickCheck && !SkipCurrentFrameMouseClick)
            {
                // if the mouse was released and we were not panning
                // the camera, fire the click method
                if (mouseLeftUp && !_cameraIsPanning)
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
                    if (!_cameraIsPanning && (mousePosition - _mousePanStartPos).magnitude > _minMouseMoveLength)
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

            // reset per-frame properties
            SkipCurrentFrameMouseClick = false;
        }

        #endregion

        #region Helper Methods

        protected Coord? GetSectorAtScreen(Vector3 position)
        {
            Ray rayFromCamera = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;
            if (Physics.Raycast(rayFromCamera, out hit))
                return Layout.Default.PixelToHex(hit.collider.transform.position).Round();
            return null;
        }

        protected void SelectSector(Coord? coord)
        {
            if (SelectedSector != null)
                SelectedSector.Highlighted = false;
            if (coord.HasValue)
            {
                SelectedSector = Main.GameContext.Map.Grid[(Coord)coord];
                if (SelectedSector.Traversable)
                    SelectedSector.Highlighted = true;
                else
                    SelectedSector = null;
            }
            else
                SelectedSector = null;
        }

        protected void SelectSector(Coord? coord, int movementRange)
        {

            if (SelectedSector != null)
                SelectedSector.Highlighted = false;
            if (coord.HasValue)
            {
                HashSet<Coord> range = Main.GameContext.Map.Grid.MovementRange((Coord)coord, movementRange);

                foreach (Coord plot in range)
                {
                    SelectedSector = Main.GameContext.Map.Grid[plot];
                    SelectedSector.Highlighted = true;
                }
            }
            else
                SelectedSector = null;
        }

        #endregion
    }
}
