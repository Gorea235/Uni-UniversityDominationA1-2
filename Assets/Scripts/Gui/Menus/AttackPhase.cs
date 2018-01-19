using Helpers;
using Map;
using UnityEngine;

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
                    _oldBrightTint = Gc.Map.SectorMaterials.GetHighlightTint(HighlightLevel.Bright);
                    _oldDimmedTint = Gc.Map.SectorMaterials.GetHighlightTint(HighlightLevel.Dimmed);
                    SetHighlightTints(_brightTint, _dimmedTint);
                }
                else
                {
                    SetHighlightTints(_oldBrightTint, _oldDimmedTint);
                    DoUnitSelection(null, s => 0); // clear selection state
                }
            }
        }

        #endregion

        #region Handlers

        protected override void OnMouseLeftClick(Vector3 position)
        {
            // will re-orginise to allow selection of one unit then selection of unit to attack
            DoUnitSelection(position, sector => sector.OccupyingUnit.AttackRange);
            if (SelectedUnit != null)
            {
                // do other selection processing if needed
            }
        }

        #endregion

        #region Helpers

        void SetHighlightTints(Color bright, Color dimmed)
        {
            Gc.Map.SectorMaterials.SetHighlightTint(HighlightLevel.Bright, bright);
            Gc.Map.SectorMaterials.SetHighlightTint(HighlightLevel.Dimmed, dimmed);
        }

        #endregion
    }
}
