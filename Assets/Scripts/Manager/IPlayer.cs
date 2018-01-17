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
        bool ShouldSkipMovePhase { get; set; }
        bool ShouldSkipAttackPhase { get; set; }
    }
}
