using Gui;
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
            GameContext.Players.Add(new HumanPlayer(0));
            GameContext.Players.Add(new HumanPlayer(1));
        }

        void Start()
        {
            
        }

        void Update()
        {

        }

        #endregion
    }
}
