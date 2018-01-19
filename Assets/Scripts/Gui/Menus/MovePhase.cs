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
        [SerializeField] private GameObject _BuildMenuPanel;
        [SerializeField] private GameObject _ErrorPanel;


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
                {} // menu active processing
                else
                {
                    SelectSector(null);
                }
            }
        }

        protected override void OnMouseLeftClick(Vector3 position)
        {
            Coord? fetchCoord = GetSectorAtScreen(position);
            Sector selectedPlot = Main.GameContext.Map.Grid[(Coord)fetchCoord];

            Debug.Log(fetchCoord);
            if (SelectedSector == null)
            {
                fetchCoord = null;
            }
            else {
                if (selectedPlot.OccupyingUnit != null)
                {
                    highlightOccupyingUnit((Coord)fetchCoord);

                    if (selectedPlot.OccupyingUnit is Map.Unit.BaseUnit )
                    {
                        BuildMenuButton_OnClick();
                    }

                }
                else
                {
                    SelectSector(fetchCoord,true);
                }
            }

            // SelectRangeAround(fetchCoord, 3);
            //Coord selected = (Coord)fetchCoord;
            //Queue<Coord> path = Main.GameContext.Map.Grid.PathFind(selected, new Coord(selected.Q, selected.R+2));

        }

        //Make a reference to all UI elements that are going to be used in this phase
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

        [Obsolete]
        public void highlightOccupyingUnit(Coord selectedSectorCoord)
        {
            IUnit selectedUnit = Main.GameContext.Map.Grid[selectedSectorCoord].OccupyingUnit;
            if (selectedUnit.Owner.Id == Main.GameContext.CurrentPlayerId && selectedUnit.AvailableMove > 0)
            {
                SelectRangeAround(selectedSectorCoord,selectedUnit.MaxMove);
            }

        }

        public void MoveUnit()
        {

        }

		public void BuildMenuButton_OnClick()
		{
            _BuildMenuPanel.SetActive(true);
        }

        public void CloseBuildMenuButton_OnClick()
        {
            _BuildMenuPanel.SetActive(false);

        }

		public void UpdateMana(int Mana)
		{
			GameObject Mask = GameObject.Find("ManaMask");
			GameObject OverflowDisplay = GameObject.Find("ExtraPintsText");
			Text OverflowText = OverflowDisplay.GetComponent<Text>();

			float MaskScale;
			int Overflow;
			if(Mana > 8)
			{
				MaskScale = 1;
				Overflow = Mana - 8;
			}
			else
			{
				MaskScale = Mana / 8.0F;
				Overflow = 0;
			}
			Debug.Log (MaskScale);
			Mask.transform.localScale = new Vector3(MaskScale, 1, 1);
			OverflowText.text = "+" + Overflow;

		}


        public void BuyAttackUnit_OnClick()
        {
            if (Main.GameContext.CurrentPlayer.Mana >= 3) //check for cost. Arbitrarily 3 for all units
            {

            }
            else
            {
                StartCoroutine(ShowPopUpMessage(2)); // show popup message for two seconds if player has no mana
            }
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


        public void BuyDefenceUnit()
        {
            //TBD
        }
        public void BuyScoutUnit()
        {
            //TBD
        }

        #region Helper methods
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
