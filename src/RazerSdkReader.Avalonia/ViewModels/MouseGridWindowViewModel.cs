using RazerSdkReader.Structures;
using ReactiveUI;
using System.Reactive.Disposables;

namespace RazerSdkReader.Avalonia.ViewModels;

public class MouseGridWindowViewModel : GridViewerWindowViewModel<ChromaMouse>
{
    public MouseGridWindowViewModel()
    {
        this.WhenActivated(d =>
        {
            App.Reader!.MouseUpdated += ReaderOnMouseUpdated;
            Disposable.Create(() => App.Reader.MouseUpdated -= ReaderOnMouseUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnMouseUpdated(object? sender, in ChromaMouse e)
    {
        Update(in e);
    }
}