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

        return BuildBinaryOutput(bytes, options);
    }

    private static string BuildBinaryOutput(byte[] bytes, EncodingOptions options)
    {
        var orderedBytes = options.ByteEndian == ByteEndian.big && options.Encoding == EncodingKind.utf16
            ? SwapByteOrder(bytes)
            : bytes;

        var bits = orderedBytes.Select(byteValue => ToBitString(byteValue, options.BitOrder));

        return string.Join(" ", bits);
    }

    private static byte[] SwapByteOrder(byte[] bytes)
    {
        var swapped = (byte[])bytes.Clone();

        for (var i = 0; i + 1 < swapped.Length; i += 2)
        {
            (swapped[i], swapped[i + 1]) = (swapped[i + 1], swapped[i]);
        }

        return swapped;
    }

    private static string ToBitString(byte value, BitOrder bitOrder)
    {
        var bits = Convert.ToString(value, 2).PadLeft(8, '0');

        return bitOrder == BitOrder.msbFirst
            ? bits
            : new string(bits.Reverse().ToArray());
    }
}
