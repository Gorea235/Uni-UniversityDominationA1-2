using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Manager.Players
{
    public class HumanPlayer : MonoBehaviour, IPlayer
    {

        #region Private Fields

        uint _id;
        int _mana; // set on player creation
        int _maxMana; // set on player creation
        bool _skipMove = false;
        bool _skipAttack = false;

        #endregion

        #region Public Properties

        public uint Id { get { return _id; } }
        public int Mana
        {
            get { return _mana; }
            set { _mana = value; }
        }
        public int MaxMana
        {
            get { return _maxMana; }
            set { _maxMana = value; }
        }
        public bool ShouldSkipMovePhase
        {
            get { return _skipMove; }
            set { _skipMove = value; }
        }
        public bool ShouldSkipAttackPhase
        {
            get { return _skipAttack; }
            set { _skipAttack = value; }
        }

        #endregion

        #region Constructor

        public HumanPlayer(uint id)
        {
            _id = id;
        }

        #endregion
    }
}
