using Bunit;
using Xunit;
using FStar.UI.Components.Charts;
using FStar.UI.Models;

namespace FStarEquations.App.Tests.Components.Charts;

public class TornadoDiagramTests : BunitContext
{
    [Fact]
    public void TornadoDiagram_WithFactors_ContainsSvg()
    {
        var factors = new List<TornadoFactor>
        {
            new("Factor A", 0.6, -0.3),
            new("Factor B", 0.4, -0.5)
        };

        var cut = Render<TornadoDiagram>(p => p
            .Add(x => x.Factors, factors));

        Assert.NotNull(cut.Find("svg"));
    }

    [Fact]
    public void TornadoDiagram_WithFactors_RendersRects()
    {
        var factors = new List<TornadoFactor>
        {
            new("Factor A", 0.6, -0.3),
            new("Factor B", 0.4, -0.5)
        };

        var cut = Render<TornadoDiagram>(p => p
            .Add(x => x.Factors, factors));

        // Each factor produces 2 rects (positive + negative)
        var rects = cut.FindAll("rect");
        Assert.True(rects.Count >= 4, $"Expected at least 4 rects but found {rects.Count}");
    }
}
