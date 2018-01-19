using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Manager.Players
{
    public class HumanPlayer : IPlayer
    {

        #region Private Fields

        int _id;
        float _mana; // set on player creation
        float _maxMana; // set on player creation
        bool _skipMove = false;
        bool _skipAttack = false;

        #endregion

        #region Public Properties

        public int Id { get { return _id; } }
        public float Mana
        {
            get { return _mana; }
            set { _mana = value; }
        }
        public float MaxMana
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

        public HumanPlayer(int id)
        {
            _id = id;
        }

        #endregion

        #region Operators

        public bool Equals(IPlayer other)
        {
            return Id == other.Id;
        }

        #endregion
    }
}
