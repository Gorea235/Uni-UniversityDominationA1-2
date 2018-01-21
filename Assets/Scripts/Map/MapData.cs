using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// The object that represents the map data as in the JSON format.
    /// </summary>
    public class MapData
    {
        /// <summary>
        /// The list of sectors defined in the save file.
        /// </summary>
        public List<GridItem> sectors;
    }
}
