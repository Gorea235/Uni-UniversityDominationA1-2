using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Manager.Players
{
    public class AiPlayer : IPlayer
    {

        #region Private Fields

        int _id;

        #endregion

        #region Public Properties

#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter

        public int Id { get { return _id; } }
        public int Mana
        {
            get { return 0; }
            set { }
        }
        public int MaxMana
        {
            get { return 0; }
            set { }
        }
        public bool ShouldSkipMovePhase
        {
            get { return true; }
            set { }
        }
        public bool ShouldSkipAttackPhase
        {
            get { return true; }
            set { }
        }

#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter

        #endregion

        #region Constructor

        public AiPlayer(int id)
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
