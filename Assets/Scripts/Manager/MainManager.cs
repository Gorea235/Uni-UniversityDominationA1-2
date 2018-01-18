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
        #region Properties

        public Context GameContext { get; private set; }

        #endregion

        #region MonoBehaviour

        void Awake()
        {
            // pull in the game context
            GameContext = new Context(
                new List<IPlayer>(),
                GameObject.Find("Gui").GetComponent<GuiManager>(),
                GameObject.Find("Map").GetComponent<MapManager>(),
                gameObject.GetComponent<AudioManager>());

            // basic default players
            // for full game, the menu that deals with allowing users to set up the game
            // and available players in said game will add each player instance.
            GameContext.Players.Add(new HumanPlayer(0)
            {
                Mana = 10,
                MaxMana = 10
            });
            GameContext.Players.Add(new HumanPlayer(1)
            {
                Mana = 10,
                MaxMana = 10
            });
        }

        void Start()
        {
            // testing unit creation
            IUnit testUnit = Instantiate(GameContext.Map.BaseUnit).GetComponent<IUnit>();
            testUnit.Init(GameContext.Map.SectorMaterials, GameContext.Players[0], College.Halifax);
            GameContext.Map.Grid[new Map.Hex.Coord(1, 1)].OccupyingUnit = testUnit;
        }

        void Update()
        {

        }

        #endregion
    }
}