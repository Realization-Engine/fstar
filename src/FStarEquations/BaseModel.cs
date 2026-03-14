using System;

namespace FStarEquations
{
    public static class BaseModel
    {
        /// <summary>
        /// (1) O = M * product(fi^wi)
        /// </summary>
        public static double ComputeOutput(double m, double[] forces, double[] weights)
        {
            double product = 1.0;
            for (int i = 0; i < forces.Length; i++)
            {
                product *= Math.Pow(forces[i], weights[i]);
            }
            return m * product;
        }

        /// <summary>
        /// (1a) Validates surface > middle > deep, and deep is approximately 1.
        /// </summary>
        public static bool ValidateLayerOrdering(double mEffSurface, double mEffMiddle, double mEffDeep)
        {
            return mEffSurface > mEffMiddle && mEffMiddle > mEffDeep && Math.Abs(mEffDeep - 1.0) < 0.5;
        }

        /// <summary>
        /// (2) O = sum(Ms(d) * F(d)) across domains.
        /// </summary>
        public static double ComputeOutputAcrossDomains(double[] domainMultipliers, double[] domainForces)
        {
            double sum = 0.0;
            for (int i = 0; i < domainMultipliers.Length; i++)
            {
                sum += domainMultipliers[i] * domainForces[i];
            }
            return sum;
        }

        /// <summary>
        /// (3) dVs/dIp = (dMs/dIp) * F_bar_s
        /// </summary>
        public static double SkillValueSensitivity(double dMsDIp, double avgForce)
        {
            return dMsDIp * avgForce;
        }
    }
}
