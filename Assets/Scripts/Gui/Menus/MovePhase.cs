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
        [SerializeField] private GameObject _panel;


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

            SelectSector(fetchCoord);
            if (SelectedSector == null)
                fetchCoord = null;

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
                SelectSector(fetchCoord);
            }

           // SelectRangeAround(fetchCoord, 3);
            //Coord selected = (Coord)fetchCoord;
            //Queue<Coord> path = Main.GameContext.Map.Grid.PathFind(selected, new Coord(selected.Q, selected.R+2));

        }

        //Make a reference to all UI elements that are going to be used in this phase
        private void Start()
        {
            _panel = GameObject.Find("BuildListPanel");
            _panel.SetActive(false); //hide panel at start of game
        }

        protected override void Update()
        {
            base.Update();
        }

        public void TestButton_OnClick()
        {
            // event handlers can options have a string arg
            // e.g. 'public void TestButton_OnClick(string arg) { }'
            // when setting up the OnClick handler, you can give the string to
            // be passed in (leaving it blank would pass in a string of "",
            // which wouldn't be too helpful).
            Debug.Log("button click event fired");
            SkipCurrentFrameMouseClick = true;
        }

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
            _panel.SetActive(true);
        }

        public void CloseBuildMenuButton_OnClick()
        {
            _panel.SetActive(false);

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


        public void BuyAttackUnit()
        {
            //TBD
        }
        public void BuyDefenceUnit()
        {
            //TBD
        }
        public void BuyScoutUnit()
        {
            //TBD
        }
    }
}
