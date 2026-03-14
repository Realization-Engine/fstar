using Bunit;
using Xunit;
using FStar.UI.Components.Charts;
using FStar.UI.Models;

namespace FStarEquations.App.Tests.Components.Charts;

public class TimeSeriesAnimatorTests : BunitContext
{
    [Fact]
    public void TimeSeriesAnimator_Renders_ContainsCanvas()
    {
        var cut = Render<TimeSeriesAnimator>(parameters => parameters
            .Add(p => p.TimeSeries, new List<AnimatedSeries>())
            .Add(p => p.Duration, 10)
            .Add(p => p.Dt, 0.1));

        Assert.NotNull(cut.Find("canvas"));
    }

    [Fact]
    public void TimeSeriesAnimator_WithDimensions_SetsCanvasSize()
    {
        var cut = Render<TimeSeriesAnimator>(parameters => parameters
            .Add(p => p.Width, 1024)
            .Add(p => p.Height, 768)
            .Add(p => p.TimeSeries, new List<AnimatedSeries>())
            .Add(p => p.Duration, 10)
            .Add(p => p.Dt, 0.1));

        var canvas = cut.Find("canvas");
        Assert.Equal("1024", canvas.GetAttribute("width"));
        Assert.Equal("768", canvas.GetAttribute("height"));
    }
}
