using System;

namespace FStarEquations
{
    public static class ForceToModelTransfer
    {
        /// <summary>
        /// (26) dM_abs/dt = sum(eta_i * f_{i,high}) * tau_extract
        /// </summary>
        public static double TransferRate(double[] eta, double[] forcesHigh, double tauExtract)
        {
            double sum = 0.0;
            for (int i = 0; i < eta.Length; i++)
            {
                sum += eta[i] * forcesHigh[i];
            }
            return sum * tauExtract;
        }

        /// <summary>
        /// (26a) Validates transfer efficiencies:
        /// eta_surf in [0.9, 1.0], eta_mid in [0.3, 0.6], eta_deep in [0.0, 0.1]
        /// </summary>
        public static bool ValidateTransferEfficiencies(double etaSurface, double etaMiddle, double etaDeep)
        {
            return etaSurface >= 0.9 && etaSurface <= 1.0
                && etaMiddle >= 0.3 && etaMiddle <= 0.6
                && etaDeep >= 0.0 && etaDeep <= 0.1;
        }

        /// <summary>
        /// (27) lim M_abs = M0 + sum(eta_i * f_{i,explicit})
        /// </summary>
        public static double AbsorptionCeiling(double m0, double[] eta, double[] forcesExplicit)
        {
            double sum = 0.0;
            for (int i = 0; i < eta.Length; i++)
            {
                sum += eta[i] * forcesExplicit[i];
            }
            return m0 + sum;
        }

        /// <summary>
        /// (28) tau = tau_create + tau_eval + tau_extract
        /// </summary>
        public static double TotalTimeAllocation(double tauCreate, double tauEval, double tauExtract)
        {
            return tauCreate + tauEval + tauExtract;
        }

        /// <summary>
        /// (29) Returns whether K_tacit == K_model (typically false; K_tacit != K_model)
        /// </summary>
        public static bool IsKnowledgePreserved(double kTacit, double kModel)
        {
            return kTacit == kModel;
        }

        /// <summary>
        /// (30) Returns whether F*_post > F*_pre (tipping point has risen)
        /// </summary>
        public static bool HasTippingPointRisen(double fStarPost, double fStarPre)
        {
            return fStarPost > fStarPre;
        }

        /// <summary>
        /// (31) Returns qualityFunction(qualityCurrent, avgForceAnnotators)
        /// </summary>
        public static double ModelQualityNext(double qualityCurrent, double avgForceAnnotators, Func<double, double, double> qualityFunction)
        {
            return qualityFunction(qualityCurrent, avgForceAnnotators);
        }
    }
}
