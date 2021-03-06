﻿using Gui;
using Manager.Players;
using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class MainManager : MonoBehaviour
    {
        #region Unity Bindings

        // unit bindings
        public GameObject AttackUnit;
        public GameObject BaseUnit;
        public GameObject DefenceUnit;
        public GameObject ScoutUnit;

        #endregion

        #region Properties

        /// <summary>
        /// The overall game context. This acts as the entry point to all of the information
        /// of the current game state.
        /// </summary>
        public Context GameContext { get; private set; }

        #endregion

        #region MonoBehaviour

        void Awake()
        {
            // ===== initialise unit variables =====
            IPlayer gamePlayer = new AiPlayer(-1); // init a base game player
            GameObject[] unitPrefabs = new[] { AttackUnit, BaseUnit, DefenceUnit, ScoutUnit };
            Dictionary<College, Dictionary<int, IUnit>> defaultUnits = new Dictionary<College, Dictionary<int, IUnit>>();
            IUnit tmpUnit;
            foreach (College college in Enum.GetValues(typeof(College)))
            {
                Dictionary<int, IUnit> currentCollege = new Dictionary<int, IUnit>();
                for (int i = 0; i < unitPrefabs.Length; i++)
                {
                    tmpUnit = Instantiate(unitPrefabs[i], gameObject.transform).GetComponent<IUnit>();
                    tmpUnit.Init(gamePlayer, college);
                    tmpUnit.Transform.localPosition = new Vector3();
                    currentCollege.Add(i, tmpUnit);
                }
                defaultUnits.Add(college, currentCollege);
            }

            // pull in the game context
            // done last to collect game state
            GameContext = new Context(
                GameObject.Find("Gui").GetComponent<GuiManager>(),
                GameObject.Find("Map").GetComponent<MapManager>(),
                gameObject.GetComponent<AudioManager>(),
                unitPrefabs, defaultUnits);
            GameContext.Players.Add(gamePlayer.Id, gamePlayer);

            // ===== testing code =====
            // basic default players
            // for full game, the menu that deals with allowing users to set up the game
            // and available players in said game will add each player instance.
            IPlayer tmp = new HumanPlayer(GameContext.GetNewPlayerId())
            {
                Mana = 120,
                MaxMana = 120
            };
            GameContext.Players.Add(tmp.Id, tmp);
            GameContext.PlayerOrder.Enqueue(tmp.Id);
            tmp = new HumanPlayer(GameContext.GetNewPlayerId())
            {
                Mana = 120,
                MaxMana = 120
            };
            GameContext.Players.Add(tmp.Id, tmp);
            GameContext.PlayerOrder.Enqueue(tmp.Id);
            GameContext.StartingPlayerId = GameContext.CurrentPlayerId;
        }

        void Start()
        {
            // testing unit creation
            IUnit testUnit = Instantiate(BaseUnit).GetComponent<IUnit>();
            testUnit.Init(GameContext.Map.SectorMaterials, GameContext.Players[0], College.James);
            GameContext.Map.Grid[new Map.Hex.Coord(20, -23, 3)].OccupyingUnit = testUnit;

            testUnit = Instantiate(BaseUnit).GetComponent<IUnit>();
            testUnit.Init(GameContext.Map.SectorMaterials, GameContext.Players[1], College.Goodricke);
            GameContext.Map.Grid[new Map.Hex.Coord(39,-8,-31)].OccupyingUnit = testUnit;

            // start menu
            GameContext.Gui.CurrentMenu = MenuType.MovePhase;
            //GameContext.Gui.CurrentMenu = MenuType.AttackPhase;
        }

        #endregion
    }
}
