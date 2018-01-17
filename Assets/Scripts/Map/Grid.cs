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

        /// <summary>
        /// Path finding algorithm between two coordinates.
        /// Used https://www.redblobgames.com/pathfinding/a-star/introduction.html as a main resource
        /// Only the early-exit flood-fill and Breath-first search are implemented
        /// </summary>
        /// <param name="start">Coord from which to start the path finding</param>
        /// <param name="finish">Coordinate we are looking how to get to</param>
        /// <returns>Queue of Coord values representing the path from start to finish</returns>
        public Queue<Coord> PathFind(Coord start, Coord finish)
        {
            Coord current;
            Queue<Coord> path = new Queue<Coord>();
            Queue<Coord> frontier = new Queue<Coord>();
            frontier.Enqueue(start);
            Dictionary<Coord, Coord> cameFrom = new Dictionary<Coord, Coord>();
            cameFrom[start] = start;

            while (frontier.Count > 0)
            {
                current = frontier.Dequeue();
                if (current.Equals(finish))
                {
                    break;
                }

                foreach (Direction direct in Enum.GetValues(typeof(Direction)))
                {
                    Coord next = current.Neighbor(direct);
                    if (!cameFrom.ContainsKey(next) && _gridStore[next].Traversable)
                    {
                        frontier.Enqueue(next);
                        cameFrom[next] = current;
                    }
                }
            }

            current = finish;

            while (current != start)
            {
                path.Enqueue(current);
                current = cameFrom[current];
            }

            return path;
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
