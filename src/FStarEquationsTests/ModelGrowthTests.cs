using System;
using FStarEquations;

namespace FStarEquationsTests
{
    public class ModelGrowthTests
    {
        // --- MultiplierAtTime tests ---

        [Fact]
        public void MultiplierAtTime_KnownInputs_ReturnsExpectedGrowth()
        {
            // M0=2, mu=0.1, t=5
            // M(5) = 2 * e^(0.1*5) = 2 * e^(0.5)
            //      = 2 * 1.6487212707
            //      = 3.2974425414
            double expected = 3.2974425414;

            double actual = ModelGrowth.MultiplierAtTime(2.0, 0.1, 5.0);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void MultiplierAtTime_AtTimeZero_ReturnsM0()
        {
            // M(0) = M0 * e^(mu*0) = M0 * e^0 = M0 * 1 = M0
            double m0 = 7.5;

            double actual = ModelGrowth.MultiplierAtTime(m0, 0.1, 0.0);

            Assert.Equal(m0, actual, 10);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(2.0)]
        [InlineData(5.0)]
        [InlineData(10.0)]
        public void MultiplierAtTime_PositiveMu_GrowsOverTime(double t)
        {
            // With positive mu=0.05, M(t) > M0 for all t > 0
            // M(t) = 3.0 * e^(0.05*t) > 3.0
            double m0 = 3.0;
            double mu = 0.05;

            double actual = ModelGrowth.MultiplierAtTime(m0, mu, t);

            Assert.True(actual > m0);
        }

        [Fact]
        public void MultiplierAtTime_NegativeMu_ShrinksOverTime()
        {
            // M0=4, mu=-0.2, t=3
            // M(3) = 4 * e^(-0.2*3) = 4 * e^(-0.6)
            //      = 4 * 0.5488116361
            //      = 2.1952465444
            double m0 = 4.0;
            double expected = 4.0 * Math.Exp(-0.6); // 2.1952465444

            double actual = ModelGrowth.MultiplierAtTime(m0, -0.2, 3.0);

            Assert.Equal(expected, actual, 10);
            Assert.True(actual < m0);
        }
    }
}
