namespace FStarEquations;

public static class Integration
{
    /// <summary>
    /// Forward Euler integration: y(t+dt) = y(t) + f(t, y) * dt
    /// Returns array of y values at each time step from tStart to tEnd.
    /// </summary>
    public static double[] ForwardEuler(
        double y0,
        double tStart,
        double tEnd,
        double dt,
        Func<double, double, double> dydt)
    {
        // Validate inputs
        if (dt <= 0) throw new ArgumentException("Time step dt must be positive.", nameof(dt));
        if (tEnd <= tStart) throw new ArgumentException("tEnd must be greater than tStart.", nameof(tEnd));

        int steps = (int)Math.Ceiling((tEnd - tStart) / dt);
        var result = new double[steps + 1];
        result[0] = y0;
        double t = tStart;

        for (int i = 0; i < steps; i++)
        {
            result[i + 1] = result[i] + dydt(t, result[i]) * dt;
            t += dt;
        }

        return result;
    }

    /// <summary>
    /// Multi-variable Forward Euler for coupled ODEs.
    /// Returns jagged array: result[variableIndex][timeStepIndex].
    /// </summary>
    public static double[][] ForwardEulerSystem(
        double[] y0,
        double tStart,
        double tEnd,
        double dt,
        Func<double, double[], double[]> dydt)
    {
        if (dt <= 0) throw new ArgumentException("Time step dt must be positive.", nameof(dt));
        if (tEnd <= tStart) throw new ArgumentException("tEnd must be greater than tStart.", nameof(tEnd));

        int steps = (int)Math.Ceiling((tEnd - tStart) / dt);
        int n = y0.Length;
        var result = new double[n][];
        for (int j = 0; j < n; j++)
        {
            result[j] = new double[steps + 1];
            result[j][0] = y0[j];
        }

        var current = (double[])y0.Clone();
        double t = tStart;

        for (int i = 0; i < steps; i++)
        {
            var derivatives = dydt(t, current);
            for (int j = 0; j < n; j++)
            {
                current[j] += derivatives[j] * dt;
                result[j][i + 1] = current[j];
            }
            t += dt;
        }

        return result;
    }
}
