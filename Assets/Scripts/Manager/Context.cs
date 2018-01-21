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

        int _lastPlayerId = 0;

        #endregion

        #region Public Properties

        /// <summary>
        /// All active players in the game currently. Not all of them are to be processing
        /// in the normal turn phases.
        /// </summary>
        public Dictionary<int, IPlayer> Players { get; } = new Dictionary<int, IPlayer>();
        /// <summary>
        /// The order of the player turns.
        /// </summary>
        public Queue<int> PlayerOrder { get; } = new Queue<int>();
        /// <summary>
        /// The ID of the player whos turn it is.
        /// </summary>
        public int CurrentPlayerId { get { return PlayerOrder.Peek(); } }
        /// <summary>
        /// The <see cref="IPlayer"/> object of the player whos turn it is.
        /// </summary>
        public IPlayer CurrentPlayer { get { return Players[CurrentPlayerId]; } }
        /// <summary>
        /// The ID of the first player in the turn cycle.
        /// </summary>
        public int StartingPlayerId { get; set; }
        /// <summary>
        /// The GUI manager.
        /// </summary>
        public GuiManager Gui { get; }
        /// <summary>
        /// The map manager.
        /// </summary>
        public MapManager Map { get; }
        /// <summary>
        /// The audio manager.
        /// </summary>
        public AudioManager Audio { get; }
        /// <summary>
        /// All of the unit prefabs that we have available in game.
        /// </summary>
        public GameObject[] UnitPrefabs { get; }
        /// <summary>
        /// A dictionary of a default version of every unit with every college.
        /// </summary>
        public Dictionary<College, Dictionary<int, IUnit>> DefaultUnits { get; }

        #endregion

        #region Consuctor

        /// <summary>
        /// Constructs a new Context object.
        /// </summary>
        /// <param name="gui">The <see cref="GuiManager"/>.</param>
        /// <param name="map">The <see cref="MapManager"/>.</param>
        /// <param name="audio">The <see cref="AudioManager"/>.</param>
        /// <param name="unitPrefabs">The unit prefab <see cref="GameObject"/>s.</param>
        /// <param name="defaultUnits">The default unit dictionary.</param>
        public Context(GuiManager gui, MapManager map, AudioManager audio, GameObject[] unitPrefabs,
            Dictionary<College, Dictionary<int, IUnit>> defaultUnits)
        {
            Gui = gui;
            Map = map;
            Audio = audio;
            UnitPrefabs = unitPrefabs;
            DefaultUnits = defaultUnits;
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Generates a new unit ID to ensure that there are no conflicts.
        /// </summary>
        /// <returns>The newly generated ID.</returns>
        public int GetNewPlayerId() => _lastPlayerId++;

        /// <summary>
        /// Moves to the next player in the turn cycle.
        /// If we move back to the starting player, then all players mana is increased.
        /// </summary>
        public void NextPlayer()
        {
            PlayerOrder.Enqueue(PlayerOrder.Dequeue());
            if (CurrentPlayerId == StartingPlayerId)
                foreach (IPlayer player in Players.Values)
                    player.MaxMana += 5; // mana is increased each full cycle of players (this will need tweaking)
            CurrentPlayer.Mana = CurrentPlayer.MaxMana;
        }

        #endregion
    }
}
