using Bunit;
using Xunit;
using FStar.UI.Components.Charts;
using FStar.UI.Models;

namespace FStarEquations.App.Tests.Components.Charts;

public class WaterfallChartTests : BunitContext
{
    [Fact]
    public void WaterfallChart_WithItems_ContainsSvg()
    {
        var items = new List<WaterfallItem>
        {
            new("Revenue", 100),
            new("Cost", -40),
            new("Profit", 60, IsTotal: true)
        };

        var cut = Render<WaterfallChart>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.YLabel, "Value"));

        var svg = cut.Find("svg");
        Assert.NotNull(svg);
        Assert.Equal("600", svg.GetAttribute("width"));
        Assert.Equal("400", svg.GetAttribute("height"));
    }

    [Fact]
    public void WaterfallChart_WithItems_RendersRects()
    {
        var items = new List<WaterfallItem>
        {
            new("A", 50),
            new("B", -20),
            new("C", 30, IsTotal: true)
        };

        var cut = Render<WaterfallChart>(parameters => parameters
            .Add(p => p.Items, items));

        var rects = cut.FindAll("rect");
        Assert.Equal(3, rects.Count);
    }
}
