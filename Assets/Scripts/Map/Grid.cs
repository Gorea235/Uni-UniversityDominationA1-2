using System;
using System.Collections;
using System.Collections.Generic;
using Map.Hex;
using UnityEngine;
using System.IO;

namespace Map
{
    public class Grid
    {
        #region Private Fields

        Dictionary<Coord, Sector> _gridStore = new Dictionary<Coord, Sector>();

        #endregion

        #region Public Properties

        public Sector this[Coord coord]
        {
            get
            {
                return _gridStore[coord];
            }
        }

        #endregion

        #region Constructor

        public Grid(GameObject parent, GameObject sectorPrefab, SectorMaterials sectorMaterials, string mapData)
        {
            Debug.Log("Initialising grid");
            float startTime = Time.realtimeSinceStartup;
            MapData preProcessedMap = JsonUtility.FromJson<MapData>(mapData);
            GameObject tmpSectorObj;
            Sector tmpSector;
            Coord tmpCoord;
            foreach (var gridItem in preProcessedMap.sectors)
            {
                tmpCoord = (Coord)gridItem.coordinate;
                tmpSectorObj = UnityEngine.Object.Instantiate(sectorPrefab, parent.transform);
                tmpSector = tmpSectorObj.GetComponent<Sector>();
                tmpSector.Init(sectorMaterials,
                               tmpCoord,
                               (SectorTexture)Enum.Parse(typeof(SectorTexture), gridItem.texture),
                               gridItem.traversable);
                _gridStore.Add(tmpCoord, tmpSector);
            }
            // todo: landmark processing
            float elapsedTime = Time.realtimeSinceStartup - startTime;
            Debug.Log(string.Format("Grid initialised in {0} seconds", elapsedTime));
        }

        #endregion
    }
}
