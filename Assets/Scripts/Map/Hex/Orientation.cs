using System.Collections;
using UnityEngine;

namespace Map.Hex
{
    public struct Orientation
    {
        public double F0 { get; }
        public double F1 { get; }
        public double F2 { get; }
        public double F3 { get; }
        public double B0 { get; }
        public double B1 { get; }
        public double B2 { get; }
        public double B3 { get; }
        public double StartAngle { get; }

        public Orientation(double f0, double f1, double f2, double f3,
                           double b0, double b1, double b2, double b3,
                           double startAngle)
        {
            F0 = f0;
            F1 = f1;
            F2 = f2;
            F3 = f3;

            B0 = b0;
            B1 = b1;
            B2 = b2;
            B3 = b3;

            StartAngle = startAngle;
        }
    }
}
