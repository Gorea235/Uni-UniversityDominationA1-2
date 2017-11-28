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

        public Grid(GameObject parent, GameObject sectorPrefab, string mapData)
        {
            MapData preProcessedMap = JsonUtility.FromJson<MapData>(mapData);
            GameObject tmpSectorObj;
            Sector tmpSector;
            Coord tmpCoord;
            foreach (var gridItem in preProcessedMap.sectors)
            {
                tmpCoord = (Coord)gridItem.coordinate;
                tmpSectorObj = UnityEngine.Object.Instantiate(sectorPrefab, parent.transform);
                tmpSector = tmpSectorObj.GetComponent<Sector>();
                tmpSector.Init(tmpCoord, (SectorTexture)Enum.Parse(typeof(SectorTexture), gridItem.texture));
                _gridStore.Add(tmpCoord, tmpSector);
            }
            // todo: landmark processing
        }

        #endregion
    }
}
