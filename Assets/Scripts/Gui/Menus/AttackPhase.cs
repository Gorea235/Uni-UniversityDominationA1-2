using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gui.Menus
{
    public class AttackPhase : PhaseLogic
    {
        public override bool IsEnabled
        {
            get
            {
                return gameObject.activeInHierarchy;
            }
            set
            {
                gameObject.SetActive(value);
            }
        }

        protected override void OnMouseLeftClick(Vector3 position)
        {
            Debug.Log(GetSectorAtScreen(position));
        }
    }
}
