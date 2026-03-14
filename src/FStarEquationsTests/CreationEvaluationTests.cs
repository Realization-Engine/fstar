using FStarEquations;

namespace FStarEquationsTests
{
    public class CreationEvaluationTests
    {
        // --- EvaluationThroughput tests ---

        [Fact]
        public void EvaluationThroughput_KnownValues_ReturnsBudgetDividedByCost()
        {
            // Budget=1000, Cost=50
            // Throughput = 1000 / 50 = 20.0
            double expected = 20.0;

            double actual = CreationEvaluation.EvaluationThroughput(1000.0, 50.0);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void EvaluationThroughput_HighCost_ReducesThroughput()
        {
            // Budget=1000, Cost=500
            // Throughput = 1000 / 500 = 2.0
            double expected = 2.0;

            double actual = CreationEvaluation.EvaluationThroughput(1000.0, 500.0);

            Assert.Equal(expected, actual, 10);
        }

        // --- ReallocatedForce tests ---

        [Fact]
        public void ReallocatedForce_FractionOne_ReturnsFullForce()
        {
            // forceHigh=0.9, fraction=1.0
            // result = 0.9 * 1.0 = 0.9
            double expected = 0.9;

            double actual = CreationEvaluation.ReallocatedForce(0.9, 1.0);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void ReallocatedForce_FractionZero_ReturnsZero()
        {
            // forceHigh=0.9, fraction=0.0
            // result = 0.9 * 0.0 = 0.0
            double expected = 0.0;

            double actual = CreationEvaluation.ReallocatedForce(0.9, 0.0);

            Assert.Equal(expected, actual, 10);
        }
    }
}
