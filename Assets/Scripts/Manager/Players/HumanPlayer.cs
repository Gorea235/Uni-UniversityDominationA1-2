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
        int _mana; // set on player creation
        int _maxMana; // set on player creation
        bool _skipMove = false;
        bool _skipAttack = false;

        #endregion

        #region Public Properties

        // all properties here are implementations for IPlayer

        public int Id { get { return _id; } }
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

        /// <summary>
        /// Constructions an new <c>HumanPlayer</c> with the given ID.
        /// </summary>
        /// <param name="id"></param>
        public HumanPlayer(int id)
        {
            _id = id;
        }

        #endregion

        #region Operators

        /// <summary>
        /// See <see cref="IPlayer.Equals(IPlayer)"/>.
        /// </summary>
        public bool Equals(IPlayer other)
        {
            return Id == other.Id;
        }

        #endregion
    }
}
