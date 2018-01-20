using Helpers;
using Map;
using Map.Hex;
using UnityEngine;

namespace Gui.Menus
{
    public class AttackPhase : PhaseLogic
    {
        #region Private Fields

        readonly Color _brightTint = new Color(1, 0.8f, 0.8f);
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

            Coord? fetchCoord = GetSectorAtScreen(position);

            if (!fetchCoord.HasValue) // if the player clicked off-screen, clear selection
                DoUnitSelection(null, s => 0);
            else if (ContainsUnit(fetchCoord.Value, true)) // if player clicked on owned unit, shift selection to that one
            {
                DoUnitSelection(fetchCoord.Value, s => s.OccupyingUnit.AttackRange);
                if (SelectedUnit != null) // if the player was able to select the unit
                { // this should pass anyway, but it's good to double check
                    // unit just selected
                }
                else
                    Debug.Log("Owned unit selection failed");
            }
            else if (SelectedUnit != null) // if player has clicked a unit before, and has now clicked on a separate space, we need to prepare to move
            {
                SelectSector(fetchCoord); // try to select the clicked sector
                // only consider an attack selection if it wasn't traversable and if an enemy was on it
                if (SelectedSector != null && SelectedSectorContainsUnit(true))
                {
                    // do attack setup here
                }
            }
        }

        public void ConfirmAttackButton_OnClick() => Debug.Log("confirm attack fired");

        public void EndTurnButton_OnClick() => Debug.Log("end turn fired");

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
