using Gui;
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
        }

        void Start()
        {
            // player initialisation is done here
            IPlayer playerOne = new HumanPlayer(1);
            IPlayer playerTwo = new HumanPlayer(2);

            List<IPlayer> currentPlayers = new List<IPlayer>()
            {
                playerOne,
                playerTwo
            };
        }

        void Update()
        {

        }

        #endregion
    }
}
