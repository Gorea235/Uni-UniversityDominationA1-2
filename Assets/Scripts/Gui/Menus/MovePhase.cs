using UnityEngine;
using System.Collections;
using System;

namespace Gui.Menus
{
    public class MovePhase : MonoBehaviour, IMenu
    {
        public event EventHandler<GuiEventArgs> OnAction;

        public bool IsEnabled { get; set; }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
