using System;
using System.Collections;
using UnityEngine;

namespace Gui.Menus
{
    public class MovePhase : PhaseLogic
    {

        RaycastHit selectedSector;

        protected override void OnMouseLeftClick(Vector3 position)
        {
            Ray rayFromCamera = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(rayFromCamera, out selectedSector))
            {
                Collider sectorCollider = selectedSector.collider;
                sectorCollider.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }
            Debug.DrawRay(rayFromCamera.origin, rayFromCamera.direction*50, Color.yellow); // *50 modifier is just so its visible in the Editor. Note that it won't display in game view
            //throw new NotImplementedException();
        }

        protected override void Update()
        {
            base.Update();
        }

        public void TestButton_OnClick()
        {
            // event handlers can options have a string arg
            // e.g. 'public void TestButton_OnClick(string arg) { }'
            // when setting up the OnClick handler, you can give the string to
            // be passed in (leaving it blank would pass in a string of "",
            // which wouldn't be too helpful).
            Debug.Log("button click event fired");
            SkipCurrentFrameMouseClick = true;
        }
    }
}
