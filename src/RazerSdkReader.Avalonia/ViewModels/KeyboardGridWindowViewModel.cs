using System;
using System.Reactive.Disposables;
using Avalonia.Media;
using Avalonia.Threading;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class KeyboardGridWindowViewModel : GridViewerWindowViewModel<CChromaKeyboard>
{
    public KeyboardGridWindowViewModel() : base(22, 6, "Keyboard")
    {
        this.WhenActivated(d =>
        {
            App.Reader.KeyboardUpdated += ReaderOnKeyboardUpdated;
            Disposable.Create(() => App.Reader.KeyboardUpdated -= ReaderOnKeyboardUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnKeyboardUpdated(object? sender, CChromaKeyboard e)
    {
        Update(e);
    }

    protected override Color GetColor(in CChromaKeyboard e, int index)
    {
        var clr =  e.GetColor(index);
        return Color.FromRgb(clr.R, clr.G, clr.B);
    }
}