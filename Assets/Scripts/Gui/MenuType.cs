using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gui
{
    /// <summary>
    /// The enum for specifying menus. Each item (execpt None) refer directly to a menu available in the scene.
    /// (The menu system could be altered to use different scenes, in which cause we will need to work out
    /// whether to switch scene).
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// No menu.
        /// </summary>
        None,
        /// <summary>
        /// The movement phase menu.
        /// </summary>
        MovePhase,
        /// <summary>
        /// The attack phase menu.
        /// </summary>
        AttackPhase
    }
}
