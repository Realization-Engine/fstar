using System;

namespace FStarEquations
{
    public static class DivergenceTrajectories
    {
        /// <summary>
        /// (15a) dF_H/dt = alpha * S0 + gamma * M * F_H
        /// </summary>
        public static double DfHighDt(double alpha, double struggle0, double gamma, double m, double forceHigh)
        {
            return alpha * struggle0 + gamma * m * forceHigh;
        }

        /// <summary>
        /// (15b) dF_L/dt = max(alpha * S0 - beta * M, -kappa * F_L)
        /// </summary>
        public static double DfLowDt(double alpha, double struggle0, double beta, double m, double kappa, double forceLow)
        {
            return Math.Max(alpha * struggle0 - beta * m, -kappa * forceLow);
        }

        /// <summary>
        /// (16) Gap is widening when d(F_H - F_L)/dt > 0, i.e., dfHighDt > dfLowDt
        /// </summary>
        public static bool IsGapWidening(double dfHighDt, double dfLowDt)
        {
            return dfHighDt > dfLowDt;
        }

        /// <summary>
        /// (16a) Gap is accelerating when d²(F_H - F_L)/dt² > 0, i.e., d2fHighDt2 > d2fLowDt2
        /// </summary>
        public static bool IsGapAccelerating(double d2fHighDt2, double d2fLowDt2)
        {
            return d2fHighDt2 > d2fLowDt2;
        }

        /// <summary>
        /// (32) F_init = F_max * (S_avail / S_pre)^rho
        /// </summary>
        public static double InitialForce(double forceMax, double struggleAvailable, double strugglePre, double rho)
        {
            return forceMax * Math.Pow(struggleAvailable / strugglePre, rho);
        }
    }
}
