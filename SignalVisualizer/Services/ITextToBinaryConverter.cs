using SignalVisualizer.Models;

namespace SignalVisualizer.Services;

public interface ITextToBinaryConverter
{
    /// <summary>
    /// Converts the given text to a binary representation using the specified encoding options.
    /// </summary>
    /// <param name="text">The text to convert.</param>
    /// <param name="options">The encoding options to use for conversion.</param>
    /// <returns>A string representing the binary output.</returns>
    string ConvertTextToBinary(string text, EncodingOptions options);
}
