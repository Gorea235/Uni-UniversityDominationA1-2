using System;
using System.Collections;
using System.Collections.Generic;
using Map.Hex;
using UnityEngine;

namespace Map
{
    public class Grid : MonoBehaviour
    {
        Dictionary<Coord, Sector> _gridStore = new Dictionary<Coord, Sector>();

        public Sector this[Coord coord]
        {
            get
            {
               return _gridStore[coord];
            }
            set
            {
                _gridStore[coord] = value;
            }
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
