namespace FStarEquations;

/// <summary>
/// Equations (11)-(14): Force dynamics, layer decomposition, tipping points, and hysteresis.
/// </summary>
public static class ForceDynamics
{
    /// <summary>
    /// (11) dF/dt = alpha*S + gamma*E*F - beta*R - sigma*M_abs.
    /// </summary>
    public static double DfDt(double alpha, double struggle, double gamma, double engagement,
        double force, double beta, double replacement, double sigma, double mAbsorbed)
        => alpha * struggle + gamma * engagement * force - beta * replacement - sigma * mAbsorbed;

    /// <summary>
    /// (11a) df_surf/dt = -beta_s * R.
    /// </summary>
    public static double DfSurfaceDt(double betaSurface, double replacement)
        => -betaSurface * replacement;

    /// <summary>
    /// (11b) df_mid/dt = alpha_m * S + gamma_m * E * f_mid - beta_m * R.
    /// </summary>
    public static double DfMiddleDt(double alphaM, double struggle, double gammaM,
        double engagement, double forceMid, double betaM, double replacement)
        => alphaM * struggle + gammaM * engagement * forceMid - betaM * replacement;

    /// <summary>
    /// (11c) df_deep/dt = alpha_d * S - beta_d * R.
    /// </summary>
    public static double DfDeepDt(double alphaD, double struggle, double betaD, double replacement)
        => alphaD * struggle - betaD * replacement;

    /// <summary>
    /// (14) Tipping Point: F* = (beta*R + sigma*M_abs) / (gamma*E).
    /// </summary>
    public static double TippingPoint(double beta, double replacement, double sigma,
        double mAbsorbed, double gamma, double engagement)
        => (beta * replacement + sigma * mAbsorbed) / (gamma * engagement);

    /// <summary>
    /// (14a) Hysteresis is present when |decay rate| > |recovery rate|.
    /// </summary>
    public static bool IsHysteresisPresent(double dfDtDecay, double dfDtRecovery)
        => Math.Abs(dfDtDecay) > Math.Abs(dfDtRecovery);
}
