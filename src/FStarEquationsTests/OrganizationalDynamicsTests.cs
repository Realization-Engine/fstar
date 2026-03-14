using FStarEquations;

namespace FStarEquationsTests
{
    public class OrganizationalDynamicsTests
    {
        // --- MarginalReturn tests ---

        [Fact]
        public void MarginalReturn_KnownInputs_ReturnsExpected()
        {
            // M=5, Fj=0.8
            // ΔOⱼ = (5−1) × 0.8 = 4 × 0.8 = 3.2
            double expected = 3.2;

            double actual = OrganizationalDynamics.MarginalReturn(5.0, 0.8);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void MarginalReturn_MIsOne_ReturnsZero()
        {
            // M=1, Fj=0.8
            // ΔOⱼ = (1−1) × 0.8 = 0 × 0.8 = 0.0
            double expected = 0.0;

            double actual = OrganizationalDynamics.MarginalReturn(1.0, 0.8);

            Assert.Equal(expected, actual, 10);
        }

        // --- AssessmentSnr tests ---

        [Fact]
        public void AssessmentSnr_KnownInputs_ReturnsExpected()
        {
            // VarF=1.0, Mp=2, VarE=0.5, VarEta=0.1
            // SNR = 1.0 / (1.0 + 2²×0.5 + 0.1)
            //     = 1.0 / (1.0 + 4×0.5 + 0.1)
            //     = 1.0 / (1.0 + 2.0 + 0.1)
            //     = 1.0 / 3.1
            //     ≈ 0.3225806452
            double expected = 1.0 / 3.1;

            double actual = OrganizationalDynamics.AssessmentSnr(1.0, 2.0, 0.5, 0.1);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void AssessmentSnr_VeryLargeMp_ApproachesZero()
        {
            // As Mp→∞, the Mp²·Var(ε) term dominates, so SNR→0
            // Mp=1000000, VarF=1.0, VarE=0.5, VarEta=0.1
            // SNR = 1.0 / (1.0 + 1e12×0.5 + 0.1) ≈ 0
            double actual = OrganizationalDynamics.AssessmentSnr(1.0, 1000000.0, 0.5, 0.1);

            Assert.True(actual < 1e-6);
        }

        // --- MeasuredForce tests ---

        [Fact]
        public void MeasuredForce_SimpleAddition_ReturnsSumOfTrueAndDelta()
        {
            // F_true=0.7, δ=0.1
            // F_meas = 0.7 + 0.1 = 0.8
            double expected = 0.8;

            double actual = OrganizationalDynamics.MeasuredForce(0.7, 0.1);

            Assert.Equal(expected, actual, 10);
        }

        // --- OrganizationalThroughput tests ---

        [Fact]
        public void OrganizationalThroughput_DecisionBound_ReturnsRateDecision()
        {
            // Rdec=10, M=5, Rexec=3
            // min(10, 5×3) = min(10, 15) = 10
            double expected = 10.0;

            double actual = OrganizationalDynamics.OrganizationalThroughput(10.0, 5.0, 3.0);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void OrganizationalThroughput_ExecutionBound_ReturnsMTimesRateExecution()
        {
            // Rdec=20, M=3, Rexec=5
            // min(20, 3×5) = min(20, 15) = 15
            double expected = 15.0;

            double actual = OrganizationalDynamics.OrganizationalThroughput(20.0, 3.0, 5.0);

            Assert.Equal(expected, actual, 10);
        }

        // --- IndecisionCost tests ---

        [Fact]
        public void IndecisionCost_KnownInputs_ReturnsExpected()
        {
            // M=5, Rexec=3, Δt=2
            // OC = 5 × 3 × 2 = 30
            double expected = 30.0;

            double actual = OrganizationalDynamics.IndecisionCost(5.0, 3.0, 2.0);

            Assert.Equal(expected, actual, 10);
        }

        // --- CompetitiveAdvantage tests ---

        [Fact]
        public void CompetitiveAdvantage_FirmBetter_ReturnsPositive()
        {
            // M=3, F_firm=0.9, F_comp=0.6
            // A = 3 × (0.9 − 0.6) = 3 × 0.3 = 0.9
            double expected = 0.9;

            double actual = OrganizationalDynamics.CompetitiveAdvantage(3.0, 0.9, 0.6);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void CompetitiveAdvantage_CompetitorBetter_ReturnsNegative()
        {
            // M=3, F_firm=0.4, F_comp=0.7
            // A = 3 × (0.4 − 0.7) = 3 × (−0.3) = −0.9
            double expected = -0.9;

            double actual = OrganizationalDynamics.CompetitiveAdvantage(3.0, 0.4, 0.7);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void CompetitiveAdvantage_EqualForces_ReturnsZero()
        {
            // M=3, F_firm=0.5, F_comp=0.5
            // A = 3 × (0.5 − 0.5) = 3 × 0 = 0
            double expected = 0.0;

            double actual = OrganizationalDynamics.CompetitiveAdvantage(3.0, 0.5, 0.5);

            Assert.Equal(expected, actual, 10);
        }
    }
}
