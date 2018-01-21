using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Hex
{
    public struct CoordDouble
    {
        #region Properties

        /// <summary>
        /// Q in cube/axial.
        /// </summary>
        /// <value>Q</value>
        public double Q { get; }
        /// <summary>
        /// R in cube/axial.
        /// </summary>
        /// <value>R</value>
        public double R { get; }
        /// <summary>
        /// S in cube/axial.
        /// </summary>
        /// <value>S</value>
        public double S { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a Coord using Axial coordinates.
        /// </summary>
        /// <param name="q">Q</param>
        /// <param name="r">R</param>
        public CoordDouble(double q, double r)
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
        public CoordDouble(double q, double r, double s)
        {
            //if (Math.Abs(q + r + s) > float.Epsilon) // allow for floating point errors
            //if (!Mathf.Approximately((float)(q + r + s), 0)) // allow for floating point errors
            if (Math.Abs(Math.Round(q + r + s, 14)) > double.Epsilon)
                throw new ArgumentException("Q + R + S must equal 0");
            Q = q;
            R = r;
            S = s;
        }

        #endregion

        #region Hex Algorithms

        /// <summary>
        /// Rounds the <see cref="T:Map.Hex.CoordDouble"/> into a <see cref="T:Map.Hex.Coord"/>.
        /// </summary>
        /// <returns>The rounded coordinate.</returns>
        public Coord Round()
        {
            int q = (int)(Math.Round(Q));
            int r = (int)(Math.Round(R));
            int s = (int)(Math.Round(S));
            double q_diff = Math.Abs(q - Q);
            double r_diff = Math.Abs(r - R);
            double s_diff = Math.Abs(s - S);
            if (q_diff > r_diff && q_diff > s_diff)
                q = -r - s;
            else if (r_diff > s_diff)
                r = -q - s;
            else
                s = -q - r;
            return new Coord(q, r, s);
        }

        /// <summary>
        /// Performs a linear interpolation between 2 values.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The percentage to interpolate.</param>
        /// <returns>The point that is <c>t%</c> between <c>a</c> and <c>b</c>.</returns>
        static double Lerp(double a, double b, double t) => a * (1 - t) + b * t;

        /// <summary>
        /// Performs an linear interpolation to the given <see cref="CoordDouble"/>.
        /// </summary>
        /// <param name="b">The coordinate to interpolate to.</param>
        /// <param name="t">The percentage to interpolate.</param>
        /// <returns>
        /// The <see cref="CoordDouble"/> that is <c>t%</c> between the current coordinate and
        /// the given one.
        /// </returns>
        public CoordDouble Lerp(CoordDouble b, double t) => new CoordDouble(Lerp(Q, b.Q, t), Lerp(R, b.R, t));

        #endregion
    }
}
