using System.Collections.Generic;
using UnityEngine;

namespace Gui
{
    public class GuiManager : MonoBehaviour
    {
        #region Unity Bindings

        public GameObject menuMove;
        public GameObject menuAttack;

        #endregion

        #region Private Fields

        Dictionary<MenuType, IMenu> _menus = new Dictionary<MenuType, IMenu>();

        #endregion

        #region Public Properties

        MenuType _currentMenu = MenuType.None;
        /// <summary>
        /// Sets the current menu.
        /// </summary>
        public MenuType CurrentMenu
        {
            get { return _currentMenu; }
            set
            {
                SetCurrentMenuState(false);
                _currentMenu = value;
                SetCurrentMenuState(true);
            }
        }

        #endregion

        #region MonoBehaviour
        
        void Awake()
        {
            // init menus
            FetchMenu(MenuType.MovePhase, menuMove);
            FetchMenu(MenuType.AttackPhase, menuAttack);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// A shorthand function to set up the given UI <see cref="GameObject"/> in the dictionary.
        /// </summary>
        /// <param name="menu">The <see cref="MenuType"/> enum that the given menu is referencing.</param>
        /// <param name="obj">The <see cref="GameObject"/> that the enum refereces.</param>
        void FetchMenu(MenuType menu, GameObject obj) => _menus.Add(menu, obj.GetComponent<IMenu>());

        /// <summary>
        /// Sets the current menu state. If <see cref="MenuType.None"/>, then it is ignored.
        /// </summary>
        /// <param name="state">The state to set the menu.</param>
        void SetCurrentMenuState(bool state)
        {
            if (_currentMenu != MenuType.None)
                _menus[_currentMenu].IsEnabled = state;
        }

        #endregion
    }
}
