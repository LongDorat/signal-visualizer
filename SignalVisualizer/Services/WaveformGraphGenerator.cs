using System;
using System.Collections.Generic;

using ScottPlot;

using SignalVisualizer.Models;

namespace SignalVisualizer.Services;

/// <summary>
/// Shared implementation for generators that draw one carrier waveform per bit.
/// Derived generators only supply their identity and the waveform shape, while all
/// appearance is driven by the centralized <see cref="GraphOptions"/>.
/// </summary>
public abstract class WaveformGraphGenerator : IGraphGenerator
{
    /// <inheritdoc />
    public abstract string DisplayName { get; }

    /// <inheritdoc />
    public abstract GraphOptions Options { get; }

    /// <summary>
    /// Returns the normalized waveform value for a bit at the supplied position within
    /// that bit. <paramref name="fraction"/> ranges from 0 (start of the bit) to 1 (end).
    /// The base class scales the returned value by the configured amplitude.
    /// </summary>
    /// <param name="bit">The bit being rendered (0 or 1).</param>
    /// <param name="fraction">Position within the bit, from 0 to 1.</param>
    protected abstract double GetWaveValue(byte bit, double fraction);

    /// <inheritdoc />
    public virtual void GenerateSignalGraph(Plot plot, string binaryOutput)
    {
        ArgumentNullException.ThrowIfNull(plot);

        var options = Options;
        plot.Clear();

        var bits = ParseBits(binaryOutput);

        if (bits.Count == 0)
        {
            SetUpAxes(plot, options, xMax: 1);
            return;
        }

        var (xs, ys) = BuildSignal(bits, options);

        var signal = plot.Add.SignalXY(xs, ys, options.SignalColor);
        signal.LineWidth = options.LineWidth;

        SetUpAxes(plot, options, xMax: bits.Count);
    }

    /// <summary>
    /// Extracts the individual bits, ignoring every character that is not a
    /// <c>0</c> or <c>1</c> (spaces, new lines, separators, etc.).
    /// </summary>
    protected static List<byte> ParseBits(string binaryOutput)
    {
        var bits = new List<byte>();

        if (string.IsNullOrEmpty(binaryOutput))
        {
            return bits;
        }

        foreach (var character in binaryOutput)
        {
            switch (character)
            {
                case '0':
                    bits.Add(0);
                    break;
                case '1':
                    bits.Add(1);
                    break;
            }
        }

        return bits;
    }

    private (double[] Xs, double[] Ys) BuildSignal(IReadOnlyList<byte> bits, GraphOptions options)
    {
        var samplesPerBit = Math.Max(1, options.SamplesPerBit);
        var sampleCount = bits.Count * samplesPerBit;

        var xs = new double[sampleCount + 1];
        var ys = new double[sampleCount + 1];

        var index = 0;

        for (var bitIndex = 0; bitIndex < bits.Count; bitIndex++)
        {
            var bit = bits[bitIndex];

            for (var sample = 0; sample < samplesPerBit; sample++)
            {
                var fraction = (double)sample / samplesPerBit;

                xs[index] = bitIndex + fraction;
                ys[index] = options.Amplitude * GetWaveValue(bit, fraction);
                index++;
            }
        }

        // Terminate the waveform at the end of the final bit.
        xs[index] = bits.Count;
        ys[index] = options.Amplitude * GetWaveValue(bits[^1], 1.0);

        return (xs, ys);
    }

    private void SetUpAxes(Plot plot, GraphOptions options, double xMax)
    {
        plot.Title(options.Title ?? DisplayName);
        plot.XLabel(options.XLabel);
        plot.YLabel(options.YLabel);
        plot.Axes.SetLimits(
            left: 0,
            right: xMax,
            bottom: -options.Amplitude - options.AxisYMargin,
            top: options.Amplitude + options.AxisYMargin);

        ApplyTheme(plot, options);
    }

    private static void ApplyTheme(Plot plot, GraphOptions options)
    {
        plot.FigureBackground.Color = options.BackgroundColor;
        plot.DataBackground.Color = options.BackgroundColor;
        plot.Axes.Color(options.ForegroundColor);
        plot.Axes.Title.Label.ForeColor = options.ForegroundColor;
        plot.Grid.MajorLineColor = options.ForegroundColor.WithOpacity(options.GridMajorOpacity);
        plot.Grid.MinorLineColor = options.ForegroundColor.WithOpacity(options.GridMinorOpacity);
    }
}
