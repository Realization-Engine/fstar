using System;

namespace FStarEquations
{
    public static class OrganizationalDynamics
    {
        /// <summary>
        /// (17) ΔOⱼ = (M−1) · Fⱼ
        /// </summary>
        public static double MarginalReturn(double m, double forceJ)
        {
            return (m - 1.0) * forceJ;
        }

        /// <summary>
        /// (18) SNR = Var(F_true) / (Var(F_true) + Mp²·Var(ε) + Var(η))
        /// As Mp→∞, SNR→0
        /// </summary>
        public static double AssessmentSnr(double varForceTrue, double mPresentation, double varEpsilon, double varEta)
        {
            return varForceTrue / (varForceTrue + mPresentation * mPresentation * varEpsilon + varEta);
        }

        /// <summary>
        /// (19) F_meas = F_true + δ
        /// </summary>
        public static double MeasuredForce(double forceTrue, double deltaGaming)
        {
            return forceTrue + deltaGaming;
        }

        /// <summary>
        /// (20) Throughput = min(R_dec, M·R_exec)
        /// </summary>
        public static double OrganizationalThroughput(double rateDecision, double m, double rateExecution)
        {
            return Math.Min(rateDecision, m * rateExecution);
        }

        /// <summary>
        /// (21) OC = M · R_exec · Δt
        /// </summary>
        public static double IndecisionCost(double m, double rateExecution, double deltaTime)
        {
            return m * rateExecution * deltaTime;
        }

        /// <summary>
        /// (22) A = M(F_firm − F_comp)
        /// </summary>
        public static double CompetitiveAdvantage(double m, double forceFirm, double forceCompetitor)
        {
            return m * (forceFirm - forceCompetitor);
        }
    }
}
