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
        
        void Start()
        {
            // init menus
            FetchMenu(MenuType.MovePhase, menuMove);
            FetchMenu(MenuType.AttackPhase, menuAttack);
        }

        #endregion

        #region Helpers

        void FetchMenu(MenuType menu, GameObject obj) => _menus.Add(menu, obj.GetComponent<IMenu>());

        void SetCurrentMenuState(bool state)
        {
            if (_currentMenu != MenuType.None)
                _menus[_currentMenu].IsEnabled = state;
        }

        #endregion
    }
}
