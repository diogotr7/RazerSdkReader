using System;
using Avalonia.Controls;
using RazerEmulatorReader.Avalonia.ViewModels;

namespace RazerEmulatorReader.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosed(EventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.Dispose();
        }
    }
}