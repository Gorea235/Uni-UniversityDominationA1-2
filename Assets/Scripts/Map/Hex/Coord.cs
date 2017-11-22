using System;
using System.Collections;
using System.Collections.Generic;

namespace Map.Hex
{
    /*
     * Some of the methods have commented out versions which use Cube
     * coordinates rather than axial. The reason for this is so, if
     * we find problems with this struct, we can switch the comments
     * and use the constructor argument checking to ensure correctness,
     * but for normal use we only Q and R since it is cleaner and maybe
     * slightly faster (although it probably doesn't matter too much).
     * 
     * This and the other structs in Map.Hex are taken from
     * https://www.redblobgames.com/grids/hexagons/
     */
    public struct Coord
    {
        #region Properties

        static Dictionary<Direction, Coord> _directions = new Dictionary<Direction, Coord>
        {
            { Direction.NorthEast, new Coord(1, 0, -1) },
            { Direction.East, new Coord(1, -1, 0) },
            { Direction.SouthEast, new Coord(0, -1, 1) },
            { Direction.SouthWest, new Coord(-1, 0, 1) },
            { Direction.West, new Coord(-1, 1, 0) },
            { Direction.NorthWest, new Coord(0, 1, -1) }
        };
        /// <summary>
        /// Direction lookup table.
        /// </summary>
        /// <value>The directions.</value>
        public static Dictionary<Direction, Coord> Directions
        {
            get { return _directions; }
        }

        static Dictionary<DiagonalDirection, Coord> _diagonalDirections = new Dictionary<DiagonalDirection, Coord>
        {
            { DiagonalDirection.North, new Coord(1, 1, -2) },
            { DiagonalDirection.NorthEast, new Coord(2, -1, -1) },
            { DiagonalDirection.SouthEast, new Coord(1, -2, 1) },
            { DiagonalDirection.South, new Coord(-1, -1, 2) },
            { DiagonalDirection.SouthWest, new Coord(-2, 1, 1) },
            { DiagonalDirection.NorthWest, new Coord(-1, 2, -1) }
        };
        /// <summary>
        /// Diagonal direction lookup table.
        /// </summary>
        /// <value>The directions.</value>
        public static Dictionary<DiagonalDirection, Coord> DiagonalDirections
        {
            get { return _diagonalDirections; }
        }

