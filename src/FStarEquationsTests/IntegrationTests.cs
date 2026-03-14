namespace FStarEquationsTests;

public class IntegrationTests
{
    [Fact]
    public void ForwardEuler_ConstantDerivative_LinearResult()
    {
        // dy/dt = 1, y0 = 0, t from 0 to 1, dt = 0.1
        // Expected: y = t, so [0, 0.1, 0.2, ..., 1.0]
        var result = FStarEquations.Integration.ForwardEuler(
            y0: 0.0,
            tStart: 0.0,
            tEnd: 1.0,
            dt: 0.1,
            dydt: (t, y) => 1.0);

        Assert.Equal(11, result.Length);
        for (int i = 0; i < result.Length; i++)
        {
            double expected = i * 0.1;
            Assert.Equal(expected, result[i], precision: 10);
        }
    }

    [Fact]
    public void ForwardEuler_ExponentialGrowth_ApproximatesAnalytical()
    {
        // dy/dt = y, y0 = 1 => y = e^t
        // At t=1, y should approximate e^1 ≈ 2.71828
        var result = FStarEquations.Integration.ForwardEuler(
            y0: 1.0,
            tStart: 0.0,
            tEnd: 1.0,
            dt: 0.01,
            dydt: (t, y) => y);

        double finalValue = result[^1];
        double expected = Math.E;
        double tolerance = expected * 0.05; // 5% tolerance
        Assert.InRange(finalValue, expected - tolerance, expected + tolerance);
    }

    [Fact]
    public void ForwardEulerSystem_TwoVariables_BothIntegrated()
    {
        // dy0/dt = 1, dy1/dt = 2, starting from [0, 0]
        // Expected: y0 = t, y1 = 2t
        var result = FStarEquations.Integration.ForwardEulerSystem(
            y0: new double[] { 0.0, 0.0 },
            tStart: 0.0,
            tEnd: 1.0,
            dt: 0.1,
            dydt: (t, y) => new double[] { 1.0, 2.0 });

        Assert.Equal(2, result.Length);
        Assert.Equal(11, result[0].Length);
        Assert.Equal(11, result[1].Length);

        for (int i = 0; i < result[0].Length; i++)
        {
            double expectedY0 = i * 0.1;
            double expectedY1 = i * 0.2;
            Assert.Equal(expectedY0, result[0][i], precision: 10);
            Assert.Equal(expectedY1, result[1][i], precision: 10);
        }
    }

    [Fact]
    public void ForwardEuler_ZeroDt_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            FStarEquations.Integration.ForwardEuler(
                y0: 0.0,
                tStart: 0.0,
                tEnd: 1.0,
                dt: 0.0,
                dydt: (t, y) => 1.0));
    }

    [Fact]
    public void ForwardEuler_NegativeTimeSpan_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            FStarEquations.Integration.ForwardEuler(
                y0: 0.0,
                tStart: 1.0,
                tEnd: 0.0,
                dt: 0.1,
                dydt: (t, y) => 1.0));
    }
}
