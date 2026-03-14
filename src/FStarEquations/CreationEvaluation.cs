using System;

namespace FStarEquations
{
    public static class CreationEvaluation
    {
        /// <summary>
        /// (7) Throughput = Budget / Cost
        /// </summary>
        public static double EvaluationThroughput(double budgetEval, double costEval)
        {
            return budgetEval / costEval;
        }

        /// <summary>
        /// (7a) Reallocated force = forceHigh * fractionToEval
        /// </summary>
        public static double ReallocatedForce(double forceHigh, double fractionToEval)
        {
            return forceHigh * fractionToEval;
        }
    }
}
