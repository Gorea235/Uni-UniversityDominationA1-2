using System;
using System.Collections;
using Map.Hex;
using UnityEngine;
using Manager;
using Map;
using System.Collections.Generic;

namespace Gui.Menus
{
    public class MovePhase : PhaseLogic
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
            }
            else
            {
                SelectSector(fetchCoord);
            }

           // SelectRangeAround(fetchCoord, 3);
            //Coord selected = (Coord)fetchCoord;
            //Queue<Coord> path = Main.GameContext.Map.Grid.PathFind(selected, new Coord(selected.Q, selected.R+2));

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
