using System;

using SignalVisualizer.Models;

namespace SignalVisualizer.Services;

/// <summary>
/// Draws a frequency-shift keyed (FSK) signal where a <c>0</c> bit and a
/// <c>1</c> bit are rendered as sine waves of different frequencies.
/// </summary>
public class FrequencyShiftKeyGenerator : WaveformGraphGenerator
{
    /// <inheritdoc />
    public override string DisplayName => "Frequency Shift Key";

    /// <inheritdoc />
    public override GraphOptions Options { get; } = GraphOptions.Default with
    {
        Title = "Frequency Shift Key Signal",
    };

    /// <inheritdoc />
    protected override double GetWaveValue(byte bit, double fraction)
    {
        var cycles = bit == 1 ? Options.OneBitCycles : Options.ZeroBitCycles;

        return Math.Sin(2 * Math.PI * cycles * fraction);
    }
}
