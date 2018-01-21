using Map;
using Map.Hex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Menus
{
    /// <summary>
    /// The movement phase menu logic.
    /// Inherits from <see cref="PhaseLogic"/>, and thus <see cref="IMenu"/>.
    /// </summary>
    public class MovePhase : PhaseLogic
    {
        #region Unity Bindings

        // build menu
        public GameObject buildMenuButton;
        public GameObject buildMenuPanel;
        public GameObject buildMenuStatName;
        public GameObject buildMenuStatHp;
        public GameObject buildMenuStatMove;
        public GameObject buildMenuStatMoveCost;
        public GameObject buildMenuStatAttack;
        public GameObject buildMenuStatAttackRange;
        public GameObject buildMenuStatAttackCost;
        public GameObject buildMenuContent;
        public GameObject buildMenuBuyButton;
        public GameObject buildMenuBuyCostText;

        // other
        public GameObject errorPanel;
        public GameObject endPhaseButton;
        public GameObject unitSelectPrefab;

        #endregion

        #region Private Fields

        // predefined
        const string buildMenuButCostTextFormat = "{0} Pints";
        const string buildMenuStatHpFormat = "HP: {0}";
        const string buildMenuStatMoveFormat = "Move Range: {0}";
        const string buildMenuStatMoveCostFormat = "Move Cost: {0} Mana/Move";
        const string buildMenuStatAttackFormat = "Attack: {0}";
        const string buildMenuStatAttackRangeFormat = "Attack Range: {0}";
        const string buildMenuStatAttackCostFormat = "Attack Cost: {0}";
        readonly List<GameObject> _buildMenuItems = new List<GameObject>();

        Coord _selectedUnitLocation;
        RectTransform _buildMenuContentRect;
        bool _isBuildingUnit = false;
        int _unitToBuild;
        HashSet<Sector> _currentSecondaryHighlightedSectors = new HashSet<Sector>();

        #endregion

        #region Private Properties

        bool BuildMenuState
        {
            get { return buildMenuPanel.activeInHierarchy; }
            set
            {
                buildMenuPanel.SetActive(value);
                foreach (GameObject obj in _buildMenuItems)
                    Destroy(obj);
                _buildMenuItems.Clear();
                _buildMenuContentRect.sizeDelta = new Vector2();
                if (value)
                {
                    buildMenuBuyButton.SetActive(false);
                    GameObject tmpObj;
                    RectTransform tmpRect;
                    float offset = 5;
                    for (int i = 0; i < Gc.UnitPrefabs.Length; i++)
                    {
                        if (Gc.DefaultUnits[SelectedUnit.OccupyingUnit.College][i].Buildable)
                        {
                            tmpObj = Instantiate(unitSelectPrefab, buildMenuContent.transform);
                            tmpObj.GetComponent<Button>().onClick.AddListener(GetBuildUnitButtonLambda(i));
                            tmpObj.transform.Find("Icon").GetComponent<Image>().sprite = Gc.DefaultUnits[SelectedUnit.OccupyingUnit.College][i].Icon;
                            tmpObj.transform.Find("Text").GetComponent<Text>().text = Gc.DefaultUnits[SelectedUnit.OccupyingUnit.College][i].Name;
                            tmpRect = tmpObj.GetComponent<RectTransform>();
                            tmpRect.localPosition = new Vector3(5, -offset, 0);
                            offset += tmpRect.rect.height;
                            _buildMenuItems.Add(tmpObj);
                        }
                    }
                    offset += 5;
                    _buildMenuContentRect.sizeDelta = new Vector2(0, offset);
                }
            }
        }

        UnityEngine.Events.UnityAction GetBuildUnitButtonLambda(int i) => () => BuildMenuUnitSelect_OnClick(i);

        #endregion

        #region Public Properties

        /// <summary>
        /// See <see cref="IMenu.IsEnabled"/>.
        /// </summary>
        public override bool IsEnabled
        {
            get
            {
                return gameObject.activeInHierarchy;
            }
            set
            {
                gameObject.SetActive(value);
                sharedPanel.SetActive(value);
                if (value)
                {
                    // setup default menu state
                    BuildMenuState = false;
                    SetUnitBuildStats(null);
                    buildMenuButton.SetActive(false);
                    errorPanel.SetActive(false);
                    endPhaseButton.SetActive(true);
                    manaPanel.SetActive(true);
                    // reset available move on all current units
                    foreach (KeyValuePair<Coord, Sector> kv in Gc.Map.Grid)
                        if (kv.Value.OccupyingUnit != null && kv.Value.OccupyingUnit.Owner.Equals(Gc.CurrentPlayer))
                            kv.Value.OccupyingUnit.AvailableMove = kv.Value.OccupyingUnit.MaxMove;
                    // update mana
                    UpdateMana();
                }
                else
                {
                    DoUnitSelection(null, s => 0); // clear selection state
                }
            }
        }

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            _buildMenuContentRect = buildMenuContent.GetComponent<RectTransform>();
        }

        protected override void Update()
        {
            base.Update();

            Coord? fetchCoord = GetSectorAtScreen(Input.mousePosition);

            // build unit highlighting
            if (_currentSecondaryHighlightedSectors.Count > 0)
            {
                foreach (Sector sector in _currentSecondaryHighlightedSectors)
                    if (sector.Highlight == HighlightLevel.Bright)
                        sector.Highlight = HighlightLevel.Dimmed;
                _currentSecondaryHighlightedSectors.Clear();
            }
            if (_isBuildingUnit)
            {
                if (fetchCoord.HasValue && Gc.Map.Grid.IsTraversable(fetchCoord.Value) && SelectedRange.Contains(Gc.Map.Grid[fetchCoord.Value]))
                {
                    _currentSecondaryHighlightedSectors.Add(Gc.Map.Grid[fetchCoord.Value]);
                    Gc.Map.Grid[fetchCoord.Value].Highlight = HighlightLevel.Bright;
                }
            }
            else if (SelectedUnit != null)
            {
                if (fetchCoord.HasValue && _selectedUnitLocation.DistanceTo(fetchCoord.Value) <= SelectedUnit.OccupyingUnit.AvailableMove &&
                    Gc.Map.Grid.IsTraversable(fetchCoord.Value))
                {
                    Stack<Coord> path = Gc.Map.Grid.PathFind(_selectedUnitLocation, fetchCoord.Value);
                    Coord item;
                    int count = 0;
                    while (count < SelectedUnit.OccupyingUnit.AvailableMove && path.Count > 0)
                    {
                        item = path.Pop();
                        Gc.Map.Grid[item].Highlight = HighlightLevel.Bright;
                        _currentSecondaryHighlightedSectors.Add(Gc.Map.Grid[item]);
                        count++;
                    }
                }
            }
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Overridden from <see cref="PhaseLogic.OnMouseLeftClick(Vector3)"/>.
        /// </summary>
        protected override void OnMouseLeftClick(Vector3 position)
        {
            Coord? fetchCoord = GetSectorAtScreen(position);
            Sector prevUnitSector = SelectedUnit;

            // if we are attempting to build a unit, handle either creation or cancellation
            if (_isBuildingUnit)
            {
                SelectSector(fetchCoord); // we need to know where the player clicked
                if (SelectedSector != null && SelectedRange.Contains(SelectedSector) && SelectedSector.OccupyingUnit == null)
                { // if the player clicked a valid build spot, then build the unit
                    IUnit newUnit = Instantiate(Gc.UnitPrefabs[_unitToBuild]).GetComponent<IUnit>();
                    newUnit.Init(Gc.Map.SectorMaterials, SelectedUnit.OccupyingUnit.Owner, SelectedUnit.OccupyingUnit.College);
                    SelectedSector.OccupyingUnit = newUnit;
                    Gc.CurrentPlayer.Mana -= newUnit.Cost;
                    UpdateMana();
                }
                // we are no longer building
                SetUnitBuildingState(false);
            }
            else
            {
                if (!fetchCoord.HasValue) // if the player clicked off-screen, clear selection
                    DoUnitSelection(null, s => 0);
                else if (ContainsUnit(fetchCoord.Value, true)) // if player clicked on owned unit, shift selection to that one
                {
                    DoUnitSelection(fetchCoord.Value, s => (int)Mathf.Clamp(Mathf.Floor(s.OccupyingUnit.Owner.Mana / s.OccupyingUnit.ManaMoveRatio), 0, s.OccupyingUnit.AvailableMove));
                    if (SelectedUnit != null) // if the player was able to select the unit
                    { // this should pass anyway, but it's good to double check
                        _selectedUnitLocation = fetchCoord.Value;
                    }
                    else
                        Debug.Log("Owned unit selection failed");
                }
                else if (SelectedUnit != null) // if player has clicked a unit before, and has now clicked on a separate space, we need to prepare to move
                {
                    SelectSector(fetchCoord); // try to select the clicked sector
                                              // if it wasn't traversable, or it contains an enemy unit, then we won't bother moving
                    if (SelectedSector != null && !SelectedSectorContainsUnit(true) && !BuildMenuState)
                    { // only move the unit if we are allowed, and are actually looking to move
                        if (SelectedRange.Contains(SelectedSector))
                            MoveUnit(_selectedUnitLocation.DistanceTo(fetchCoord.Value));
                    }
                }

                if (SelectedUnit != null && SelectedUnit.OccupyingUnit.BuildRange > 0) // if this is a builder unit
                {
                    // if player selected a new builder unit, make sure the build menu is close and give the option to open it
                    if (!BuildMenuState || prevUnitSector != SelectedUnit)
                    {
                        buildMenuButton.SetActive(true);
                        BuildMenuState = false;
                    }
                }
                else // if not on a builder unit, then make sure the build menu and open button are hidden
                {
                    buildMenuButton.SetActive(false);
                    BuildMenuState = false;
                }
            }
        }

        /// <summary>
        /// Fired when the 'Open Build Menu' button is clicked.
        /// </summary>
        public void BuildMenuButton_OnClick() => SetBuildMenuState(true);

        /// <summary>
        /// Fired when a unit button in the build menu is clicked. Each handler for the
        /// unit buttons is created with the index of the unit it is referencing.
        /// </summary>
        /// <param name="unitIndex">The index of the unit to start building.</param>
        public void BuildMenuUnitSelect_OnClick(int unitIndex)
        {
            // enable and setup buy button
            buildMenuBuyButton.SetActive(true);
            IUnit unit = Gc.DefaultUnits[SelectedUnit.OccupyingUnit.College][unitIndex];
            buildMenuBuyButton.GetComponent<Button>().interactable = unit.Cost <= Gc.CurrentPlayer.Mana;
            buildMenuBuyCostText.GetComponent<Text>().text = string.Format(buildMenuButCostTextFormat, unit.Cost);
            // setup stats
            SetUnitBuildStats(unit);
            _unitToBuild = unitIndex;
        }

        /// <summary>
        /// Fired when the 'Buy Unit' button is clicked.
        /// </summary>
        public void BuildMenuBuyButton_OnClick() => SetUnitBuildingState(true);

        /// <summary>
        /// Fired when the 'Close Build Menu' button is clicked.
        /// </summary>
        public void CloseBuildMenuButton_OnClick() => SetBuildMenuState(false);

        /// <summary>
        /// Fired when the 'End Phase' button is clicked.
        /// </summary>
        public void EndPhaseButton_OnClick() => Gc.Gui.CurrentMenu = MenuType.AttackPhase;

        #endregion

        #region Helpers

        /// <summary>
        /// Sets the state of the build menu.
        /// </summary>
        /// <param name="state">Whether the build menu should be opened or closed.</param>
        void SetBuildMenuState(bool state)
        {
            if (state)
            {
                buildMenuButton.SetActive(false); // we don't need the open button any more
                BuildMenuState = true; // we need the build menu open now
            }
            else
            {
                BuildMenuState = false; // we need the build menu closed
                DoUnitSelection(null, s => 0); // we will delect the unit
            }
        }

        /// <summary>
        /// Sets whether we are looking to build a unit or not.
        /// </summary>
        /// <param name="state"></param>
        void SetUnitBuildingState(bool state)
        {
            _isBuildingUnit = state;
            if (state) // if we are, then we need to select the build range and disable the build menu
            {
                BuildMenuState = false;
                SelectRangeAround(_selectedUnitLocation, SelectedUnit.OccupyingUnit.BuildRange);
                SelectedRangeHighlight = HighlightLevel.Dimmed;
            }
            else
                DoUnitSelection(null, s => 0); // otherwise just clear the build range
        }

        /// <summary>
        /// Sets the stats shown in the build menu to the given unit's stats.
        /// If null, then the stats are cleared.
        /// </summary>
        /// <param name="unit">The unit to set the stats to.</param>
        void SetUnitBuildStats(IUnit unit)
        {
            // we use the C# null propergators and checkers to allow a null unit to clear the stats
            buildMenuStatName.GetComponent<Text>().text = unit?.Name ?? "";
            buildMenuStatHp.GetComponent<Text>().text = string.Format(buildMenuStatHpFormat, unit?.Health.ToString() ?? "");
            buildMenuStatMove.GetComponent<Text>().text = string.Format(buildMenuStatMoveFormat, unit?.MaxMove.ToString() ?? "");
            buildMenuStatMoveCost.GetComponent<Text>().text = string.Format(buildMenuStatMoveCostFormat, unit?.ManaMoveRatio.ToString() ?? "");
            buildMenuStatAttack.GetComponent<Text>().text = string.Format(buildMenuStatAttackFormat, unit?.Attack.ToString() ?? "");
            buildMenuStatAttackRange.GetComponent<Text>().text = string.Format(buildMenuStatAttackRangeFormat, unit?.AttackRange.ToString() ?? "");
            buildMenuStatAttackCost.GetComponent<Text>().text = string.Format(buildMenuStatAttackCostFormat, unit?.ManaAttackCost.ToString() ?? "");
        }

        /// <summary>
        /// Using the <see cref="PhaseLogic.SelectedUnit"/> as the unit to move, and the
        /// <see cref="PhaseLogic.SelectedSector"/> as the target sector, it will perform the 
        /// standard movement calculations and actions.
        /// </summary>
        /// <param name="distance">The distance that the unit was moved.</param>
        void MoveUnit(int distance)
        {
            SelectedUnit.OccupyingUnit.AvailableMove -= distance;
            SelectedSector.OccupyingUnit = SelectedUnit.OccupyingUnit;
            SelectedUnit.OccupyingUnit.Owner.Mana -= distance * SelectedUnit.OccupyingUnit.ManaMoveRatio;
            SelectedUnit.OccupyingUnit = null;
            DoUnitSelection(null, s => 0);
            UpdateMana();
        }

        /// <summary>
        /// Display a block bar on top of screen if there is an error.
        /// TBD: modify so it takes a string parameter and customises the error message
        /// </summary>
        /// <param name="delay">The time to show the bar for.</param>
        /// <returns>The enumerator for unit concurrency.</returns>
        IEnumerator ShowPopUpMessage(float delay)
        {
            errorPanel.SetActive(true);
            yield return new WaitForSeconds(delay);
            errorPanel.SetActive(false);
        }

        #endregion
    }
}
