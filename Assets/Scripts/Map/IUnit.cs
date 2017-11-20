using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit {
    int Health { get; set; }
    int Attack { get; }
    int MaxMove { get; }
    int Defence { get; }
    IPlayer Owner { get; }
    College.College College { get; }  //I think the descriptive properties of this particular line are nonexistent
}
