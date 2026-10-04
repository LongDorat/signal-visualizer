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
        // Peaks at 1 in the middle of the bit and returns to 0 at the edges.
        var triangle = 1.0 - Math.Abs(2 * fraction - 1);

        // A 1 bit swings the full -1 -> 1 -> -1; a 0 bit only rises to 0 (-1 -> 0 -> -1).
        return bit == 1 ? 2.0 * triangle - 1.0 : triangle - 1.0;
    }
}
