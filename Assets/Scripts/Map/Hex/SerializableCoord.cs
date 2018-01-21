using System;

namespace Map.Hex
{
    /// <summary>
    /// A serializable version of <see cref="Coord"/> that we can use for map loading.
    /// </summary>
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
        /// Converts the <see cref="SerializableCoord"/> to a
        /// <see cref="Coord"/> via an explict cast.
        /// </summary>
        /// <returns>The standard coordinate.</returns>
        /// <param name="coord">The serializable doordinate.</param>
        public static explicit operator Coord(SerializableCoord coord)
        {
            return new Coord(coord.q, coord.r);
        }
    }
}
