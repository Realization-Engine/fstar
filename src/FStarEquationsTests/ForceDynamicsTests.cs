using FStarEquations;

namespace FStarEquationsTests;

public class ForceDynamicsTests
{
    // ───────────────────────────────────────────────
    // (11) DfDt
    // ───────────────────────────────────────────────

    [Fact]
    public void DfDt_KnownValues_ReturnsExpected()
    {
        // alpha=0.5, S=1, gamma=0.3, E=1, F=2, beta=0.2, R=1, sigma=0.1, Mabs=1
        // dF/dt = 0.5*1 + 0.3*1*2 - 0.2*1 - 0.1*1
        //       = 0.5 + 0.6 - 0.2 - 0.1 = 0.8
        var result = ForceDynamics.DfDt(0.5, 1.0, 0.3, 1.0, 2.0, 0.2, 1.0, 0.1, 1.0);

        Assert.Equal(0.8, result, 10);
    }

    [Fact]
    public void DfDt_DecayDominates_ReturnsNegative()
    {
        // alpha=0.1, S=1, gamma=0.1, E=1, F=1, beta=0.5, R=2, sigma=0.3, Mabs=2
        // dF/dt = 0.1*1 + 0.1*1*1 - 0.5*2 - 0.3*2
        //       = 0.1 + 0.1 - 1.0 - 0.6 = -1.4
        var result = ForceDynamics.DfDt(0.1, 1.0, 0.1, 1.0, 1.0, 0.5, 2.0, 0.3, 2.0);

        Assert.Equal(-1.4, result, 10);
    }

    // ───────────────────────────────────────────────
    // (11a) DfSurfaceDt
    // ───────────────────────────────────────────────

    [Fact]
    public void DfSurfaceDt_PositiveBetaAndR_AlwaysNegative()
    {
        // df_surf/dt = -beta_s * R = -0.4 * 2.0 = -0.8
        var result = ForceDynamics.DfSurfaceDt(0.4, 2.0);

        Assert.True(result < 0);
    }

    [Fact]
    public void DfSurfaceDt_KnownValue_ReturnsExpected()
    {
        // beta_s=0.5, R=3.0
        // df_surf/dt = -0.5 * 3.0 = -1.5
        var result = ForceDynamics.DfSurfaceDt(0.5, 3.0);

        Assert.Equal(-1.5, result, 10);
    }

    // ───────────────────────────────────────────────
    // (11b) DfMiddleDt
    // ───────────────────────────────────────────────

    [Fact]
    public void DfMiddleDt_AboveTipping_ReturnsPositive()
    {
        // alpha_m=0.5, S=2, gamma_m=0.4, E=1, f_mid=3, beta_m=0.2, R=1
        // df_mid/dt = 0.5*2 + 0.4*1*3 - 0.2*1
        //           = 1.0 + 1.2 - 0.2 = 2.0
        var result = ForceDynamics.DfMiddleDt(0.5, 2.0, 0.4, 1.0, 3.0, 0.2, 1.0);

        Assert.True(result > 0);
        Assert.Equal(2.0, result, 10);
    }

    [Fact]
    public void DfMiddleDt_BelowTipping_ReturnsNegative()
    {
        // alpha_m=0.1, S=1, gamma_m=0.1, E=1, f_mid=0.5, beta_m=0.8, R=2
        // df_mid/dt = 0.1*1 + 0.1*1*0.5 - 0.8*2
        //           = 0.1 + 0.05 - 1.6 = -1.45
        var result = ForceDynamics.DfMiddleDt(0.1, 1.0, 0.1, 1.0, 0.5, 0.8, 2.0);

        Assert.True(result < 0);
        Assert.Equal(-1.45, result, 10);
    }

    // ───────────────────────────────────────────────
    // (11c) DfDeepDt
    // ───────────────────────────────────────────────

    [Fact]
    public void DfDeepDt_KnownValue_ReturnsExpected()
    {
        // alpha_d=0.3, S=2, beta_d=0.1, R=1
        // df_deep/dt = 0.3*2 - 0.1*1 = 0.6 - 0.1 = 0.5
        var result = ForceDynamics.DfDeepDt(0.3, 2.0, 0.1, 1.0);

        Assert.Equal(0.5, result, 10);
    }

    // ───────────────────────────────────────────────
    // (14) TippingPoint
    // ───────────────────────────────────────────────

    [Fact]
    public void TippingPoint_KnownValue_ReturnsExpected()
    {
        // beta=0.2, R=1, sigma=0.1, Mabs=2, gamma=0.3, E=1
        // F* = (0.2*1 + 0.1*2) / (0.3*1)
        //    = (0.2 + 0.2) / 0.3
        //    = 0.4 / 0.3 = 1.33333...
        var result = ForceDynamics.TippingPoint(0.2, 1.0, 0.1, 2.0, 0.3, 1.0);

        Assert.Equal(4.0 / 3.0, result, 10);
    }

    [Fact]
    public void TippingPoint_HigherReplacement_IncreasesThreshold()
    {
        // R=1: F* = (0.2*1 + 0.1*1) / (0.3*1) = 0.3/0.3 = 1.0
        // R=3: F* = (0.2*3 + 0.1*1) / (0.3*1) = 0.7/0.3 = 2.333...
        var tp1 = ForceDynamics.TippingPoint(0.2, 1.0, 0.1, 1.0, 0.3, 1.0);
        var tp2 = ForceDynamics.TippingPoint(0.2, 3.0, 0.1, 1.0, 0.3, 1.0);

        Assert.True(tp2 > tp1);
    }

    // ───────────────────────────────────────────────
    // (14a) IsHysteresisPresent
    // ───────────────────────────────────────────────

    [Fact]
    public void IsHysteresisPresent_DecayFasterThanRecovery_ReturnsTrue()
    {
        // |decay| = |-0.8| = 0.8 > |recovery| = |0.3| = 0.3
        var result = ForceDynamics.IsHysteresisPresent(-0.8, 0.3);

        Assert.True(result);
    }

    [Fact]
    public void IsHysteresisPresent_RecoveryFasterThanDecay_ReturnsFalse()
    {
        // |decay| = |-0.2| = 0.2 < |recovery| = |0.5| = 0.5
        var result = ForceDynamics.IsHysteresisPresent(-0.2, 0.5);

        Assert.False(result);
    }
}
