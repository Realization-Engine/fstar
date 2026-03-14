using Bunit;
using Xunit;
using FStar.UI.Components.Controls;

namespace FStarEquations.App.Tests.Components.Controls;

public class SliderPanelTests : BunitContext
{
    [Fact]
    public void SliderPanel_WithChildContent_Renders()
    {
        var cut = Render<SliderPanel>(parameters => parameters
            .AddChildContent("<p>Child content here</p>"));

        Assert.NotNull(cut.Markup);
        Assert.NotEmpty(cut.Markup);
    }

    [Fact]
    public void SliderPanel_ContainsChildContent()
    {
        var cut = Render<SliderPanel>(parameters => parameters
            .AddChildContent("<p>Slider controls go here</p>"));

        Assert.Contains("Slider controls go here", cut.Markup);
    }
}
