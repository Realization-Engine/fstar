using FStarEquations;

namespace FStarEquationsTests
{
    public class SovereigntyTests
    {
        // --- NationalCapability tests ---

        [Fact]
        public void NationalCapability_KnownInputs_ReturnsExpected()
        {
            // forces=[0.8, 0.6, 0.4], M=3, P=0.9
            // Σ Fᵢ = 0.8 + 0.6 + 0.4 = 1.8
            // E[cap] = 1.8 × 3 × 0.9 = 5.4 × 0.9 = 4.86
            double expected = 4.86;

            double actual = Sovereignty.NationalCapability(new[] { 0.8, 0.6, 0.4 }, 3.0, 0.9);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void NationalCapability_ZeroProbability_ReturnsZero()
        {
            // forces=[0.8, 0.6, 0.4], M=3, P=0
            // E[cap] = 1.8 × 3 × 0 = 0
            double expected = 0.0;

            double actual = Sovereignty.NationalCapability(new[] { 0.8, 0.6, 0.4 }, 3.0, 0.0);

            Assert.Equal(expected, actual, 10);
        }

        // --- IsSovereignResilient tests ---

        [Fact]
        public void IsSovereignResilient_SumAboveThreshold_ReturnsTrue()
        {
            // forces=[0.8, 0.6, 0.4], minViable=1.5
            // Σ Fᵢ = 0.8 + 0.6 + 0.4 = 1.8 ≥ 1.5 → true
            bool actual = Sovereignty.IsSovereignResilient(new[] { 0.8, 0.6, 0.4 }, 1.5);

            Assert.True(actual);
        }

        [Fact]
        public void IsSovereignResilient_SumBelowThreshold_ReturnsFalse()
        {
            // forces=[0.3, 0.2], minViable=1.0
            // Σ Fᵢ = 0.3 + 0.2 = 0.5 < 1.0 → false
            bool actual = Sovereignty.IsSovereignResilient(new[] { 0.3, 0.2 }, 1.0);

            Assert.False(actual);
        }

        [Fact]
        public void IsSovereignResilient_ExactBoundary_ReturnsTrue()
        {
            // forces=[0.5, 0.5], minViable=1.0
            // Σ Fᵢ = 0.5 + 0.5 = 1.0 ≥ 1.0 → true
            bool actual = Sovereignty.IsSovereignResilient(new[] { 0.5, 0.5 }, 1.0);

            Assert.True(actual);
        }
    }
}
