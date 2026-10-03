using ScottPlot;

namespace SignalVisualizer.Models;

/// <summary>
/// Centralized, immutable appearance and rendering settings for signal graphs.
/// Start from <see cref="Default"/> and override individual values with a
/// <c>with</c> expression, e.g. <c>GraphOptions.Default with { LineWidth = 3 }</c>.
/// </summary>
public sealed record GraphOptions
{
    /// <summary>Shared baseline options used by the application.</summary>
    public static GraphOptions Default { get; } = new();

    /// <summary>Plot title. When <c>null</c>, the generator's display name is used.</summary>
    public string? Title { get; init; }

    /// <summary>Label shown on the horizontal axis.</summary>
    public string XLabel { get; init; } = "Bit Index";

    /// <summary>Label shown on the vertical axis.</summary>
    public string YLabel { get; init; } = "Amplitude";

    /// <summary>Peak amplitude of the drawn waveform.</summary>
    public double Amplitude { get; init; } = 1.0;

    /// <summary>Number of carrier cycles drawn for a <c>0</c> bit (the reference frequency).</summary>
    public double ZeroBitCycles { get; init; } = 1.0;

    /// <summary>Number of carrier cycles drawn for a <c>1</c> bit (the shifted frequency).</summary>
    public double OneBitCycles { get; init; } = 2.0;

    /// <summary>Number of samples per bit used to render a smooth waveform.</summary>
    public int SamplesPerBit { get; init; } = 64;

    /// <summary>Thickness of the plotted line.</summary>
    public float LineWidth { get; init; } = 2f;

    /// <summary>Vertical padding added above and below the waveform on the Y axis.</summary>
    public double AxisYMargin { get; init; } = 0.25;

    /// <summary>Color of the plotted signal line.</summary>
    public Color SignalColor { get; init; } = Color.FromHex("#2F80ED");

    /// <summary>Background color of both the figure and the data area.</summary>
    public Color BackgroundColor { get; init; } = Color.FromHex("#0d0d0d00");

    /// <summary>Color applied to axes, titles and tick labels.</summary>
    public Color ForegroundColor { get; init; } = Color.FromHex("#D7D7D7");

    /// <summary>Opacity of the major grid lines.</summary>
    public double GridMajorOpacity { get; init; } = 0.12;

    /// <summary>Opacity of the minor grid lines.</summary>
    public double GridMinorOpacity { get; init; } = 0.06;
}
