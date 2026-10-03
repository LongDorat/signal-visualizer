using ScottPlot;

using SignalVisualizer.Models;

namespace SignalVisualizer.Services;

/// <summary>
/// Builds signal graphs from binary data.
/// </summary>
public interface IGraphGenerator
{
    /// <summary>Human-readable name shown in the user interface.</summary>
    string DisplayName { get; }

    /// <summary>Appearance and rendering settings used by this generator.</summary>
    GraphOptions Options { get; }

    /// <summary>
    /// Clears the supplied plot and renders a signal graph from the binary data.
    /// </summary>
    /// <param name="plot">The ScottPlot plot to draw the signal on.</param>
    /// <param name="binaryOutput">
    /// A binary string. Whitespace and separators (such as the spaces produced by the
    /// text-to-binary converter) are ignored, so the value can be passed in directly.
    /// </param>
    void GenerateSignalGraph(Plot plot, string binaryOutput);
}
