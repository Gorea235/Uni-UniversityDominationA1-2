using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public interface IUnit
    {
        int Health { get; set; }
        int Attack { get; }
        int MaxMove { get; }
        int Defence { get; }
        Manager.IPlayer Owner { get; }
        College College { get; }
        Vector3 Position { get; set; }
        void Init(Manager.IPlayer player, College college, Vector3 position);
        void ApplyEffect(IEffect effect);
        void ProcessEffects(); // called on all units of a player on turn start
    }
}
