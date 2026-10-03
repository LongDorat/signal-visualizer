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

        // DI supplies the generators in registration order, which is the order
        // shown in the UI. The first entry is the default selection.
        Generators = generators.ToList();
        _selectedGenerator = Generators[0];
    }

    /// <summary>All graph generators available for selection in the view.</summary>
    public IReadOnlyList<IGraphGenerator> Generators { get; }

    /// <summary>The currently selected graph generator.</summary>
    [ObservableProperty]
    private IGraphGenerator _selectedGenerator;

    public Plot? SignalPlot { get; private set; }

    public void AttachPlot(Plot plot)
    {
        SignalPlot = plot;
        Convert();
    }

    [ObservableProperty]
    private string _userInput = string.Empty;

    [ObservableProperty]
    private string _binaryOutput = string.Empty;

    [ObservableProperty]
    private bool _isMsbFirst = true;

    [ObservableProperty]
    private bool _isLsbFirst;

    [ObservableProperty]
    private int _byteEndianIndex = 0;

    [ObservableProperty]
    private int _encodingIndex;

    private static readonly TimeSpan InputDelay = TimeSpan.FromMilliseconds(1000);

    private CancellationTokenSource? _debounceCts;

    private BitOrder BitOrder => IsMsbFirst ? BitOrder.msbFirst : BitOrder.lsbFirst;

    private ByteEndian ByteEndian => ByteEndianIndex switch
    {
        0 => ByteEndian.big,
        _ => ByteEndian.little,
    };

    private EncodingKind Encoding => EncodingIndex switch
    {
        1 => EncodingKind.utf16,
        2 => EncodingKind.ascii,
        _ => EncodingKind.utf8,
    };

    private EncodingOptions BuildEncodingOptions() => new(Encoding, ByteEndian, BitOrder);

    partial void OnUserInputChanged(string value)
    {
        // Text input is debounced to avoid converting on every keystroke.
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
        UpdateSignalGraph();
    }

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
