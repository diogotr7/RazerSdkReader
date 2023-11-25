using RazerSdkReader.Structures;
using ReactiveUI;
using System.Reactive.Disposables;

namespace RazerSdkReader.Avalonia.ViewModels;

public class KeyboardGridWindowViewModel : GridViewerWindowViewModel<ChromaKeyboard>
{
    public KeyboardGridWindowViewModel()
    {
        this.WhenActivated(d =>
        {
            App.Reader!.KeyboardUpdated += ReaderOnKeyboardUpdated;
            Disposable.Create(() => App.Reader.KeyboardUpdated -= ReaderOnKeyboardUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnKeyboardUpdated(object? sender, in ChromaKeyboard e)
    {
        Update(in e);
    }
}