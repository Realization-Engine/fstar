using FStarEquations;

namespace FStarEquationsTests;

public class NegativeForceTests
{
    // ───────────────────────────────────────────────
    // (8) DirectedForce
    // ───────────────────────────────────────────────

    [Fact]
    public void DirectedForce_MixedPositiveAndNegativeForces_ReturnsWeightedSum()
    {
        // weights = [0.6, 0.4], forces = [1.0, -0.5]
        // F_dir = 0.6*1.0 + 0.4*(-0.5) = 0.6 - 0.2 = 0.4
        double[] weights = [0.6, 0.4];
        double[] forces = [1.0, -0.5];

        var result = NegativeForce.DirectedForce(weights, forces);

        Assert.Equal(0.4, result, 10);
    }

    [Fact]
    public void DirectedForce_AllPositiveForces_ReturnsPositiveSum()
    {
        // weights = [0.5, 0.5], forces = [2.0, 3.0]
        // F_dir = 0.5*2.0 + 0.5*3.0 = 1.0 + 1.5 = 2.5
        double[] weights = [0.5, 0.5];
        double[] forces = [2.0, 3.0];

        var result = NegativeForce.DirectedForce(weights, forces);

        Assert.Equal(2.5, result, 10);
    }

    [Fact]
    public void DirectedForce_DominantNegativeForces_ReturnsNegativeResult()
    {
        // weights = [0.3, 0.7], forces = [1.0, -2.0]
        // F_dir = 0.3*1.0 + 0.7*(-2.0) = 0.3 - 1.4 = -1.1
        double[] weights = [0.3, 0.7];
        double[] forces = [1.0, -2.0];

        var result = NegativeForce.DirectedForce(weights, forces);

        Assert.Equal(-1.1, result, 10);
    }

    // ───────────────────────────────────────────────
    // (9) Damage
    // ───────────────────────────────────────────────

    [Fact]
    public void Damage_KnownValues_ReturnsExpectedDamage()
    {
        // M=5, F_neg=-0.3, tau=2
        // D = 5 * |-0.3| * 2 = 5 * 0.3 * 2 = 3.0
        var result = NegativeForce.Damage(5.0, -0.3, 2.0);

        Assert.Equal(3.0, result, 10);
    }

    [Fact]
    public void Damage_ZeroForce_ReturnsZero()
    {
        // M=5, F_neg=0, tau=2
        // D = 5 * |0| * 2 = 0.0
        var result = NegativeForce.Damage(5.0, 0.0, 2.0);

        Assert.Equal(0.0, result, 10);
    }

    // ───────────────────────────────────────────────
    // (10) EpistemicGap
    // ───────────────────────────────────────────────

    [Fact]
    public void EpistemicGap_KnownValue_ReturnsExpected()
    {
        // Mp=10, Ms=2, Fi=0.5
        // Delta = 10 / (2 * 0.5) = 10 / 1.0 = 10.0
        var result = NegativeForce.EpistemicGap(10.0, 2.0, 0.5);

        Assert.Equal(10.0, result, 10);
    }

    [Fact]
    public void EpistemicGap_HigherMp_IncreasesGap()
    {
        // Mp=10 vs Mp=20, Ms=2, Fi=0.5
        // Delta1 = 10 / (2*0.5) = 10.0
        // Delta2 = 20 / (2*0.5) = 20.0
        var gap1 = NegativeForce.EpistemicGap(10.0, 2.0, 0.5);
        var gap2 = NegativeForce.EpistemicGap(20.0, 2.0, 0.5);

        Assert.True(gap2 > gap1);
    }

    [Theory]
    [InlineData(2.0, 0.5, 10.0)]  // Delta = 10 / (2*0.5) = 10.0
    [InlineData(4.0, 0.5, 5.0)]   // Delta = 10 / (4*0.5) = 5.0  — higher Ms decreases gap
    [InlineData(2.0, 1.0, 5.0)]   // Delta = 10 / (2*1.0) = 5.0  — higher Fi decreases gap
    public void EpistemicGap_HigherMsOrFi_DecreasesGap(double ms, double fi, double expected)
    {
        // Mp fixed at 10
        var result = NegativeForce.EpistemicGap(10.0, ms, fi);

        Assert.Equal(expected, result, 10);
    }
}
