using System;
using System.Collections;
using Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gui.Menus
{
    public class AttackPhase : PhaseLogic
    {
        #region Private Fields

        readonly Color _brightTint = new Color(1, 0.4f, 0.4f);
        readonly Color _dimmedTint = new Color(1f, 0.4f, 0.4f);
        Color _oldBrightTint;
        Color _oldDimmedTint;

        #endregion

        #region Public Properties

        public override bool IsEnabled
        {
            get
            {
                return gameObject.activeInHierarchy;
            }
            set
            {
                gameObject.SetActive(value);
                if (value)
                {
                    _oldBrightTint = Main.GameContext.Map.SectorMaterials.GetHighlightTint(HighlightLevel.Bright);
                    _oldDimmedTint = Main.GameContext.Map.SectorMaterials.GetHighlightTint(HighlightLevel.Dimmed);
                    SetHighlightTints(_brightTint, _dimmedTint);
                }
                else
                {
                    SetHighlightTints(_oldBrightTint, _oldDimmedTint);
                }
            }
        }

        #endregion

        #region Handlers

        protected override void OnMouseLeftClick(Vector3 position)
        {
            // will re-orginise to allow selection of one unit then selection of unit to attack
            DoSectorSelection(position, sector => sector.OccupyingUnit.AttackRange);
            if (SelectedSector != null)
            {
                // do other selection processing if needed
            }
        }

        #endregion

        #region Helpers

        void SetHighlightTints(Color bright, Color dimmed)
        {
            Main.GameContext.Map.SectorMaterials.SetHighlightTint(HighlightLevel.Bright, bright);
            Main.GameContext.Map.SectorMaterials.SetHighlightTint(HighlightLevel.Dimmed, dimmed);
        }

        #endregion
    }
}
