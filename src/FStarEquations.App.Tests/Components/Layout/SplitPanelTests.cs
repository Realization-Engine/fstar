using FStar.UI.Components.Layout;

namespace FStarEquations.App.Tests.Components.Layout;

public class SplitPanelTests : BunitContext
{
    [Fact]
    public void SplitPanel_WithBothPanels_RendersBothRegions()
    {
        var cut = Render<SplitPanel>(parameters => parameters
            .Add(p => p.Left, "<p class=\"left-content\">Chart area</p>")
            .Add(p => p.Right, "<p class=\"right-content\">Controls area</p>"));

        var left = cut.Find(".split-panel-left .left-content");
        Assert.Contains("Chart area", left.TextContent);

        var right = cut.Find(".split-panel-right .right-content");
        Assert.Contains("Controls area", right.TextContent);
    }

    [Fact]
    public void SplitPanel_RendersWithoutThrowing()
    {
        var cut = Render<SplitPanel>();

        var panel = cut.Find(".split-panel");
        Assert.NotNull(panel);
    }
}
