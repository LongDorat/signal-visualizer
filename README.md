# Signal Visualizer

A simple tool to visualize signals via physical connections as sine wave. This is an educational project to demonstrate how signals can be represented and visualized in a physical form.

# Getting Started

> [!NOTE]
> This project doesn't have a pre-compiled version. You will need to compile it yourself.

- Make sure that you have [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) installed on your machine.
- Clone the repository to your local machine using Git:
  ```bash
  git clone
  cd signal-visualizer/SignalVisualizer
  ```
- Publish the project using the .NET CLI:
  ```bash
  dotnet publish -c Release -r win-x64 --self-contained true
  ```
  If for linux, replace `win-x64` with `linux-x64` in the command above.
- After that, you should see a `publish` folder in the `bin/Release/net10.0` directory. Inside the `publish` folder, you will find the compiled executable file for your platform.

## Contributing

This project does not accept contributions, but you are welcome to open an issue or fork the repository and make your own modifications. If you do so, please ensure that you comply with the terms of the MIT License.

## Acknowledgements

This project is possible thanks to [Avalonia](https://avaloniaui.net/) for the UI framework and [ScottPlot](https://scottplot.net/) for the plotting library.
