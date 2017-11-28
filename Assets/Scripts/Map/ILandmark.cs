using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Map.Hex;
using UnityEngine;

namespace Map
{
    public interface ILandmark
    {
        void StartBonus(IPlayer player, Coord coord, Context context);
        void TurnBonus(IPlayer player, Coord coord, Context context);
    }
}
