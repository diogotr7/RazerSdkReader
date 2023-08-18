using System.Reactive.Disposables;
using Avalonia.Media;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class MousepadGridWindowViewModel : GridViewerWindowViewModel<ChromaMousepad>
{
    public MousepadGridWindowViewModel()
    {
        this.WhenActivated(d =>
        {
            App.Reader!.MousepadUpdated += ReaderOnMousepadUpdated;
            Disposable.Create(() => App.Reader.MousepadUpdated -= ReaderOnMousepadUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnMousepadUpdated(object? sender, in ChromaMousepad e)
    {
        Update(in e);
    }
}