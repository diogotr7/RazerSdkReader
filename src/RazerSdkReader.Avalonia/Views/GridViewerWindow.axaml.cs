using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using RazerSdkReader.Avalonia.ViewModels;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.Views;

public partial class GridViewerWindow : ReactiveWindow<GridViewerWindowViewModel>
{
    public GridViewerWindow()
    {
        InitializeComponent();
    }
}