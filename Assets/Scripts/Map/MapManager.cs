using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        #region Unity Bindings

        public TextAsset mapData;
        public GameObject sectorPrefab;

        #endregion

        #region Private Fields

        Grid _grid;
        SectorMaterials _sectorMaterials;
        GameObject[] _allUnits;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current map grid.
        /// </summary>
        public Grid Grid { get { return _grid; } }
        /// <summary>
        /// The current SectorMaterials object.
        /// </summary>
        public SectorMaterials SectorMaterials { get { return _sectorMaterials; } }

        #endregion

        #region MonoBehaviour

        void Awake()
        {
            // init the sector materials
            _sectorMaterials = gameObject.GetComponent<SectorMaterials>();
        }

        void Start()
        {
            // now init the grid
            // has to be done here to give sector materials time to set up
            _grid = new Grid(gameObject, sectorPrefab, _sectorMaterials, mapData.text);
        }

        void Update()
        {

        }

        #endregion
    }
}
