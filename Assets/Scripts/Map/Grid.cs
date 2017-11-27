using System;
using System.Collections;
using System.Collections.Generic;
using Map.Hex;
using UnityEngine;
using System.IO;

namespace Map
{
    public class Grid : MonoBehaviour
    {
        Dictionary<Coord, Sector> _gridStore = new Dictionary<Coord, Sector>();
        string mapDataFileName; // TODO: Map .json to here

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

        private void LoadMapData() {
            string filePath = Path.Combine("somepathtomapdata", mapDataFileName);
            if (File.Exists(filePath))
            {
                string mapAsJson = File.ReadAllText(filePath);
                SCoord loadedMap = JsonUtility.FromJson<SCoord>(mapAsJson);
            }
            else
            {
                throw new System.Runtime.Serialization.SerializationException();
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
