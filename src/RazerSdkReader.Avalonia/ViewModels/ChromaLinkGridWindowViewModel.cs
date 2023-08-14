using System.Reactive.Disposables;
using Avalonia.Media;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class ChromaLinkGridWindowViewModel : GridViewerWindowViewModel<ChromaLink>
{
    public ChromaLinkGridWindowViewModel() : base(5, 1, "ChromaLink")
    {
        this.WhenActivated(d =>
        {
            App.Reader.ChromaLinkUpdated += ReaderOnChromaLinkUpdated;
            Disposable.Create(() => App.Reader.ChromaLinkUpdated -= ReaderOnChromaLinkUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnChromaLinkUpdated(object? sender, ChromaLink e)
    {
        Update(e);
    }

    protected override Color GetColor(in ChromaLink data, int index)
    {
        var clr =  data.GetColor(index);
        return Color.FromRgb(clr.R, clr.G, clr.B);
    }
}