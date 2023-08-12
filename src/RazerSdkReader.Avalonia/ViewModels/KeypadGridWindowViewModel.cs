using System.Reactive.Disposables;
using Avalonia.Media;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class KeypadGridWindowViewModel : GridViewerWindowViewModel<CChromaKeypad>
{
    public KeypadGridWindowViewModel() : base(5, 4, "Keypad")
    {
        this.WhenActivated(d =>
        {
            App.Reader.KeypadUpdated += ReaderOnKeypadUpdated;
            Disposable.Create(() => App.Reader.KeypadUpdated -= ReaderOnKeypadUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnKeypadUpdated(object? sender, CChromaKeypad e)
    {
        Update(e);
    }

    protected override Color GetColor(in CChromaKeypad data, int index)
    {
        var clr =  data.GetColor(index);
        return Color.FromRgb(clr.R, clr.G, clr.B);
    }
}