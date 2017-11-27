using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Map.Hex;

[System.Serializable]
public class SCoord  {
    public Dictionary<Coord, Sector> gridStore;
    public Coord coordinate;
    public Sector plot;
}
