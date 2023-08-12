using System;
using System.Reactive.Disposables;
using Avalonia.Media;
using Avalonia.Threading;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class MouseGridWindowViewModel : GridViewerWindowViewModel<CChromaMouse>
{
    public MouseGridWindowViewModel() : base(7, 9, "Mouse")
    {
        this.WhenActivated(d =>
        {
            App.Reader.MouseUpdated += ReaderOnMouseUpdated;
            Disposable.Create(() => App.Reader.MouseUpdated -= ReaderOnMouseUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnMouseUpdated(object? sender, CChromaMouse e)
    {
        Update(e);
    }
    
    protected override Color GetColor(in CChromaMouse e, int index)
    {
        var clr =  e.GetColor(index);
        return Color.FromRgb(clr.R, clr.G, clr.B);
    }
}