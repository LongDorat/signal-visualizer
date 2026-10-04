using System;

using SignalVisualizer.Models;

namespace SignalVisualizer.Services;

/// <summary>
/// Draws an amplitude-shift keyed (ASK) wave that rises and falls across each bit, peaking positive for a
/// <c>1</c> bit and negative for a <c>0</c> bit.
/// </summary>
public class AmplitudeShiftKeyGenerator : WaveformGraphGenerator
{
    /// <inheritdoc />
    public override string DisplayName => "Amplitude Shift Key";

    /// <inheritdoc />
    public override GraphOptions Options { get; } = GraphOptions.Default with
    {
        Title = "Amplitude Shift Key Signal",
    };

    /// <inheritdoc />
    protected override double GetWaveValue(byte bit, double fraction)
    {
        // Rises from 0 to 1 at the midpoint, then falls back to 0.
        var triangle = 1.0 - Math.Abs(2 * fraction - 1);

        return bit == 1 ? triangle : -triangle;
    }
}
