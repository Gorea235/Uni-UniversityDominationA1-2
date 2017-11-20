using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILandmark {

    void StartBonus(IPlayer player, Coord coord, Context context);
    void TurnBonus(IPlayer player, Coord coord, Context context);
}

//Added class Coord here just so it doesn't complain as current project setting doesnt have a placeholder for coord 
public class Coord
{
}