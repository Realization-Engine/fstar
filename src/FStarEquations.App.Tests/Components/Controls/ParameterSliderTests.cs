using Bunit;
using Xunit;
using FStar.UI.Components.Controls;

namespace FStarEquations.App.Tests.Components.Controls;

public class ParameterSliderTests : BunitContext
{
    [Fact]
    public void ParameterSlider_WithParams_Renders()
    {
        var cut = Render<ParameterSlider>(parameters => parameters
            .Add(p => p.Label, "Growth Rate")
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 10)
            .Add(p => p.Step, 0.1)
            .Add(p => p.Value, 5));

        Assert.NotNull(cut.Markup);
        Assert.NotEmpty(cut.Markup);
    }

    [Fact]
    public void ParameterSlider_RendersInputRange()
    {
        var cut = Render<ParameterSlider>(parameters => parameters
            .Add(p => p.Label, "Rate")
            .Add(p => p.Min, 1)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 0.5)
            .Add(p => p.Value, 50));

        var input = cut.Find("input[type='range']");
        Assert.Equal("1", input.GetAttribute("min"));
        Assert.Equal("100", input.GetAttribute("max"));
        Assert.Equal("0.5", input.GetAttribute("step"));
        Assert.Equal("50", input.GetAttribute("value"));
    }

    [Fact]
    public void ParameterSlider_DisplaysLabel()
    {
        var cut = Render<ParameterSlider>(parameters => parameters
            .Add(p => p.Label, "Decay Factor")
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 1)
            .Add(p => p.Step, 0.01)
            .Add(p => p.Value, 0.5));

        var label = cut.Find(".parameter-slider-label");
        Assert.Equal("Decay Factor", label.TextContent);
    }

    [Fact]
    public void ParameterSlider_DisplaysValue()
    {
        var cut = Render<ParameterSlider>(parameters => parameters
            .Add(p => p.Label, "Test")
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 10)
            .Add(p => p.Step, 0.1)
            .Add(p => p.Value, 7.5));

        var valueSpan = cut.Find(".parameter-slider-value");
        Assert.Contains("7.5", valueSpan.TextContent);
    }
}
