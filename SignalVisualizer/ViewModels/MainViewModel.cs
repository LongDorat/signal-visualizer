using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SignalVisualizer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _userInput = string.Empty;

    [ObservableProperty]
    private string _binaryOutput = string.Empty;

    private static readonly TimeSpan InputDelay = TimeSpan.FromMilliseconds(300);

    private CancellationTokenSource? _debounceCts;

    partial void OnUserInputChanged(string value)
    {
        // Cancel the pending delay and start a fresh one.
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _debounceCts = new CancellationTokenSource();

        _ = DebounceAsync(value, _debounceCts.Token);
    }

    private async Task DebounceAsync(string value, CancellationToken token)
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

        // Marshal to the UI thread since Task.Delay resumes on a pool thread.
        Dispatcher.UIThread.Post(() => BinaryOutput = value);
    }
}
