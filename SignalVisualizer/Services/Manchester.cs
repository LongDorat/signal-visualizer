using System;

using SignalVisualizer.Models;

namespace SignalVisualizer.Services;

public class Manchester : WaveformGraphGenerator
{
    /// <inheritdoc />
    public override string DisplayName => "Manchester";

    /// <inheritdoc />
    public override GraphOptions Options { get; } = GraphOptions.Default with
    {
        Title = "Manchester Signal",
        YLabel = "Level",
    };

    /// <inheritdoc />
    protected override double GetWaveValue(byte bit, double fraction)
    {
        // A 1 bit starts high and transitions low at the midpoint; a 0 bit starts low and transitions high.
        return bit == 1
            ? (fraction < 0.5 ? 1.0 : -1.0)
            : (fraction < 0.5 ? -1.0 : 1.0);
    }
}
