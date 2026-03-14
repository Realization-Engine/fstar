using System;

namespace FStarEquations
{
    public static class ModelGrowth
    {
        /// <summary>
        /// (25) M(t) = M0 * e^(mu * t)
        /// </summary>
        public static double MultiplierAtTime(double m0, double mu, double t)
        {
            return m0 * Math.Exp(mu * t);
        }
    }
}
