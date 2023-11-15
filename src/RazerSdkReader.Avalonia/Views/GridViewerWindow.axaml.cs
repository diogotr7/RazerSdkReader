using Avalonia.ReactiveUI;
using RazerSdkReader.Avalonia.ViewModels;

namespace RazerSdkReader.Avalonia.Views;

public partial class GridViewerWindow : ReactiveWindow<GridViewerWindowViewModel>
{
    public GridViewerWindow()
    {
        InitializeComponent();
    }
}