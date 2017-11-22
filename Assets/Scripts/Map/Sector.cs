using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Sector : MonoBehaviour
    {
        Hex.Coord currentCoord;
        GameObject plot;

        public IUnit OccupyingUnit { get; set; }
        public ILandmark Landmark { get; set; }



        public void Init(Hex.Coord positionCoord, Hex.SectorTexture texture)
        {
            currentCoord = positionCoord;
            plot = (GameObject)Resources.Load("/Assets/Prefabs/<name-of-prefab>", typeof(GameObject)); //cast predefined prefab of textureless hex shape as a GameObject
           // plot.GetComponent<Renderer>().material.SetTexture("<name-of-texture>", texture);           //render a material mesh from a predefined texture set. TODO: finish the SectorTexture enum to implement this
        }

        public GameObject GetPlot { get { return plot; } }
        public Hex.Coord GetCoord { get { return currentCoord; } }



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
