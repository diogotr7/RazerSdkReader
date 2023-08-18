using System.Reactive.Disposables;
using Avalonia.Media;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class KeypadGridWindowViewModel : GridViewerWindowViewModel<ChromaKeypad>
{
    public KeypadGridWindowViewModel()
    {
        this.WhenActivated(d =>
        {
            App.Reader!.KeypadUpdated += ReaderOnKeypadUpdated;
            Disposable.Create(() => App.Reader.KeypadUpdated -= ReaderOnKeypadUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnKeypadUpdated(object? sender, in ChromaKeypad e)
    {
        Update(in e);
    }
}