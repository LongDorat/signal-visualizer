namespace SignalVisualizer.Models;

public enum EncodingKind
{
    utf8,
    utf16,
    ascii,
}

public enum BitEndian
{
    little,
    big,
}

public sealed record EncodingOptions(EncodingKind Encoding, BitEndian Endian);
