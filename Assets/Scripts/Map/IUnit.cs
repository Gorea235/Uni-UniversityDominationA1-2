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
        IPlayer Owner { get; }
        College College { get; }
    }
}
