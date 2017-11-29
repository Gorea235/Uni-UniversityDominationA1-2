using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gui
{
    public class GuiEventArgs : EventArgs
    {
        GuiAction Action { get; }
        Vector3 ClickPosition { get; }

        public GuiEventArgs(GuiAction action, Vector3 position)
        {
            Action = action;
            ClickPosition = position;
        }
    }
}
