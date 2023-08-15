using System;
using System.Reactive.Disposables;
using Avalonia.Media;
using Avalonia.Threading;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class MouseGridWindowViewModel : GridViewerWindowViewModel<ChromaMouse>
{
    public MouseGridWindowViewModel() : base(7, 9, "Mouse")
    {
        this.WhenActivated(d =>
        {
            App.Reader.MouseUpdated += ReaderOnMouseUpdated;
            Disposable.Create(() => App.Reader.MouseUpdated -= ReaderOnMouseUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnMouseUpdated(object? sender, ChromaMouse e)
    {
        Update(e);
    }
}