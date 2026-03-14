using Bunit;
using Xunit;
using FStar.UI.Components.Charts;
using FStar.UI.Models;

namespace FStarEquations.App.Tests.Components.Charts;

public class PhasePortraitTests : BunitContext
{
    [Fact]
    public void PhasePortrait_WithCurve_ContainsSvg()
    {
        var curve = new List<DataPoint>
        {
            new(0, 1), new(1, 0.5), new(2, -0.5), new(3, -1)
        };

        var cut = Render<PhasePortrait>(p => p
            .Add(x => x.Curve, curve)
            .Add(x => x.XLabel, "F")
            .Add(x => x.YLabel, "dF/dt"));

        Assert.NotNull(cut.Find("svg"));
    }

    [Fact]
    public void PhasePortrait_WithCurve_RendersPolyline()
    {
        var curve = new List<DataPoint>
        {
            new(0, 1), new(1, 0.5), new(2, -0.5), new(3, -1)
        };

        var cut = Render<PhasePortrait>(p => p
            .Add(x => x.Curve, curve)
            .Add(x => x.XLabel, "F")
            .Add(x => x.YLabel, "dF/dt")
            .Add(x => x.ZeroCrossing, 1.5));

        Assert.Single(cut.FindAll("polyline"));
    }
}
