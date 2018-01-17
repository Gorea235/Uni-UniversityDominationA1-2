using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gui
{
    public class GuiManager : MonoBehaviour
    {
        #region Public Properties

        MenuType _currentMenu = MenuType.None;
        public MenuType CurrentMenu
        {
            get { return _currentMenu; }
            set
            {

                _currentMenu = value;
            }
        }

        #endregion

        #region MonoBehaviour

        void Awake()
        {
            
        }
        
        void Start()
        {

        }
        
        void Update()
        {

        }

        #endregion
    }
}
