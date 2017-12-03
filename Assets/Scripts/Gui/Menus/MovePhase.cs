using System;
using System.Collections;
using UnityEngine;

namespace Gui.Menus
{
    public class MovePhase : PhaseLogic
    {
        protected override void OnMouseLeftClick(Vector3 position)
        {
            throw new NotImplementedException();
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
