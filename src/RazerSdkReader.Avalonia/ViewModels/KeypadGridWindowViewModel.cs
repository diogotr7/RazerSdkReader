using System.Reactive.Disposables;
using Avalonia.Media;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class KeypadGridWindowViewModel : GridViewerWindowViewModel<ChromaKeypad>
{
    public KeypadGridWindowViewModel() : base(5, 4, "Keypad")
    {
        this.WhenActivated(d =>
        {
            App.Reader.KeypadUpdated += ReaderOnKeypadUpdated;
            Disposable.Create(() => App.Reader.KeypadUpdated -= ReaderOnKeypadUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnKeypadUpdated(object? sender, ChromaKeypad e)
    {
        Update(e);
    }
}