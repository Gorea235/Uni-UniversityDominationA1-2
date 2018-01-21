using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gui
{
    /// <summary>
    /// The generic menu interface to enable equal handling.
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Whether the menu is enabled or not.
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
