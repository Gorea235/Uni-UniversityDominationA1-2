using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public interface IPlayer
    {
        int Id { get; }
        float Mana { get; set; }
        float MaxMana { get; set; }
        bool ShouldSkipMovePhase { get; set; }
        bool ShouldSkipAttackPhase { get; set; }
        bool Equals(IPlayer other);
    }
}
