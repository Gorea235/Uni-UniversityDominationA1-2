using Gui;
using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Context
    {
        #region Private Fields

        int _lastPlayerId;

        #endregion

        #region Public Properties

        public Dictionary<int, IPlayer> Players { get; }
        public int CurrentPlayerId { get; set; }
        public IPlayer CurrentPlayer { get { return Players[CurrentPlayerId]; } }
        public GuiManager Gui { get; }
        public MapManager Map { get; }
        public AudioManager Audio { get; }

        #endregion

        #region Consuctor

        public Context(Dictionary<int, IPlayer> players, GuiManager gui, MapManager map, AudioManager audio)
        {
            Players = players;
            Gui = gui;
            Map = map;
            Audio = audio;
        }

        #endregion

        #region Helper Functions

        public int GetNewPlayerId() => _lastPlayerId++;

        #endregion
    }
}
