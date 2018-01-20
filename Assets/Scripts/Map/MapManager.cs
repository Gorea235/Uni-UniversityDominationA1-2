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

        public Grid Grid { get { return _grid; } }
        public SectorMaterials SectorMaterials { get { return _sectorMaterials; } }

        #endregion

        #region MonoBehaviour

        void Awake()
        {
            _sectorMaterials = gameObject.GetComponent<SectorMaterials>();
        }

        void Start()
        {
            _grid = new Grid(gameObject, sectorPrefab, _sectorMaterials, mapData.text);
        }

        void Update()
        {

        }

        #endregion
    }
}
