using System;
using System.Collections;
using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// Math helper functions.
    /// Easing functions derived from
    /// https://gist.github.com/gre/1650294.
    /// </summary>
    public static class MathHelpers
    {
        /// <summary>
        /// Clamps t between 0 and 1.
        /// Ensures that functions work as expected,
        /// even if values slight above 1 or below 0 are given.
        /// </summary>
        /// <returns>The clamped version of t.</returns>
        /// <param name="t">The value to clamp.</param>
        static float CT(float t)
        {
            return Mathf.Clamp(t, 0, 1);
        }

        public static float EaseInPolynomial(float t, float power)
        {
            return Mathf.Pow(CT(t), power);
        }

        public static float EaseOutPolynomial(float t, float power)
        {
            return 1 - Mathf.Abs(Mathf.Pow(CT(t) - 1, power));
        }

        public static float EaseInOutPolynomial(float t, float power)
        {
            t = CT(t);
            return t < 0.5 ?
                        EaseInPolynomial(t * 2, power) / 2 :
                        EaseOutPolynomial(t * 2 - 1, power) / 2 + 0.5f;
        }
    }
}
