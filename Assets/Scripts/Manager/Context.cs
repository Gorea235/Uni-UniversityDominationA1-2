using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Context
    {
        public List<IPlayer> Players { get; }
        public Gui.Gui Gui { get; }
        public Map.Map Map { get; }
        public AudioManager Audio { get; }

        public Context(List<IPlayer> players, Gui.Gui gui, Map.Map map, AudioManager audio)
        {
            Players = players;
            Gui = gui;
            Map = map;
            Audio = audio;
        }
    }
}
