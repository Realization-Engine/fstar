using System;

namespace FStarEquations
{
    public static class Motivation
    {
        /// <summary>
        /// (23) f_mot = f_mot(0) · e^(−λA(t))
        /// </summary>
        public static double MotivationDecay(double fMot0, double lambda, double accumulatedLoss)
        {
            return fMot0 * Math.Exp(-lambda * accumulatedLoss);
        }
    }
}
