using Bunit;
using Xunit;
using FStar.UI.Components.Charts;
using FStar.UI.Models;

namespace FStarEquations.App.Tests.Components.Charts;

public class LineChartTests : BunitContext
{
    [Fact]
    public void LineChart_SingleSeries_RendersOnePolyline()
    {
        var series = new List<ChartSeries>
        {
            new("Test", new List<DataPoint> { new(0, 0), new(1, 1), new(2, 4) })
        };

        var cut = Render<LineChart>(parameters => parameters
            .Add(p => p.Series, series)
            .Add(p => p.XLabel, "X")
            .Add(p => p.YLabel, "Y"));

        Assert.Single(cut.FindAll("polyline"));
    }

    [Fact]
    public void LineChart_ThreeSeries_RendersThreePolylines()
    {
        var series = new List<ChartSeries>
        {
            new("Series A", new List<DataPoint> { new(0, 0), new(1, 2), new(2, 1) }),
            new("Series B", new List<DataPoint> { new(0, 1), new(1, 3), new(2, 2) }, "#dc2626"),
            new("Series C", new List<DataPoint> { new(0, 3), new(1, 0), new(2, 5) }, "#16a34a")
        };

        var cut = Render<LineChart>(parameters => parameters
            .Add(p => p.Series, series));

        Assert.Equal(3, cut.FindAll("polyline").Count);
    }

    [Fact]
    public void LineChart_EmptySeries_RendersWithoutThrowing()
    {
        var series = new List<ChartSeries>();

        var cut = Render<LineChart>(parameters => parameters
            .Add(p => p.Series, series));

        // Should render the SVG element without throwing
        Assert.NotNull(cut.Find("svg"));
    }

    [Fact]
    public void LineChart_ContainsSvgElement()
    {
        var series = new List<ChartSeries>
        {
            new("Test", new List<DataPoint> { new(0, 0), new(5, 10) })
        };

        var cut = Render<LineChart>(parameters => parameters
            .Add(p => p.Series, series));

        var svg = cut.Find("svg");
        Assert.NotNull(svg);
        Assert.Equal("0 0 600 400", svg.GetAttribute("viewBox"));
    }

    [Fact]
    public void LineChart_WithLabels_RendersAxisLabels()
    {
        var series = new List<ChartSeries>
        {
            new("Test", new List<DataPoint> { new(0, 0), new(1, 1) })
        };

        var cut = Render<LineChart>(parameters => parameters
            .Add(p => p.Series, series)
            .Add(p => p.XLabel, "Time (s)")
            .Add(p => p.YLabel, "Amplitude"));

        var textElements = cut.FindAll("text");
        Assert.Contains(textElements, t => t.TextContent == "Time (s)");
        Assert.Contains(textElements, t => t.TextContent == "Amplitude");
    }
}
