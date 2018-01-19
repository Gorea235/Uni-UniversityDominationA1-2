using System;
using System.Collections;
using Map.Hex;
using UnityEngine;
using Manager;
using Map;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Gui.Menus
{
    public class MovePhase : PhaseLogic
    {
        #region Private Fields

        GameObject _BuildMenuPanel;
        GameObject _ErrorPanel;

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
                { } // menu active processing
                else
                {
                    SelectSector(null);
                }
            }
        }

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            //hide panels at start of turn
            _BuildMenuPanel.SetActive(false);
            _ErrorPanel.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();
        }

        #endregion

        #region Handlers

        protected override void OnMouseLeftClick(Vector3 position)
        {
            Coord? fetchCoord = GetSectorAtScreen(position);
            if (!fetchCoord.HasValue) // if the player clicked off-screen, clear selection
                DoUnitSelection(null, s => 0);
            else if (ContainsUnit(fetchCoord.Value, true)) // if player clicked on owned unit, shift selection to that one
            {
                DoUnitSelection(fetchCoord.Value, s => s.OccupyingUnit.AvailableMove);
                if (SelectedUnit != null) // if the player was able to select the unit
                { // this should pass anyway, but it's good to double check
                    if (SelectedUnit.OccupyingUnit.BuildRange > 0) // if this is a builder unit
                    {
                        // enable the open build menu button
                    }
                }
                else
                    Debug.Log("Owned unit selection failed");
            }
            else if (SelectedUnit != null) // if player has clicked a unit before, and has now clicked on a separate space, we need to prepare to move
            {
                SelectSector(fetchCoord); // try to select the clicked sector
                // if it wasn't traversable, or it contains an enemy unit, then we won't bother moving
                if (SelectedSector != null && !SelectedSectorContainsUnit(true))
                {
                    // do movement here
                }
            }

        }

        public void BuildMenuButton_OnClick()
        {
            _BuildMenuPanel.SetActive(true);
        }

        public void CloseBuildMenuButton_OnClick()
        {
            _BuildMenuPanel.SetActive(false);

        }

        #endregion

        #region Helpers

        public void MoveUnit()
        {

        }

        public void UpdateMana(int Mana)
        {
            GameObject Mask = GameObject.Find("ManaMask");
            GameObject OverflowDisplay = GameObject.Find("ExtraPintsText");
            Text OverflowText = OverflowDisplay.GetComponent<Text>();

            float MaskScale;
            int Overflow;
            if (Mana > 8)
            {
                MaskScale = 1;
                Overflow = Mana - 8;
            }
            else
            {
                MaskScale = Mana / 8.0F;
                Overflow = 0;
            }
            Debug.Log(MaskScale);
            Mask.transform.localScale = new Vector3(MaskScale, 1, 1);
            OverflowText.text = "+" + Overflow;

        }

        public void SpawnAttackUnit(Vector3 positionOfSelectedBase)
        {
            Coord? selectedBase = GetSectorAtScreen(positionOfSelectedBase);
            HashSet<Coord> possibleBuildPositions = Main.GameContext.Map.Grid.GetRange((Coord)selectedBase, 2); //get possible building positions in range of the College sectors

            IUnit spawnedAttack = Instantiate(Main.GameContext.Map.AttackUnit).GetComponent<IUnit>();
            spawnedAttack.Init(Main.GameContext.Map.SectorMaterials, Main.GameContext.CurrentPlayer, College.Alcuin); //TBD: FIND A WAY TO REFERENCE THE COLLEGE FROM THE SECTOR MATERIAL

            foreach (Coord collegePlot in possibleBuildPositions)
            {
                if (Main.GameContext.Map.Grid[collegePlot].OccupyingUnit == null)
                {
                    Main.GameContext.Map.Grid[collegePlot].OccupyingUnit = spawnedAttack;
                    break;
                }
            }
        }

        ///display a block bar on top of screen if there is an error
        ///TBD: modify so it takes a string parameter and customises the error message
        IEnumerator ShowPopUpMessage(float delay)
        {
            _ErrorPanel.SetActive(true);
            yield return new WaitForSeconds(delay);
            _ErrorPanel.SetActive(false);
        }

        #endregion
    }
}
