using System;

namespace FStarEquations
{
    public static class Sovereignty
    {
        /// <summary>
        /// (24) E[cap] = Σ Fᵢ · M · P(access)
        /// </summary>
        public static double NationalCapability(double[] forces, double m, double probabilityAccess)
        {
            double sum = 0.0;
            for (int i = 0; i < forces.Length; i++)
                sum += forces[i];
            return sum * m * probabilityAccess;
        }

        /// <summary>
        /// (24a) Σ Fᵢ ≥ minViable
        /// </summary>
        public static bool IsSovereignResilient(double[] forces, double minViable)
        {
            double sum = 0.0;
            for (int i = 0; i < forces.Length; i++)
                sum += forces[i];
            return sum >= minViable;
        }
    }
}
