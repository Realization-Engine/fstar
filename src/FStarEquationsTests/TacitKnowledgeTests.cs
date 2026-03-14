using FStarEquations;

namespace FStarEquationsTests
{
    public class TacitKnowledgeTests
    {
        [Fact]
        public void KnowledgeStockNext_KnownValues_ReturnsCorrect()
        {
            // K=100, delta=0.1, T=15
            // K(t+1) = 100 * (1 - 0.1) + 15 = 100 * 0.9 + 15 = 90 + 15 = 105
            double result = TacitKnowledge.KnowledgeStockNext(100.0, 0.1, 15.0);
            Assert.Equal(105.0, result, 10);
        }

        [Fact]
        public void KnowledgeStockNext_ZeroTransmission_Decays()
        {
            // K=100, delta=0.1, T=0
            // K(t+1) = 100 * (1 - 0.1) + 0 = 90
            double result = TacitKnowledge.KnowledgeStockNext(100.0, 0.1, 0.0);
            Assert.Equal(90.0, result, 10);
        }

        [Fact]
        public void Transmission_KnownValues_ReturnsCorrect()
        {
            // phi=0.8, W=5, Fs=0.9
            // T = 0.8 * 5 * 0.9 = 3.6
            double result = TacitKnowledge.Transmission(0.8, 5.0, 0.9);
            Assert.Equal(3.6, result, 10);
        }

        [Fact]
        public void SharedWork_DecaysWithM_ReturnsCorrect()
        {
            // W0=10, psi=0.5, M=2
            // W = 10 * e^(-0.5 * 2) = 10 * e^(-1) = 10 * 0.36787944117... ≈ 3.6787944117
            double result = TacitKnowledge.SharedWork(10.0, 0.5, 2.0);
            Assert.Equal(3.6787944117, result, 10);
        }

        [Fact]
        public void SharedWork_ZeroM_ReturnsW0()
        {
            // W0=10, psi=0.5, M=0
            // W = 10 * e^(-0.5 * 0) = 10 * e^0 = 10 * 1 = 10
            double result = TacitKnowledge.SharedWork(10.0, 0.5, 0.0);
            Assert.Equal(10.0, result, 10);
        }

        [Fact]
        public void IsPipelineBroken_TransmissionLessThanDeltaK_ReturnsTrue()
        {
            // T=5, delta=0.1, K=100
            // delta * K = 0.1 * 100 = 10, T=5 < 10 → true
            bool result = TacitKnowledge.IsPipelineBroken(5.0, 0.1, 100.0);
            Assert.True(result);
        }

        [Fact]
        public void IsPipelineBroken_TransmissionGreaterOrEqualDeltaK_ReturnsFalse()
        {
            // T=10, delta=0.1, K=100
            // delta * K = 0.1 * 100 = 10, T=10 >= 10 → false
            bool result = TacitKnowledge.IsPipelineBroken(10.0, 0.1, 100.0);
            Assert.False(result);
        }
    }
}
