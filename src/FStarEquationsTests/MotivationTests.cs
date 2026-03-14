using FStarEquations;

namespace FStarEquationsTests
{
    public class MotivationTests
    {
        // --- MotivationDecay tests ---

        [Fact]
        public void MotivationDecay_KnownInputs_ReturnsExpected()
        {
            // fMot0=1.0, λ=0.5, A=2
            // f_mot = 1.0 × e^(−0.5×2) = 1.0 × e^(−1) ≈ 0.3678794412
            double expected = 0.3678794412;

            double actual = Motivation.MotivationDecay(1.0, 0.5, 2.0);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void MotivationDecay_ZeroLoss_ReturnsInitialValue()
        {
            // fMot0=0.85, λ=0.5, A=0
            // f_mot = 0.85 × e^(−0.5×0) = 0.85 × e^0 = 0.85 × 1 = 0.85
            double expected = 0.85;

            double actual = Motivation.MotivationDecay(0.85, 0.5, 0.0);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void MotivationDecay_LargeLoss_StillPositive()
        {
            // fMot0=1.0, λ=2.0, A=100
            // f_mot = 1.0 × e^(−200) > 0 (exponential never reaches zero)
            double actual = Motivation.MotivationDecay(1.0, 2.0, 100.0);

            Assert.True(actual > 0.0);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(1.0)]
        [InlineData(2.0)]
        [InlineData(5.0)]
        [InlineData(10.0)]
        public void MotivationDecay_IncreasingLoss_DecreasesMonotonically(double accumulatedLoss)
        {
            // fMot0=1.0, λ=0.5
            // As A increases, e^(−λA) decreases, so f_mot decreases
            // f_mot(A) = e^(−0.5A)
            // f_mot(A+1) = e^(−0.5(A+1)) < e^(−0.5A) = f_mot(A)
            double current = Motivation.MotivationDecay(1.0, 0.5, accumulatedLoss);
            double next = Motivation.MotivationDecay(1.0, 0.5, accumulatedLoss + 1.0);

            Assert.True(next < current);
        }
    }
}
