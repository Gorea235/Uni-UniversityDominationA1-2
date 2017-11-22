using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Sector : MonoBehaviour
    {
        readonly Hex.Coord _currentCoord;

        public IUnit OccupyingUnit { get; set; }
        public ILandmark Landmark { get; set; }

        public Sector(Hex.Coord currentCoord) {
            _currentCoord = currentCoord;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
