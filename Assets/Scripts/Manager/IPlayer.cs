using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public interface IPlayer
    {
        int Id { get; }
        int Mana { get; set; }
        int MaxMana { get; set; }
        bool ShouldSkipMovePhase { get; set; }
        bool ShouldSkipAttackPhase { get; set; }
        bool Equals(IPlayer other);
    }
}
