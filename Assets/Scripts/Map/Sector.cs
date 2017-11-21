using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Sector : MonoBehaviour
    {
        private Hex.Coord coordinates;
        private IUnit ocupUnit;
        private ILandmark landmark;

        //public IUnit OccupyingUnit { get; set; }
        //public IUandmark Landmark { get; }

            //constructor for a Sector

        public Sector(Hex.Coord initial_coord, ILandmark landmark) {
            this.coordinates = initial_coord;
            this.ocupUnit = null;
            this.landmark = landmark;
        }

        //getters and setters
        public Hex.Coord GetCoord() => this.coordinates;
        public IUnit OccupyingUnit { get => this.ocupUnit; set => this.ocupUnit = value; }  //is this how you write properties?
        public ILandmark Landmark { get => this.landmark; }



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
