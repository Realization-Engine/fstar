using Bunit;
using Xunit;
using FStar.UI.Components.Charts;
using FStar.UI.Models;

namespace FStarEquations.App.Tests.Components.Charts;

public class NumberLineTests : BunitContext
{
    [Fact]
    public void NumberLine_WithPoints_ContainsSvg()
    {
        var points = new List<NumberLinePoint>
        {
            new(2.0, "#2563eb"),
            new(5.0, "#dc2626")
        };

        var cut = Render<NumberLine>(p => p
            .Add(x => x.Min, 0)
            .Add(x => x.Max, 10)
            .Add(x => x.Threshold, 4.0)
            .Add(x => x.Points, points));

        Assert.NotNull(cut.Find("svg"));
    }

    [Fact]
    public void NumberLine_WithPoints_RendersCircles()
    {
        var points = new List<NumberLinePoint>
        {
            new(1.0, "#2563eb"),
            new(3.0, "#dc2626"),
            new(7.0, "#16a34a")
        };

        var cut = Render<NumberLine>(p => p
            .Add(x => x.Min, 0)
            .Add(x => x.Max, 10)
            .Add(x => x.Threshold, 5.0)
            .Add(x => x.Points, points));

        Assert.Equal(3, cut.FindAll("circle").Count);
    }

    [Fact]
    public void NumberLine_WithThreshold_RendersMarkerLine()
    {
        var points = new List<NumberLinePoint>
        {
            new(2.0, "#2563eb")
        };

        var cut = Render<NumberLine>(p => p
            .Add(x => x.Min, 0)
            .Add(x => x.Max, 10)
            .Add(x => x.Threshold, 5.0)
            .Add(x => x.Points, points));

        // The threshold marker is a line with class "threshold-line"
        var thresholdLines = cut.FindAll("line.threshold-line");
        Assert.Single(thresholdLines);
    }
}
