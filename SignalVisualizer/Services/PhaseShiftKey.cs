using System;

using SignalVisualizer.Models;

namespace SignalVisualizer.Services;
    
/// <summary>
/// Draws a phase-shift keyed (PSK) signal where a <c>0</c> bit and a
/// <c>1</c> bit are rendered as sine waves with different phases.
/// </summary>
public class PhaseShiftKeyGenerator : WaveformGraphGenerator
{
    /// <inheritdoc />
    public override string DisplayName => "Phase Shift Key";

    /// <inheritdoc />
    public override GraphOptions Options { get; } = GraphOptions.Default with
    {
        Title = "Phase Shift Key Signal",
        YLabel = "Level",
    };

    /// <inheritdoc />
    protected override double GetWaveValue(byte bit, double fraction)
    {
        // A 1 bit shifts the phase by 180 degrees (pi radians); a 0 bit keeps the phase.
        var phase = bit == 1 ? Math.PI : 0.0;

        return Math.Sin(2 * Math.PI * fraction + phase);
    }
}
