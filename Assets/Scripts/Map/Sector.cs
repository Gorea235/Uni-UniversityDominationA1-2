using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Sector : MonoBehaviour
    {
        #region Unity Bindings

        public Texture testTexture;

        #endregion

        #region Private fields

        Hex.Coord _currentCoord;

        #endregion

        #region Public Properties

        public IUnit OccupyingUnit { get; set; }
        public ILandmark Landmark { get; set; }

        #endregion

        #region Initialisation

        public void Init(Hex.Coord currentCoord, SectorTexture texture)
        {
            _currentCoord = currentCoord;

            // Set texture
            switch (texture)
            {
                case SectorTexture.TestTexture:
                    gameObject.GetComponent<Renderer>().material.mainTexture = testTexture;
                    break;
            }
        }

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion
    }
}
