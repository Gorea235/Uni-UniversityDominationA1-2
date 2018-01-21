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
        /// Lerps the bitch
        /// </summary>
        /// <returns>slURP</returns>
        /// <param name="a">The first Coord item.</param>
        /// <param name="b">The second Coord item.</param>
        /// <param name="t">t I guess...</param>
        static double Lerp(double a, double b, double t)
        {
            return a * (1 - t) + b * t;
        }

        public CoordDouble Lerp(CoordDouble b, double t)
        {
            //return new CoordDouble(Q * (1 - t) + b.Q * t, R * (1 - t) + b.R * t, S * (1 - t) + b.S * t);
            //return new CoordDouble(Lerp(Q, b.Q, t), Lerp(R, b.R, t), Lerp(S, b.S, t));
            return new CoordDouble(Lerp(Q, b.Q, t), Lerp(R, b.R, t));
        }

        //static public List<Coord> Linedraw(Coord a, Coord b)
        //{
        //    int N = a.DistanceTo(b);
        //    CoordDouble a_nudge = new CoordDouble(a.Q + 0.000001, a.R + 0.000001, a.S - 0.000002);
        //    CoordDouble b_nudge = new CoordDouble(b.Q + 0.000001, b.R + 0.000001, b.S - 0.000002);
        //    List<Coord> results = new List<Coord> { };
        //    double step = 1.0 / Math.Max(N, 1);
        //    for (int i = 0; i <= N; i++)
        //        results.Add(a_nudge.Lerp(b_nudge, step * i).Round());
        //    return results;
        //}

        #endregion
    }
}
