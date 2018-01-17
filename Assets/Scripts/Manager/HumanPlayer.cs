using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class HumanPlayer : MonoBehaviour, IPlayer {

    int _id;
    int _mana;
    bool _skipMove;
    bool _skipAttack;


    public int Id
    {
        get
        {
            return _id;
        }
    }

    //Making sure the mana doesn't exceed a certain value
    //for now, mana caps at 10
    public int Mana {
        get { return _mana; }
        set {
            if (_mana + value > 10)
            {
                _mana = 10;
            }
            else if (_mana - value < 0)
            {
                _mana = 0;
            }
            else
            {
                _mana = value;
            }
            }
    }
    public bool ShouldSkipMovePhase { get { return _skipMove; } set { _skipMove = value; } }
    public bool ShouldSkipAttackPhase { get { return _skipAttack; } set { _skipAttack = value; } }

    public HumanPlayer(int id)
    {
        //starting ammount of mana for all players
        _mana = 3;
        _id = id;
        ShouldSkipAttackPhase = false;
        ShouldSkipMovePhase = false;
    }
}
