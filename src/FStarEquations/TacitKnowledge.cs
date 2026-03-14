using System;

namespace FStarEquations
{
    public static class TacitKnowledge
    {
        /// <summary>
        /// (12) K(t+1) = K(t)(1 - delta) + T(t)
        /// </summary>
        public static double KnowledgeStockNext(double kCurrent, double delta, double transmission)
        {
            return kCurrent * (1.0 - delta) + transmission;
        }

        /// <summary>
        /// (12a) T = phi * W * F_senior
        /// </summary>
        public static double Transmission(double phi, double sharedWork, double forceSenior)
        {
            return phi * sharedWork * forceSenior;
        }

        /// <summary>
        /// (12b) W = W0 * e^(-psi * M)
        /// </summary>
        public static double SharedWork(double w0, double psi, double m)
        {
            return w0 * Math.Exp(-psi * m);
        }

        /// <summary>
        /// (13) Pipeline is broken when T(t) &lt; delta * K(t)
        /// </summary>
        public static bool IsPipelineBroken(double transmission, double delta, double kCurrent)
        {
            return transmission < delta * kCurrent;
        }
    }
}
