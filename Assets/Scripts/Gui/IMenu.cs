using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gui
{
    public interface IMenu
    {
        event EventHandler<GuiEventArgs> OnAction;
        bool IsEnabled { get; set; }
    }
}
