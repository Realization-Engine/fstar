using FStarEquations;

namespace FStarEquationsTests
{
    public class DivergenceTrajectoriesTests
    {
        [Fact]
        public void DfHighDt_KnownValues_ReturnsCorrect()
        {
            // alpha=0.3, S0=1, gamma=0.2, M=5, FH=4
            // dF_H/dt = 0.3 * 1 + 0.2 * 5 * 4 = 0.3 + 4.0 = 4.3
            double result = DivergenceTrajectories.DfHighDt(0.3, 1.0, 0.2, 5.0, 4.0);
            Assert.Equal(4.3, result, 10);
        }

        [Theory]
        [InlineData(0.1, 1.0, 0.1, 2.0, 3.0)]
        [InlineData(0.5, 2.0, 0.3, 1.0, 1.0)]
        public void DfHighDt_PositiveParams_AlwaysPositive(double alpha, double s0, double gamma, double m, double fh)
        {
            // With all positive parameters, alpha*S0 + gamma*M*FH > 0
            double result = DivergenceTrajectories.DfHighDt(alpha, s0, gamma, m, fh);
            Assert.True(result > 0);
        }

        [Fact]
        public void DfLowDt_UsesMax_ReturnsCorrect()
        {
            // alpha=0.3, S0=1, beta=0.1, M=5, kappa=0.2, FL=3
            // alphaS0 - betaM = 0.3*1 - 0.1*5 = 0.3 - 0.5 = -0.2
            // -kappa*FL = -0.2*3 = -0.6
            // max(-0.2, -0.6) = -0.2
            double result = DivergenceTrajectories.DfLowDt(0.3, 1.0, 0.1, 5.0, 0.2, 3.0);
            Assert.Equal(-0.2, result, 10);
        }

        [Fact]
        public void DfLowDt_FloorCase_ReturnsFloor()
        {
            // alpha=0.1, S0=1, beta=1.0, M=10, kappa=0.05, FL=2
            // alphaS0 - betaM = 0.1*1 - 1.0*10 = 0.1 - 10 = -9.9
            // -kappa*FL = -0.05*2 = -0.1
            // max(-9.9, -0.1) = -0.1 (floor kicks in)
            double result = DivergenceTrajectories.DfLowDt(0.1, 1.0, 1.0, 10.0, 0.05, 2.0);
            Assert.Equal(-0.1, result, 10);
        }

        [Fact]
        public void IsGapWidening_HighGreaterThanLow_ReturnsTrue()
        {
            // dfHighDt=4.3, dfLowDt=-0.2 → 4.3 > -0.2 → true
            bool result = DivergenceTrajectories.IsGapWidening(4.3, -0.2);
            Assert.True(result);
        }

        [Fact]
        public void IsGapWidening_Equal_ReturnsFalse()
        {
            // dfHighDt=1.0, dfLowDt=1.0 → 1.0 > 1.0 is false
            bool result = DivergenceTrajectories.IsGapWidening(1.0, 1.0);
            Assert.False(result);
        }

        [Fact]
        public void IsGapAccelerating_HighGreaterThanLow_ReturnsTrue()
        {
            // d2fHighDt2=2.0, d2fLowDt2=0.5 → 2.0 > 0.5 → true
            bool result = DivergenceTrajectories.IsGapAccelerating(2.0, 0.5);
            Assert.True(result);
        }

        [Fact]
        public void IsGapAccelerating_LowGreaterThanHigh_ReturnsFalse()
        {
            // d2fHighDt2=0.5, d2fLowDt2=2.0 → 0.5 > 2.0 is false
            bool result = DivergenceTrajectories.IsGapAccelerating(0.5, 2.0);
            Assert.False(result);
        }

        [Fact]
        public void InitialForce_KnownValues_ReturnsCorrect()
        {
            // Fmax=1.0, Savail=0.5, Spre=1.0, rho=0.5
            // F_init = 1.0 * (0.5 / 1.0)^0.5 = 1.0 * 0.5^0.5 = 1.0 * 0.70710678118... ≈ 0.7071067812
            double result = DivergenceTrajectories.InitialForce(1.0, 0.5, 1.0, 0.5);
            Assert.Equal(0.7071067812, result, 10);
        }

        [Fact]
        public void InitialForce_FullStruggleAvailable_ReturnsFmax()
        {
            // Fmax=1.0, Savail=1.0, Spre=1.0, rho=0.5
            // F_init = 1.0 * (1.0 / 1.0)^0.5 = 1.0 * 1.0 = 1.0
            double result = DivergenceTrajectories.InitialForce(1.0, 1.0, 1.0, 0.5);
            Assert.Equal(1.0, result, 10);
        }
    }
}