        /// <summary>
        /// Q in cube/axial.
        /// </summary>
        /// <value>Q</value>
        public int Q { get; }
        /// <summary>
        /// R in cube/axial.
        /// </summary>
        /// <value>R</value>
        public int R { get; }
        /// <summary>
        /// S in cube. If Coord is created with just Q and R,
        /// then this value is pre-calculated.
        /// </summary>
        /// <value>S</value>
        public int S { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a Coord using Axial coordinates.
        /// </summary>
        /// <param name="q">Q</param>
        /// <param name="r">R</param>
        public Coord(int q, int r)
        {
            Q = q;
            R = r;
            S = -q + -r;
        }

        /// <summary>
        /// Initialises a Coord using Cube coordinates.
        /// </summary>
        /// <param name="q">Q</param>
        /// <param name="r">R</param>
        /// <param name="s">S</param>
        public Coord(int q, int r, int s)
        {
            if (q + r + s != 0)
                throw new ArgumentException("Q + R + S must equal 0");
            Q = q;
            R = r;
            S = s;
        }

        #endregion

        #region Hex Algorithms

        /// <summary>
        /// Rotates the Coord anti-clockwise about the origin.
        /// </summary>
        /// <returns>The rotated Coord.</returns>
        public Coord RotateLeft()
        {
            //return new Coord(-S, -Q, -R);
            return new Coord(-S, -Q);
        }

        /// <summary>
        /// Rotates the Coord clockwise about the origin.
        /// </summary>
        /// <returns>The rotated Coord.</returns>
        public Coord RotateRight()
        {
            //return new Coord(-R, -S, -Q);
            return new Coord(-R, -S);
        }

        /// <summary>
        /// Gets the neighbor coord.
        /// </summary>
        /// <returns>The neighbor.</returns>
        /// <param name="direction">Direction to get.</param>
        public Coord Neighbor(Direction direction)
        {
            return this + Directions[direction];
        }

        /// <summary>
        /// Gets the diagonal coord.
        /// </summary>
        /// <returns>The neighbor.</returns>
        /// <param name="direction">Direction to get.</param>
        public Coord Diagonal(DiagonalDirection direction)
        {
            return this + DiagonalDirections[direction];
        }

        /// <summary>
        /// Length of this Coord.
        /// </summary>
        /// <returns>The length.</returns>
        public int Length()
        {
            return (Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2;
        }

        /// <summary>
        /// Finds the distance to b.
        /// </summary>
        /// <returns>The distance to b.</returns>
        /// <param name="b">The Coord to get the distance to.</param>
        public int DistanceTo(Coord b)
        {
            return (this - b).Length();
        }

        /// <summary>
        /// Returns the list of Coords between this and the given Coord
        /// (inclusive (I assume)).
        /// </summary>
        /// <returns>The list of Coords</returns>
        /// <param name="b">The Coord to draw to.</param>
        public List<Coord> Linedraw(Coord b)
        {
            int N = DistanceTo(b);
            CoordDouble a_nudge = new CoordDouble(Q + 0.000001, R + 0.000001, S - 0.000002);
            CoordDouble b_nudge = new CoordDouble(b.Q + 0.000001, b.R + 0.000001, b.S - 0.000002);
            List<Coord> results = new List<Coord> { };
            double step = 1.0 / Math.Max(N, 1);
            for (int i = 0; i <= N; i++)
                results.Add(a_nudge.Lerp(b_nudge, step * i).Round());
            return results;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds a <see cref="T:Map.Hex.Coord"/> to a <see cref="T:Map.Hex.Coord"/>,
        /// yielding a new <see cref="T:Map.Hex.Coord"/>.
        /// </summary>
        /// <param name="a">The first <see cref="T:Map.Hex.Coord"/> to add.</param>
        /// <param name="b">The second <see cref="T:Map.Hex.Coord"/> to add.</param>
        /// <returns>The <see cref="T:Map.Hex.Coord"/> that is the sum of the values of <c>a</c> and <c>b</c>.</returns>
        public static Coord operator +(Coord a, Coord b)
        {
            //return new Coord(a.Q + b.Q, a.R + b.R, a.S + b.S);
            return new Coord(a.Q + b.Q, a.R + b.R);
        }

        /// <summary>
        /// Subtracts a <see cref="T:Map.Hex.Coord"/> from a <see cref="T:Map.Hex.Coord"/>,
        /// yielding a new <see cref="T:Map.Hex.Coord"/>.
        /// </summary>
        /// <param name="a">The <see cref="T:Map.Hex.Coord"/> to subtract from (the minuend).</param>
        /// <param name="b">The <see cref="T:Map.Hex.Coord"/> to subtract (the subtrahend).</param>
        /// <returns>The <see cref="T:Map.Hex.Coord"/> that is the <c>a</c> minus <c>b</c>.</returns>
        public static Coord operator -(Coord a, Coord b)
        {
            //return new Coord(a.Q - b.Q, a.R - b.R, a.S - b.S);
            return new Coord(a.Q - b.Q, a.R - b.R);
        }

        /// <summary>
        /// Computes the product of <c>a</c> and <c>k</c>, yielding a new <see cref="T:Map.Hex.Coord"/>.
        /// </summary>
        /// <param name="a">The <see cref="T:Map.Hex.Coord"/> to multiply.</param>
        /// <param name="k">The <see cref="int"/> to multiply.</param>
        /// <returns>The <see cref="T:Map.Hex.Coord"/> that is the <c>a</c> * <c>k</c>.</returns>
        static public Coord operator *(Coord a, int k)
        {
            //return new Coord(a.Q * k, a.R * k, a.S * k);
            return new Coord(a.Q * k, a.R * k);
        }

        /// <summary>
        /// Determines whether a specified instance of <see cref="T:Map.Hex.Coord"/>
        /// is equal to another specified <see cref="T:Map.Hex.Coord"/>.
        /// </summary>
        /// <param name="a">The first <see cref="T:Map.Hex.Coord"/> to compare.</param>
        /// <param name="b">The second <see cref="T:Map.Hex.Coord"/> to compare.</param>
        /// <returns><c>true</c> if <c>a</c> and <c>b</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Coord a, Coord b)
        {
            //return a.Q == b.Q && a.R == b.R && a.S == b.S;
            return a.Q == b.Q && a.R == b.R;
        }

        /// <summary>
        /// Determines whether a specified instance of <see cref="T:Map.Hex.Coord"/>
        /// is not equal to another specified <see cref="T:Map.Hex.Coord"/>.
        /// </summary>
        /// <param name="a">The first <see cref="T:Map.Hex.Coord"/> to compare.</param>
        /// <param name="b">The second <see cref="T:Map.Hex.Coord"/> to compare.</param>
        /// <returns><c>true</c> if <c>a</c> and <c>b</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Coord a, Coord b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to
        /// the current <see cref="T:Map.Hex.Coord"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:Map.Hex.Coord"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="T:Map.Hex.Coord"/>;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            Coord? b = obj as Coord?;
            if (b == null)
                return false;
            return this == b;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:Map.Hex.Coord"/> object.
        /// </summary>
        /// <returns>
        /// A hash code for this instance that is suitable for use in hashing
        /// algorithms and data structures such as a hash table.
        /// </returns>
        /// <remarks>
        /// I took the suggestion in
        /// https://stackoverflow.com/questions/2733541/what-is-the-best-way-to-implement-this-composite-gethashcode
        /// and used some larger randomly chosen primes from
        /// http://www.primos.mat.br/indexen.html
        /// in order to implement this.
        /// The reason for the concrete implementation, rather than using
        /// the base version, is that it ensures properly distributed hashes,
        /// while also ensuring 2 instantiations of <see cref="T:Map.Hex.Coord"/>
        /// will always produce the same hash, since they should be identical.
        /// </remarks>
        public override int GetHashCode()
        {
            int hash = 887;
            hash = hash * 101 + Q.GetHashCode();
            hash = hash * 103 + R.GetHashCode();
            // ignore S as it is always implied
            return hash;
            //return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Map.Hex.Coord"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Map.Hex.Coord"/>.</returns>
        public override string ToString()
        {
            return string.Format("Coord({0}, {1}, {2})", Q, R, S);
        }

        #endregion
    }
}
