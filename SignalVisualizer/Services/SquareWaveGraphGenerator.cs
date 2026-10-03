using SignalVisualizer.Models;

namespace SignalVisualizer.Services;

/// <summary>
/// Draws a non-return-to-zero (NRZ) square wave where a <c>1</c> bit is a high level
/// and a <c>0</c> bit is a low level.
/// </summary>
public class SquareWaveGraphGenerator : WaveformGraphGenerator
{
    /// <inheritdoc />
    public override string DisplayName => "Square Wave";

    /// <inheritdoc />
    public override GraphOptions Options { get; } = GraphOptions.Default with
    {
        Title = "Square Wave Signal",
        YLabel = "Level",
    };

    /// <inheritdoc />
    protected override double GetWaveValue(byte bit, double fraction) => bit == 1 ? 1.0 : -1.0;
}
