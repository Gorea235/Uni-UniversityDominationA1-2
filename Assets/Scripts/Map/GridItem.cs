using System;
using Map;
using Map.Hex;

namespace Map
{
    /// <summary>
    /// A serialzable version of each grid item. Used for map loading.
    /// </summary>
    [Serializable]
    public class GridItem
    {
        public SerializableCoord coordinate;
        public string texture;
        public bool traversable;
    }
}
