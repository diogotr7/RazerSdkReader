using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RazerSdkReader.Avalonia.ViewModels;
using RazerSdkReader.Avalonia.Views;

namespace RazerSdkReader.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        Reader = new ChromaReader();
        Reader.Start();

        desktop.Exit += OnExit;

        desktop.MainWindow = new MainWindow()
        {
            DataContext = new MainWindowViewModel(),
        };
    }

    private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        Reader?.Dispose();
    }

    public static ChromaReader? Reader { get; set; }
}