using Avalonia.Controls;
using Avalonia.Interactivity;

using ScottPlot.Avalonia;

using SignalVisualizer.ViewModels;

namespace SignalVisualizer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Hands the plot to the view model once the window and its controls are ready,
    /// since the plotting control is created by the XAML loader.
    /// </summary>
    private void OnWindowLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel &&
            this.FindControl<AvaPlot>("SignalPlot") is { } plotControl)
        {
            viewModel.AttachPlot(plotControl.Plot);
        }
    }
}
