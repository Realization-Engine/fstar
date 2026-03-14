using Bunit;
using Xunit;
using FStar.UI.Components.Controls;

namespace FStarEquations.App.Tests.Components.Controls;

public class ThresholdIndicatorTests : BunitContext
{
    [Fact]
    public void ThresholdIndicator_Passed_ShowsGreen()
    {
        var cut = Render<ThresholdIndicator>(parameters => parameters
            .Add(p => p.Label, "Convergence")
            .Add(p => p.Passed, true));

        var badge = cut.Find(".threshold-indicator");
        Assert.Contains("threshold-pass", badge.ClassList);
    }

    [Fact]
    public void ThresholdIndicator_Failed_ShowsRed()
    {
        var cut = Render<ThresholdIndicator>(parameters => parameters
            .Add(p => p.Label, "Convergence")
            .Add(p => p.Passed, false));

        var badge = cut.Find(".threshold-indicator");
        Assert.Contains("threshold-fail", badge.ClassList);
    }

    [Fact]
    public void ThresholdIndicator_DisplaysLabel()
    {
        var cut = Render<ThresholdIndicator>(parameters => parameters
            .Add(p => p.Label, "Stability Check")
            .Add(p => p.Passed, true));

        var badge = cut.Find(".threshold-indicator");
        Assert.Equal("Stability Check", badge.TextContent.Trim());
    }
}
