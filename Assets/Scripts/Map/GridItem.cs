using System;
using Map;
using Map.Hex;

namespace Map
{
    [Serializable]
    public class GridItem
    {
        public SerializableCoord coordinate;
        public string texture;
    }
}
