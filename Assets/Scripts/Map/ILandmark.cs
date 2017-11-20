using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILandmark {

    void StartBonus(IPlayer player, Coord coord, Context context);
    void TurnBonus(IPlayer player, Coord coord, Context context);
}