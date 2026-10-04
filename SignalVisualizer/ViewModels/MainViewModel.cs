using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using ScottPlot;

using SignalVisualizer.Models;
using SignalVisualizer.Services;

namespace SignalVisualizer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ITextToBinaryConverter _textToBinaryConverter;

    public MainViewModel(ITextToBinaryConverter textToBinaryConverter, IEnumerable<IGraphGenerator> generators)
    {
        _textToBinaryConverter = textToBinaryConverter;

        Generators = generators.ToList();
        _selectedGenerator = Generators[0];
    }

    /// <summary>All graph generators available for selection in the view.</summary>
    public IReadOnlyList<IGraphGenerator> Generators { get; }

    /// <summary>The currently selected graph generator.</summary>
    [ObservableProperty]
    private IGraphGenerator _selectedGenerator;

    /// <summary>The ScottPlot plot that signal graphs are drawn on.</summary>
    public Plot? SignalPlot { get; private set; }

    /// <summary>
    /// Attaches the plot owned by the view and renders the current signal into it.
    /// </summary>
    /// <param name="plot">The plot created by the view.</param>
    public void AttachPlot(Plot plot)
    {
        SignalPlot = plot;
        Convert();
    }

    /// <summary>Text entered by the user and converted to binary.</summary>
    [ObservableProperty]
    private string _userInput = string.Empty;

    /// <summary>Binary representation produced from <see cref="UserInput"/>.</summary>
    [ObservableProperty]
    private string _binaryOutput = string.Empty;

    /// <summary>Whether bits are written most-significant first.</summary>
    [ObservableProperty]
    private bool _isMsbFirst = true;

    /// <summary>Whether bits are written least-significant first.</summary>
    [ObservableProperty]
    private bool _isLsbFirst;

    /// <summary>Selected byte endianness, as an index into the endianness options.</summary>
    [ObservableProperty]
    private int _byteEndianIndex = 0;

    /// <summary>Selected character encoding, as an index into the encoding options.</summary>
    [ObservableProperty]
    private int _encodingIndex;

    private static readonly TimeSpan InputDelay = TimeSpan.FromMilliseconds(1000);

    private CancellationTokenSource? _debounceCts;

    /// <summary>Bit order derived from the MSB/LSB toggles.</summary>
    private BitOrder BitOrder => IsMsbFirst ? BitOrder.msbFirst : BitOrder.lsbFirst;

    /// <summary>Byte endianness derived from the selected index.</summary>
    private ByteEndian ByteEndian => ByteEndianIndex switch
    {
        0 => ByteEndian.little,
        _ => ByteEndian.big,
    };

    /// <summary>Character encoding derived from the selected index.</summary>
    private EncodingKind Encoding => EncodingIndex switch
    {
        1 => EncodingKind.utf16,
        2 => EncodingKind.ascii,
        _ => EncodingKind.utf8,
    };

    /// <summary>Builds the encoding options from the current UI selections.</summary>
    private EncodingOptions BuildEncodingOptions() => new(Encoding, ByteEndian, BitOrder);

    partial void OnUserInputChanged(string value)
    {
        ScheduleConversion();
    }

    partial void OnIsMsbFirstChanged(bool value)
    {
        if (value)
        {
            Convert();
        }
    }

    partial void OnIsLsbFirstChanged(bool value)
    {
        if (value)
        {
            Convert();
        }
    }

    partial void OnByteEndianIndexChanged(int value) => Convert();

    partial void OnEncodingIndexChanged(int value) => Convert();

    partial void OnSelectedGeneratorChanged(IGraphGenerator value) => UpdateSignalGraph();

    /// <summary>
    /// Starts (or restarts) the debounce timer so conversion only runs after the user
    /// stops typing.
    /// </summary>
    private void ScheduleConversion()
    {
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();

        var cts = new CancellationTokenSource();
        _debounceCts = cts;

        _ = DebounceAsync(cts.Token);
    }

    /// <summary>Waits for the debounce delay, then performs the conversion.</summary>
    /// <param name="token">Token cancelled when a newer keystroke arrives.</param>
    private async Task DebounceAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(InputDelay, token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        Convert();
    }

    /// <summary>Converts the current input into binary and refreshes the signal graph.</summary>
    private void Convert()
    {
        BinaryOutput = _textToBinaryConverter.ConvertTextToBinary(UserInput, BuildEncodingOptions());
        UpdateSignalGraph();
    }

    /// <summary>Renders the current binary output using the selected generator.</summary>
    private void UpdateSignalGraph()
    {
        if (SignalPlot is null)
        {
            return;
        }

        SelectedGenerator.GenerateSignalGraph(SignalPlot, BinaryOutput);

        SignalPlot.PlotControl?.Refresh();
    }
}
