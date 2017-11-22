using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Hex
{
    /// <summary>
    /// Convertion methods between the Coord grid system and Unity
    /// coordinates.
    /// Since we are using 3D, all calculations are done using Vector3
    /// with a default Z value. Vector3 can be implicitly converted to
    /// Vector2, in case we need to use them.
    /// </summary>
    public struct Layout
    {
        #region Default Layout w/ Lazy Init

        static Layout? _default;
        /// <summary>
        /// Gets the default layout.
        /// </summary>
        /// <value>The default layout.</value>
        public static Layout Default
        {
            get
            {
                if (_default == null)
                    _default = new Layout(Pointy, DefaultSize, DefaultOrigin);
                return (Layout)_default;
            }
        }

        #endregion

        #region Pre-Calculated Values

        static float DefaultZ { get; } = 0;
        static Vector3 DefaultSize { get; } = new Vector3(10, 10, DefaultZ); // adjust this to how big the grid should be
        static Vector3 DefaultOrigin { get; } = new Vector3(0, 0, DefaultZ);
        static Orientation Pointy { get; } = new Orientation(Math.Sqrt(3.0),
                                                             Math.Sqrt(3.0) / 2.0,
                                                             0.0,
                                                             3.0 / 2.0,
                                                             Math.Sqrt(3.0) / 3.0,
                                                             -1.0 / 3.0,
                                                             0.0,
                                                             2.0 / 3.0,
                                                             0.5);
        //static public Orientation flat = new Orientation(3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0), 2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0, 0.0);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation { get; }
        /// <summary>
        /// Gets the size of each Sector.
        /// </summary>
        /// <value>The size of a single Sector.</value>
        public Vector3 Size { get; }
        /// <summary>
        /// Gets the origin of the grid.
        /// </summary>
        /// <value>The origin of the grid.</value>
        public Vector3 Origin { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Map.Hex.Layout"/> struct.
        /// </summary>
        /// <param name="orientation">Orientation.</param>
        /// <param name="size">Size.</param>
        /// <param name="origin">Origin.</param>
        Layout(Orientation orientation, Vector3 size, Vector3 origin)
        {
            Orientation = orientation;
            Size = size;
            Origin = origin;
        }

        #endregion

        #region Hex Algorithms

        /// <summary>
        /// Converts a Coord to Unity coordinates.
        /// </summary>
        /// <returns>The Vector in Unity coordinates.</returns>
        /// <param name="h">The Coord.</param>
        public Vector3 HexToPixel(Coord h)
        {
            Orientation M = Orientation;
            double x = (M.F0 * h.Q + M.F1 * h.R) * Size.x;
            double y = (M.F2 * h.Q + M.F3 * h.R) * Size.y;
            return new Vector3((float)x + Origin.x, (float)y + Origin.y, DefaultZ);
        }

        /// <summary>
        /// Converts a Unity coordinate to a Coord
        /// </summary>
        /// <returns>The Unity coordinate.</returns>
        /// <param name="p">The coordinate.</param>
        public CoordDouble PixelToHex(Vector3 p)
        {
            Orientation M = Orientation;
            Vector3 pt = new Vector3((p.x - Origin.x) / Size.x, (p.y - Origin.y) / Size.y, DefaultZ);
            double q = M.B0 * pt.x + M.B1 * pt.y;
            double r = M.B2 * pt.x + M.B3 * pt.y;
            return new CoordDouble(q, r);
        }

        public Vector3 HexCornerOffset(int corner)
        {
            Orientation M = Orientation;
            double angle = 2.0 * Math.PI * (M.StartAngle - corner) / 6;
            return new Vector3(Size.x * (float)Math.Cos(angle), Size.y * (float)Math.Sin(angle), DefaultZ);
        }

        public List<Vector3> PolygonCorners(Coord h)
        {
            List<Vector3> corners = new List<Vector3> { };
            Vector3 center = HexToPixel(h);
            for (int i = 0; i < 6; i++)
            {
                Vector3 offset = HexCornerOffset(i);
                corners.Add(new Vector3(center.x + offset.x, center.y + offset.y, DefaultZ));
            }
            return corners;
        }

        #endregion
    }
}
