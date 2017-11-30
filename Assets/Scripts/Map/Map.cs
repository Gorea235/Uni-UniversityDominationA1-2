using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Map : MonoBehaviour
    {
        #region Unity Bindings

        public TextAsset mapData;
        public GameObject sectorPrefab;

        #endregion

        #region Private Fields

        Grid _grid;
        SectorMaterials _sectorMaterials;

        #endregion

        #region Public Properties

        public Grid Grid { get { return _grid; } }

        #endregion

        #region MonoBehaviour

        // Equivilent to constructor but for MonoBehaviour
        void Awake()
        {
            _sectorMaterials = gameObject.GetComponent<SectorMaterials>();
            _sectorMaterials.Init();
            _grid = new Grid(gameObject, sectorPrefab, _sectorMaterials, mapData.text);
        }

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
