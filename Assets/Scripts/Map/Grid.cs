﻿using System;
using System.Collections;
using System.Collections.Generic;
using Map.Hex;
using UnityEngine;
using System.IO;

namespace Map
{
    public class Grid : IEnumerable<KeyValuePair<Coord, Sector>>
    {
        #region Private Fields

        Dictionary<Coord, Sector> _gridStore = new Dictionary<Coord, Sector>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="Sector"/> at the given <see cref="Coord"/>.
        /// </summary>
        /// <param name="coord">The coordinate of the sector to get.</param>
        /// <returns>The sector at the given coordinate.</returns>
        public Sector this[Coord coord]
        {
            get
            {
                return _gridStore[coord];
            }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Checks whether a sector is traversable in a null-safe way.
        /// </summary>
        /// <param name="coord">The position of the sector to check.</param>
        /// <returns>Whether the given coordinate is traversable.</returns>
        public bool IsTraversable(Coord coord) => _gridStore.ContainsKey(coord) && _gridStore[coord].Traversable;

        /// <summary>
        /// Path finding algorithm between two coordinates.
        /// Used https://www.redblobgames.com/pathfinding/a-star/introduction.html as a main resource
        /// Only the early-exit flood-fill and Breath-first search are implemented
        /// </summary>
        /// <param name="start">Coord from which to start the path finding</param>
        /// <param name="finish">Coordinate we are looking how to get to</param>
        /// <returns>Stack of Coord values representing the path from start to finish</returns>
        public Stack<Coord> PathFind(Coord start, Coord finish)
        {
            Coord current;
            Stack<Coord> path = new Stack<Coord>();
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
                    if (!cameFrom.ContainsKey(next) && IsTraversable(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom[next] = current;
                    }
                }
            }

            current = finish;

            while (current != start)
            {
                path.Push(current);
                current = cameFrom[current];
            }

            return path;
        }

        public HashSet<Coord> GetRange(Coord start, int range)
        {
            HashSet<Coord> visited = new HashSet<Coord> { start };
            List<List<Coord>> fringes = new List<List<Coord>> { new List<Coord>() { start } };

            for (int i = 1; i <= range; i++)
            {
                fringes.Add(new List<Coord>());
                foreach (Coord plot in fringes[i - 1])
                {
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        Coord neighbor = plot.Neighbor(direction);
                        if (!visited.Contains(neighbor) && _gridStore.ContainsKey(neighbor) && IsTraversable(neighbor))
                        {
                            visited.Add(neighbor);
                            fringes[i].Add(neighbor);
                        }
                    }
                }
            }

            return visited;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialises the full grid using the given map data.
        /// </summary>
        /// <param name="parent">The parent object of the sectors.</param>
        /// <param name="sectorPrefab">The sector prefab.</param>
        /// <param name="sectorMaterials">The sector materials object.</param>
        /// <param name="mapData">The map data to use to load.</param>
        public Grid(GameObject parent, GameObject sectorPrefab, SectorMaterials sectorMaterials, string mapData)
        {
            Debug.Log("Initialising grid");
            float startTime = Time.realtimeSinceStartup;
            MapData preProcessedMap = JsonUtility.FromJson<MapData>(mapData); // parse map data into object
            GameObject tmpSectorObj;
            Sector tmpSector;
            Coord tmpCoord;
            foreach (var gridItem in preProcessedMap.sectors)
            {
                tmpCoord = (Coord)gridItem.coordinate; // convert coordinate to Coord
                tmpSectorObj = UnityEngine.Object.Instantiate(sectorPrefab, parent.transform); // instantiate the new sector prefab
                tmpSector = tmpSectorObj.GetComponent<Sector>(); // grab the Sector class on the object
                // initialise the sector using the necessary data
                tmpSector.Init(sectorMaterials,
                               tmpCoord,
                               (SectorTexture)Enum.Parse(typeof(SectorTexture), gridItem.texture), // hand in the texture the sector should have
                               gridItem.traversable); // hand in whether the sector is traversable or not
                _gridStore.Add(tmpCoord, tmpSector); // add the sector to the grid storage
            }
            // todo: landmark processing
            float elapsedTime = Time.realtimeSinceStartup - startTime;
            Debug.Log(string.Format("Grid initialised in {0} seconds", elapsedTime));
        }

        #endregion

        #region Enumerator

        // enumerator implementation to allow iterating through sectors

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_gridStore).GetEnumerator();
        }

        IEnumerator<KeyValuePair<Coord, Sector>> IEnumerable<KeyValuePair<Coord, Sector>>.GetEnumerator()
        {
            return _gridStore.GetEnumerator();
        }

        #endregion
    }
}
