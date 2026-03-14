using System;
using FStarEquations;

namespace FStarEquationsTests
{
    public class ForceToModelTransferTests
    {
        // --- TransferRate tests ---

        [Fact]
        public void TransferRate_KnownInputs_ReturnsSummedProduct()
        {
            // eta=[1.0, 0.5, 0.0], fHigh=[0.9, 0.7, 0.8], tau=0.3
            // sum = 1.0*0.9 + 0.5*0.7 + 0.0*0.8
            //     = 0.9 + 0.35 + 0.0
            //     = 1.25
            // result = 1.25 * 0.3 = 0.375
            double expected = 0.375;

            double actual = ForceToModelTransfer.TransferRate(
                new[] { 1.0, 0.5, 0.0 },
                new[] { 0.9, 0.7, 0.8 },
                0.3);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void TransferRate_ZeroTau_ReturnsZero()
        {
            // Any eta and forces, but tau=0 => result = 0
            double actual = ForceToModelTransfer.TransferRate(
                new[] { 1.0, 0.5 },
                new[] { 0.9, 0.7 },
                0.0);

            Assert.Equal(0.0, actual, 10);
        }

        // --- ValidateTransferEfficiencies tests ---

        [Fact]
        public void ValidateTransferEfficiencies_ValidValues_ReturnsTrue()
        {
            // eta_surf=0.95 in [0.9,1.0], eta_mid=0.45 in [0.3,0.6], eta_deep=0.05 in [0.0,0.1]
            bool result = ForceToModelTransfer.ValidateTransferEfficiencies(0.95, 0.45, 0.05);

            Assert.True(result);
        }

        [Fact]
        public void ValidateTransferEfficiencies_SurfaceTooLow_ReturnsFalse()
        {
            // eta_surf=0.5 < 0.9 => false
            bool result = ForceToModelTransfer.ValidateTransferEfficiencies(0.5, 0.45, 0.05);

            Assert.False(result);
        }

        [Fact]
        public void ValidateTransferEfficiencies_MiddleOutOfRange_ReturnsFalse()
        {
            // eta_mid=0.8 > 0.6 => false
            bool result = ForceToModelTransfer.ValidateTransferEfficiencies(0.95, 0.8, 0.05);

            Assert.False(result);
        }

        [Fact]
        public void ValidateTransferEfficiencies_DeepTooHigh_ReturnsFalse()
        {
            // eta_deep=0.5 > 0.1 => false
            bool result = ForceToModelTransfer.ValidateTransferEfficiencies(0.95, 0.45, 0.5);

            Assert.False(result);
        }

        // --- AbsorptionCeiling tests ---

        [Fact]
        public void AbsorptionCeiling_KnownInputs_ReturnsSumPlusM0()
        {
            // M0=3, eta=[1.0, 0.5], fExplicit=[0.8, 0.6]
            // ceiling = 3 + 1.0*0.8 + 0.5*0.6
            //         = 3 + 0.8 + 0.3
            //         = 4.1
            double expected = 4.1;

            double actual = ForceToModelTransfer.AbsorptionCeiling(
                3.0,
                new[] { 1.0, 0.5 },
                new[] { 0.8, 0.6 });

            Assert.Equal(expected, actual, 10);
        }

        // --- TotalTimeAllocation tests ---

        [Fact]
        public void TotalTimeAllocation_SimpleSum_ReturnsCorrectTotal()
        {
            // tau = 1.5 + 2.3 + 0.7 = 4.5
            double expected = 4.5;

            double actual = ForceToModelTransfer.TotalTimeAllocation(1.5, 2.3, 0.7);

            Assert.Equal(expected, actual, 10);
        }

        // --- IsKnowledgePreserved tests ---

        [Fact]
        public void IsKnowledgePreserved_DifferentValues_ReturnsFalse()
        {
            // K_tacit=10.0 != K_model=7.5 => false (typical case)
            bool result = ForceToModelTransfer.IsKnowledgePreserved(10.0, 7.5);

            Assert.False(result);
        }

        [Fact]
        public void IsKnowledgePreserved_EqualValues_ReturnsTrue()
        {
            // K_tacit=5.0 == K_model=5.0 => true (edge case)
            bool result = ForceToModelTransfer.IsKnowledgePreserved(5.0, 5.0);

            Assert.True(result);
        }

        // --- HasTippingPointRisen tests ---

        [Fact]
        public void HasTippingPointRisen_PostGreaterThanPre_ReturnsTrue()
        {
            // F*_post=0.8 > F*_pre=0.6 => true
            bool result = ForceToModelTransfer.HasTippingPointRisen(0.8, 0.6);

            Assert.True(result);
        }

        [Fact]
        public void HasTippingPointRisen_PostEqualToPre_ReturnsFalse()
        {
            // F*_post=0.6 == F*_pre=0.6 => false (not strictly greater)
            bool result = ForceToModelTransfer.HasTippingPointRisen(0.6, 0.6);

            Assert.False(result);
        }

        [Fact]
        public void HasTippingPointRisen_PostLessThanPre_ReturnsFalse()
        {
            // F*_post=0.4 < F*_pre=0.6 => false
            bool result = ForceToModelTransfer.HasTippingPointRisen(0.4, 0.6);

            Assert.False(result);
        }

        // --- ModelQualityNext tests ---

        [Fact]
        public void ModelQualityNext_DegradingFunction_ReturnsLowerQuality()
        {
            // g(q, f) = q * f, current=0.9, avgF=0.8
            // result = 0.9 * 0.8 = 0.72
            double expected = 0.72;

            double actual = ForceToModelTransfer.ModelQualityNext(
                0.9, 0.8, (q, f) => q * f);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void ModelQualityNext_ImprovingAnnotators_ReturnsHigherQuality()
        {
            // g(q, f) = q * f, current=0.9, avgF=1.1
            // result = 0.9 * 1.1 = 0.99
            double expected = 0.99;

            double actual = ForceToModelTransfer.ModelQualityNext(
                0.9, 1.1, (q, f) => q * f);

            Assert.Equal(expected, actual, 10);
        }
    }
}
