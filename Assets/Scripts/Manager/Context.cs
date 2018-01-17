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
        #region Public Properties

        public List<IPlayer> Players { get; }
        public GuiManager Gui { get; }
        public MapManager Map { get; }
        public AudioManager Audio { get; }

        #endregion

        #region Consuctor

        public Context(List<IPlayer> players, GuiManager gui, MapManager map, AudioManager audio)
        {
            Players = players;
            Gui = gui;
            Map = map;
            Audio = audio;
        }

        #endregion
    }
}
