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

        // unit bindings
        public GameObject AttackUnit;
        public GameObject BaseUnit;
        public GameObject DefenceUnit;
        public GameObject ScoutUnit;

        #endregion

        #region Private Fields

        Grid _grid;
        SectorMaterials _sectorMaterials;
        GameObject[] _allUnits;

        #endregion

        #region Public Properties

        public Grid Grid { get { return _grid; } }
        public SectorMaterials SectorMaterials { get { return _sectorMaterials; } }
        public GameObject[] AllUnits { get { return _allUnits; } }

        #endregion

        #region MonoBehaviour

        void Awake()
        {
            _sectorMaterials = gameObject.GetComponent<SectorMaterials>();

            // init unit list
            // i assume that we will have access to them by now
            _allUnits = new[] { AttackUnit, BaseUnit, DefenceUnit, ScoutUnit };
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
