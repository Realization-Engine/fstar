using Bunit;
using Xunit;
using FStar.UI.Components.Charts;

namespace FStarEquations.App.Tests.Components.Charts;

public class HeatMapTests : BunitContext
{
    [Fact]
    public void HeatMap_WithData_ContainsSvg()
    {
        var data = new double[,] { { 1, 2 }, { 3, 4 } };
        var xValues = new double[] { 0, 1 };
        var yValues = new double[] { 0, 1 };

        var cut = Render<HeatMap>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.XValues, xValues)
            .Add(p => p.YValues, yValues)
            .Add(p => p.XLabel, "X")
            .Add(p => p.YLabel, "Y"));

        var svg = cut.Find("svg");
        Assert.NotNull(svg);
        Assert.Equal("600", svg.GetAttribute("width"));
        Assert.Equal("400", svg.GetAttribute("height"));
    }

    [Fact]
    public void HeatMap_WithData_RendersRects()
    {
        var data = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } };
        var xValues = new double[] { 0, 1, 2 };
        var yValues = new double[] { 0, 1 };

        var cut = Render<HeatMap>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.XValues, xValues)
            .Add(p => p.YValues, yValues));

        var rects = cut.FindAll("rect");
        Assert.Equal(6, rects.Count);
    }
}
