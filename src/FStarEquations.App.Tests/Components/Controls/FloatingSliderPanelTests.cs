using Bunit;
using Xunit;
using FStar.UI.Components.Controls;

namespace FStarEquations.App.Tests.Components.Controls;

public class FloatingSliderPanelTests : BunitContext
{
    [Fact]
    public void DefaultStartMinimized_RendersPill()
    {
        var cut = Render<FloatingSliderPanel>(p => p
            .Add(x => x.Visible, true)
            .Add(x => x.Title, "Test Panel")
            .AddChildContent("<p>slider content</p>"));
        Assert.NotNull(cut.Find(".floating-slider-pill"));
        Assert.Empty(cut.FindAll(".floating-slider-panel"));
    }

    [Fact]
    public void StartMinimizedFalse_RendersExpandedPanel()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<FloatingSliderPanel>(p => p
            .Add(x => x.Visible, true)
            .Add(x => x.StartMinimized, false)
            .Add(x => x.Title, "Test Panel")
            .AddChildContent("<p>slider content</p>"));
        Assert.NotNull(cut.Find(".floating-slider-panel"));
        Assert.Empty(cut.FindAll(".floating-slider-pill"));
    }

    [Fact]
    public void WhenNotVisible_RendersNothing()
    {
        var cut = Render<FloatingSliderPanel>(p => p
            .Add(x => x.Visible, false)
            .Add(x => x.Title, "Test Panel"));
        Assert.Empty(cut.FindAll(".floating-slider-panel"));
        Assert.Empty(cut.FindAll(".floating-slider-pill"));
    }

    [Fact]
    public void Pill_ShowsTitle()
    {
        var cut = Render<FloatingSliderPanel>(p => p
            .Add(x => x.Visible, true)
            .Add(x => x.Title, "My Params"));
        Assert.Contains("My Params", cut.Find(".floating-slider-pill").TextContent);
    }

    [Fact]
    public void ClickPill_ExpandsPanel()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<FloatingSliderPanel>(p => p
            .Add(x => x.Visible, true)
            .Add(x => x.Title, "My Params")
            .AddChildContent("<p>sliders here</p>"));
        cut.Find(".floating-slider-pill").Click();
        Assert.NotNull(cut.Find(".floating-slider-panel"));
        Assert.Contains("My Params", cut.Find(".floating-title").TextContent);
    }
}
