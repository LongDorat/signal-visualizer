namespace SignalVisualizer.Models;

public enum EncodingKind
{
    utf8,
    utf16,
    ascii,
}

public enum ByteEndian
{
    little,
    big,
}

public enum BitOrder
{
    msbFirst,
    lsbFirst,
}

public sealed record EncodingOptions(EncodingKind Encoding, ByteEndian ByteEndian, BitOrder BitOrder);
