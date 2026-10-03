namespace SignalVisualizer.Models;

/// <summary>Character encoding used when converting text to bytes.</summary>
public enum EncodingKind
{
    utf8,
    utf16,
    ascii,
}

/// <summary>Byte order used when a character is encoded as more than one byte.</summary>
public enum ByteEndian
{
    little,
    big,
}

/// <summary>Order in which the bits of a byte are emitted.</summary>
public enum BitOrder
{
    msbFirst,
    lsbFirst,
}

/// <summary>
/// Immutable set of choices that control how text is converted to a binary string.
/// </summary>
/// <param name="Encoding">Character encoding to apply.</param>
/// <param name="ByteEndian">Byte order for multi-byte encodings.</param>
/// <param name="BitOrder">Order in which bits are written within each byte.</param>
public sealed record EncodingOptions(EncodingKind Encoding, ByteEndian ByteEndian, BitOrder BitOrder);
