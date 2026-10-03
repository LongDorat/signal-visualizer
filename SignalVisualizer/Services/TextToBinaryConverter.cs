using System;
using System.Linq;
using System.Text;
using SignalVisualizer.Models;

namespace SignalVisualizer.Services;

public class TextToBinaryConverter : ITextToBinaryConverter
{
    public string ConvertTextToBinary(string text, EncodingOptions options)
    {
        var encoding = options.Encoding switch
        {
            EncodingKind.utf8 => Encoding.UTF8,
            EncodingKind.utf16 => Encoding.Unicode,
            EncodingKind.ascii => Encoding.ASCII,
            _ => throw new ArgumentOutOfRangeException(nameof(options.Encoding), "Unsupported encoding kind.")
        };

        var bytes = encoding.GetBytes(text);

        if (options.Endian == BitEndian.big)
        {
            if (options.Encoding == EncodingKind.utf16)
            {
                SwapPairs(bytes);
            }
        }

        var binary = string.Join(" ", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

        if (options.Endian == BitEndian.big)
        {
            binary = new string(binary.Reverse().ToArray());
        }

        return binary;
    }

    private static void SwapPairs(byte[] bytes)
    {
        for (int i = 0; i < bytes.Length; i += 2)
        {
            Array.Reverse(bytes, i, 2);
        }
    }
}
