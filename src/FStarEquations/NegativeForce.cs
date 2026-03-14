namespace FStarEquations;

/// <summary>
/// Equations (8)-(10): Negative force, damage, and epistemic gap.
/// </summary>
public static class NegativeForce
{
    /// <summary>
    /// (8) Directed Force: F_dir = Sum(w_i * f_i), where some f_i can be negative.
    /// </summary>
    public static double DirectedForce(double[] weights, double[] forces)
        => weights.Zip(forces, (w, f) => w * f).Sum();

    /// <summary>
    /// (9) Damage: D = M * |F_neg| * tau.
    /// </summary>
    public static double Damage(double m, double forceNeg, double tau)
        => m * Math.Abs(forceNeg) * tau;

    /// <summary>
    /// (10) Epistemic Gap: Delta = Mp / (Ms * Fi).
    /// </summary>
    public static double EpistemicGap(double mPresentation, double mSubstance, double forceIndividual)
        => mPresentation / (mSubstance * forceIndividual);
}
