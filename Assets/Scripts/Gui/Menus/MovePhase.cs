using Helpers;
using Map;
using Map.Hex;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Menus
{
    public class MovePhase : PhaseLogic
    {
        #region Unity Bindings

        public GameObject buildMenuPanel;
        public GameObject buildMenuButton;
        public GameObject errorPanel;
        public GameObject endPhaseButton;
        public GameObject manaPanel;
        public GameObject manaPanelMask;
        public GameObject manaPanelText;

        #endregion

        #region Private Fields

        const string manaPanelTextFormat = " ({0})";

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
                    // setup default menu state
                    buildMenuPanel.SetActive(false);
                    buildMenuButton.SetActive(false);
                    errorPanel.SetActive(false);
                    endPhaseButton.SetActive(true);
                    manaPanel.SetActive(true);
                }
                else
                {
                    DoUnitSelection(null, s => 0); // clear selection state
                }
            }
        }

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            
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
            Sector prevUnitSector = SelectedUnit;

            if (!fetchCoord.HasValue) // if the player clicked off-screen, clear selection
                DoUnitSelection(null, s => 0);
            else if (ContainsUnit(fetchCoord.Value, true)) // if player clicked on owned unit, shift selection to that one
            {
                DoUnitSelection(fetchCoord.Value, s => s.OccupyingUnit.AvailableMove);
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
                // if it wasn't traversable, or it contains an enemy unit, then we won't bother moving
                if (SelectedSector != null && !SelectedSectorContainsUnit(true))
                {
                    // do movement here
                }
            }

            if (SelectedUnit != null && SelectedUnit.OccupyingUnit.BuildRange > 0) // if this is a builder unit
            {
                if (!buildMenuPanel.activeInHierarchy || prevUnitSector != SelectedUnit)
                {
                    buildMenuButton.SetActive(true);
                    buildMenuPanel.SetActive(false);
                }
            }
            else
            {
                buildMenuButton.SetActive(false);
                buildMenuPanel.SetActive(false);
            }
        }

        public void BuildMenuButton_OnClick() => SetBuildMenuState(true);

        public void CloseBuildMenuButton_OnClick() => SetBuildMenuState(false);

        public void EndPhaseButton_OnClick() => Debug.Log("end phase fired");

        #endregion

        #region Helpers

        void SetBuildMenuState(bool state)
        {
            if (state)
            {
                buildMenuButton.SetActive(false);
                buildMenuPanel.SetActive(true);
            }
            else
            {
                buildMenuPanel.SetActive(false);
                DoUnitSelection(null, s => 0);
            }
        }

        public void MoveUnit(Coord fromPosition, Coord targetPosition)
        {
            System.Collections.Generic.HashSet<Coord> range = Gc.Map.Grid.GetRange(fromPosition, Gc.Map.Grid[fromPosition].OccupyingUnit.MaxMove);
            float step = Gc.Map.Grid[fromPosition].OccupyingUnit.MaxMove * Time.deltaTime; //speed of step made to scale with unit speed

            if (range.Contains(targetPosition)) //see if position to go to is in range of our unit
            {
                Vector3 current = Gc.Map.Grid[fromPosition].transform.position; //get the Vector3 position of the first plot in our path, which is the plot our unit is currently on
                Vector3 next = new Vector3();
                System.Collections.Generic.Queue<Coord> path = Gc.Map.Grid.PathFind(fromPosition, targetPosition); //get the path
                foreach (Coord plot in path)
                {
                    next = Gc.Map.Grid[plot].transform.position; //get the next plot in our path
                    Gc.Map.Grid[fromPosition].OccupyingUnit.Transform.position = Vector3.MoveTowards(current, next, step); //move unit to next plot in path
                    current = next; //reset so current to point at the one we are now at

                }

                Gc.Map.Grid[targetPosition].OccupyingUnit = Gc.Map.Grid[fromPosition].OccupyingUnit; //bind target position to have the transitioned unit
                Gc.Map.Grid[fromPosition].OccupyingUnit = null; //remove binding from start position


            }
            else { } //handle 

        }

        public void UpdateMana()
        {
            float manaPercent = Gc.CurrentPlayer.Mana / Gc.CurrentPlayer.MaxMana;
            Debug.Log(string.Format("Player:{0} mana shown as {1:P2}", Gc.CurrentPlayer.Id, manaPercent));
            manaPanelMask.transform.localScale = new Vector3(manaPercent, 1, 1);
            manaPanelText.GetComponent<Text>().text = string.Format(manaPanelTextFormat, Gc.CurrentPlayer.Mana);
        }

        ///display a block bar on top of screen if there is an error
        ///TBD: modify so it takes a string parameter and customises the error message
        IEnumerator ShowPopUpMessage(float delay)
        {
            errorPanel.SetActive(true);
            yield return new WaitForSeconds(delay);
            errorPanel.SetActive(false);
        }

        #endregion
    }
}
