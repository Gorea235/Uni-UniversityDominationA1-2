using System;

namespace Map.Hex
{
    [Serializable]
    public struct SerializableCoord
    {
        public int q;
        public int r;

        public SerializableCoord(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        /// <summary>
        /// Converts the <see cref="T:Map.Hex.SerializableCoord"/> to a
        /// <see cref="T:Map.Hex.Coord"/> via an explict cast.
        /// </summary>
        /// <returns>The standard coordinate.</returns>
        /// <param name="coord">The serializable doordinate.</param>
        public static explicit operator Coord(SerializableCoord coord)
        {
            return new Coord(coord.q, coord.r);
        }
    }
}
