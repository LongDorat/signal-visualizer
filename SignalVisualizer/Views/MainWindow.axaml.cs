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

    private void OnWindowLoaded(object? sender, RoutedEventArgs e)
    {
        // The plotting control is created by the XAML loader, so the view model
        // receives the plot once the window (and its controls) are ready.
        if (DataContext is MainViewModel viewModel &&
            this.FindControl<AvaPlot>("SignalPlot") is { } plotControl)
        {
            viewModel.AttachPlot(plotControl.Plot);
        }
    }
}