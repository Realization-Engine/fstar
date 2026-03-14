using FStarEquations;

namespace FStarEquationsTests
{
    public class BaseModelTests
    {
        // --- ComputeOutput tests ---

        [Fact]
        public void ComputeOutput_KnownInputs_ReturnsProductOfWeightedForces()
        {
            // M=3, forces=[0.8, 0.6], weights=[0.5, 0.5]
            // O = 3 * 0.8^0.5 * 0.6^0.5
            //   = 3 * 0.894427191 * 0.774596669
            //   = 3 * 0.692820323
            //   = 2.078460969
            double expected = 3.0 * Math.Pow(0.8, 0.5) * Math.Pow(0.6, 0.5);

            double actual = BaseModel.ComputeOutput(3.0, new[] { 0.8, 0.6 }, new[] { 0.5, 0.5 });

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void ComputeOutput_SingleForceWeightOne_ReturnsMTimesForce()
        {
            // M=4, forces=[0.7], weights=[1.0]
            // O = 4 * 0.7^1.0 = 2.8
            double expected = 2.8;

            double actual = BaseModel.ComputeOutput(4.0, new[] { 0.7 }, new[] { 1.0 });

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void ComputeOutput_AllForcesOne_ReturnsM()
        {
            // M=5, forces=[1.0, 1.0, 1.0], weights=[0.3, 0.3, 0.4]
            // O = 5 * 1^0.3 * 1^0.3 * 1^0.4 = 5 * 1 * 1 * 1 = 5
            double expected = 5.0;

            double actual = BaseModel.ComputeOutput(5.0, new[] { 1.0, 1.0, 1.0 }, new[] { 0.3, 0.3, 0.4 });

            Assert.Equal(expected, actual, 10);
        }

        // --- ValidateLayerOrdering tests ---

        [Fact]
        public void ValidateLayerOrdering_CorrectOrdering_ReturnsTrue()
        {
            // surface=10 > middle=3 > deep=1.05, deep approx 1 => true
            bool result = BaseModel.ValidateLayerOrdering(10.0, 3.0, 1.05);

            Assert.True(result);
        }

        [Fact]
        public void ValidateLayerOrdering_SwappedSurfaceAndMiddle_ReturnsFalse()
        {
            // surface=3 < middle=10 => violates surface > middle
            bool result = BaseModel.ValidateLayerOrdering(3.0, 10.0, 1.05);

            Assert.False(result);
        }

        [Fact]
        public void ValidateLayerOrdering_DeepFarFromOne_ReturnsFalse()
        {
            // surface=10 > middle=3 > deep=5.0, but deep is far from 1 => false
            bool result = BaseModel.ValidateLayerOrdering(10.0, 3.0, 5.0);

            Assert.False(result);
        }

        // --- ComputeOutputAcrossDomains tests ---

        [Fact]
        public void ComputeOutputAcrossDomains_KnownInputs_ReturnsSumOfProducts()
        {
            // domainMultipliers=[2, 3, 5], domainForces=[0.8, 0.6, 0.4]
            // O = 2*0.8 + 3*0.6 + 5*0.4
            //   = 1.6 + 1.8 + 2.0
            //   = 5.4
            double expected = 5.4;

            double actual = BaseModel.ComputeOutputAcrossDomains(
                new[] { 2.0, 3.0, 5.0 },
                new[] { 0.8, 0.6, 0.4 });

            Assert.Equal(expected, actual, 10);
        }

        // --- SkillValueSensitivity tests ---

        [Fact]
        public void SkillValueSensitivity_SimpleMultiplication_ReturnsProduct()
        {
            // dMs/dIp = 2.5, avgForce = 0.8
            // result = 2.5 * 0.8 = 2.0
            double expected = 2.0;

            double actual = BaseModel.SkillValueSensitivity(2.5, 0.8);

            Assert.Equal(expected, actual, 10);
        }
    }
}
