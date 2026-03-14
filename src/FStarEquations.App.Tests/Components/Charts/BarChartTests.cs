using Bunit;
using Xunit;
using FStar.UI.Components.Charts;
using FStar.UI.Models;

namespace FStarEquations.App.Tests.Components.Charts;

public class BarChartTests : BunitContext
{
    [Fact]
    public void BarChart_WithBars_RendersRects()
    {
        var bars = new List<BarItem>
        {
            new("A", 10),
            new("B", 20),
            new("C", 15)
        };

        var cut = Render<BarChart>(parameters => parameters
            .Add(p => p.Bars, bars)
            .Add(p => p.XLabel, "Category")
            .Add(p => p.YLabel, "Value"));

        var rects = cut.FindAll("rect");
        Assert.True(rects.Count >= 3, $"Expected at least 3 <rect> elements but found {rects.Count}");
    }

    [Fact]
    public void BarChart_Vertical_ContainsSvg()
    {
        var bars = new List<BarItem>
        {
            new("X", 5),
            new("Y", 10)
        };

        var cut = Render<BarChart>(parameters => parameters
            .Add(p => p.Bars, bars)
            .Add(p => p.Orientation, BarOrientation.Vertical)
            .Add(p => p.XLabel, "Items")
            .Add(p => p.YLabel, "Count"));

        var svg = cut.Find("svg");
        Assert.NotNull(svg);
        Assert.Equal("600", svg.GetAttribute("width"));
        Assert.Equal("400", svg.GetAttribute("height"));
    }

    [Fact]
    public void BarChart_Horizontal_RendersWithoutThrowing()
    {
        var bars = new List<BarItem>
        {
            new("P", 30),
            new("Q", 60)
        };

        var cut = Render<BarChart>(parameters => parameters
            .Add(p => p.Bars, bars)
            .Add(p => p.Orientation, BarOrientation.Horizontal));

        Assert.NotNull(cut.Markup);
        Assert.NotEmpty(cut.Markup);
        var rects = cut.FindAll("rect");
        Assert.True(rects.Count >= 2, $"Expected at least 2 <rect> elements but found {rects.Count}");
    }

    [Fact]
    public void BarChart_EmptyBars_RendersWithoutThrowing()
    {
        var cut = Render<BarChart>(parameters => parameters
            .Add(p => p.Bars, new List<BarItem>())
            .Add(p => p.XLabel, "Empty")
            .Add(p => p.YLabel, "Nothing"));

        var svg = cut.Find("svg");
        Assert.NotNull(svg);
        var rects = cut.FindAll("rect");
        Assert.Empty(rects);
    }
}
