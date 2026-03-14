using Bunit;
using Xunit;
using FStarEquations.App.Pages;

namespace FStarEquations.App.Tests.Pages;

public class TippingPointTests : BunitContext
{
    [Fact]
    public void TippingPointPage_Renders_WithoutThrowing()
    {
        var cut = Render<TippingPoint>();
        Assert.NotNull(cut.Find("h1"));
    }

    [Fact]
    public void TippingPointPage_ContainsAllFourCharts()
    {
        var cut = Render<TippingPoint>();

        // Each visualization is wrapped in a ChartCard which renders a .chart-card div
        var chartCards = cut.FindAll(".chart-card");
        Assert.Equal(4, chartCards.Count);
    }

    [Fact]
    public void TippingPointPage_ContainsCrossLinks()
    {
        var cut = Render<TippingPoint>();

        var links = cut.FindAll("a[href]");
        var hrefs = links.Select(l => l.GetAttribute("href")).ToList();

        Assert.Contains("/force-dynamics", hrefs);
        Assert.Contains("/transfer", hrefs);
        Assert.Contains("/model-growth", hrefs);
        Assert.Contains("/divergence", hrefs);
        Assert.Contains("/motivation", hrefs);
    }
}
