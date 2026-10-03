using System;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using SignalVisualizer.Models;
using SignalVisualizer.Services;

namespace SignalVisualizer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ITextToBinaryConverter _textToBinaryConverter;

    public MainViewModel(ITextToBinaryConverter textToBinaryConverter)
    {
        _textToBinaryConverter = textToBinaryConverter;
    }

    [ObservableProperty]
    private string _userInput = string.Empty;

    [ObservableProperty]
    private string _binaryOutput = string.Empty;

    [ObservableProperty]
    private bool _isBigEndian;

    [ObservableProperty]
    private bool _isLittleEndian = true;

    // Maps to the EncodingComboBox items: 0 = UTF-8, 1 = UTF-16, 2 = ASCII.
    [ObservableProperty]
    private int _encodingIndex;

    private static readonly TimeSpan InputDelay = TimeSpan.FromMilliseconds(1000);

    private CancellationTokenSource? _debounceCts;

    private BitEndian Endian => IsBigEndian ? BitEndian.big : BitEndian.little;

    private EncodingKind Encoding => EncodingIndex switch
    {
        1 => EncodingKind.utf16,
        2 => EncodingKind.ascii,
        _ => EncodingKind.utf8,
    };

    private EncodingOptions BuildEncodingOptions() => new(Encoding, Endian);

    partial void OnUserInputChanged(string value)
    {
        // Text input is debounced to avoid converting on every keystroke.
        ScheduleConversion();
    }

    partial void OnIsBigEndianChanged(bool value)
    {
        if (value)
        {
            Convert();
        }
    }

    partial void OnIsLittleEndianChanged(bool value)
    {
        if (value)
        {
            Convert();
        }
    }

    partial void OnEncodingIndexChanged(int value) => Convert();

    private void ScheduleConversion()
    {
        // Cancel the pending delay and start a fresh one.
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();

        var cts = new CancellationTokenSource();
        _debounceCts = cts;

        _ = DebounceAsync(cts.Token);
    }

    private async Task DebounceAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(InputDelay, token);
        }
        catch (OperationCanceledException)
        {
            // A newer keystroke superseded this one; ignore.
            return;
        }

        Convert();
    }

    private void Convert()
    {
        BinaryOutput = _textToBinaryConverter.ConvertTextToBinary(UserInput, BuildEncodingOptions());
    }
}
