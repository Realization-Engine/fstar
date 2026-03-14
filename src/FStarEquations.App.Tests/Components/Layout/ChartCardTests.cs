using FStar.UI.Components.Layout;

namespace FStarEquations.App.Tests.Components.Layout;

public class ChartCardTests : BunitContext
{
    [Fact]
    public void ChartCard_WithTitle_RendersTitle()
    {
        var cut = Render<ChartCard>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .AddChildContent("<p>chart here</p>"));

        var title = cut.Find(".chart-card-title");
        Assert.Contains("Test Title", title.TextContent);
    }

    [Fact]
    public void ChartCard_WithDescription_RendersDescription()
    {
        var cut = Render<ChartCard>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Description, "A helpful description")
            .AddChildContent("<p>chart here</p>"));

        var description = cut.Find(".chart-card-description");
        Assert.Contains("A helpful description", description.TextContent);
    }

    [Fact]
    public void ChartCard_WithChildContent_RendersContent()
    {
        var cut = Render<ChartCard>(parameters => parameters
            .Add(p => p.Title, "Title")
            .AddChildContent("<p class=\"test-content\">My chart content</p>"));

        var content = cut.Find(".chart-card-content .test-content");
        Assert.Contains("My chart content", content.TextContent);
    }

    [Fact]
    public void ChartCard_WithoutTheoryContent_RendersNoTabs()
    {
        var cut = Render<ChartCard>(parameters => parameters
            .Add(p => p.Title, "Title")
            .AddChildContent("<p>chart here</p>"));

        Assert.Empty(cut.FindAll(".chart-card-tabs"));
        Assert.Empty(cut.FindAll(".tab"));
    }

    [Fact]
    public void ChartCard_WithTheoryContent_RendersTabBar()
    {
        var cut = Render<ChartCard>(parameters => parameters
            .Add(p => p.Title, "Title")
            .AddChildContent("<p>chart here</p>")
            .Add(p => p.TheoryContent, "<p>theory here</p>"));

        var tabs = cut.FindAll(".tab");
        Assert.Equal(2, tabs.Count);
        Assert.Contains("Chart", tabs[0].TextContent);
        Assert.Contains("Theory", tabs[1].TextContent);
    }

    [Fact]
    public void ChartCard_WithTheoryContent_DefaultsToChartTab()
    {
        var cut = Render<ChartCard>(parameters => parameters
            .Add(p => p.Title, "Title")
            .AddChildContent("<p class=\"chart-content\">chart here</p>")
            .Add(p => p.TheoryContent, "<p class=\"theory-text\">theory here</p>"));

        // Chart tab is active by default
        var activeTab = cut.Find(".tab.active");
        Assert.Contains("Chart", activeTab.TextContent);

        // Chart content is visible
        Assert.Single(cut.FindAll(".chart-content"));

        // Theory content is not visible
        Assert.Empty(cut.FindAll(".theory-text"));
    }

    [Fact]
    public void ChartCard_ClickTheoryTab_ShowsTheoryContent()
    {
        var cut = Render<ChartCard>(parameters => parameters
            .Add(p => p.Title, "Title")
            .AddChildContent("<p class=\"chart-content\">chart here</p>")
            .Add(p => p.TheoryContent, "<p class=\"theory-text\">theory here</p>"));

        // Click Theory tab
        var theoryTab = cut.FindAll(".tab")[1];
        theoryTab.Click();

        // Theory content is visible
        Assert.Single(cut.FindAll(".theory-text"));
        Assert.Contains("theory here", cut.Find(".theory-text").TextContent);

        // Chart content is not visible
        Assert.Empty(cut.FindAll(".chart-content"));

        // Theory tab is active
        var activeTab = cut.Find(".tab.active");
        Assert.Contains("Theory", activeTab.TextContent);
    }

    [Fact]
    public void ChartCard_ClickChartTabAfterTheory_RestoresChartContent()
    {
        var cut = Render<ChartCard>(parameters => parameters
            .Add(p => p.Title, "Title")
            .AddChildContent("<p class=\"chart-content\">chart here</p>")
            .Add(p => p.TheoryContent, "<p class=\"theory-text\">theory here</p>"));

        // Click Theory tab
        cut.FindAll(".tab")[1].Click();
        Assert.Single(cut.FindAll(".theory-text"));

        // Click Chart tab
        cut.FindAll(".tab")[0].Click();
        Assert.Single(cut.FindAll(".chart-content"));
        Assert.Empty(cut.FindAll(".theory-text"));
    }
}
