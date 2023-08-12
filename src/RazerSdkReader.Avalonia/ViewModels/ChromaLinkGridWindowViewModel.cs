using System.Reactive.Disposables;
using Avalonia.Media;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class ChromaLinkGridWindowViewModel : GridViewerWindowViewModel<CChromaLink>
{
    public ChromaLinkGridWindowViewModel() : base(5, 1, "ChromaLink")
    {
        this.WhenActivated(d =>
        {
            App.Reader.ChromaLinkUpdated += ReaderOnChromaLinkUpdated;
            Disposable.Create(() => App.Reader.ChromaLinkUpdated -= ReaderOnChromaLinkUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnChromaLinkUpdated(object? sender, CChromaLink e)
    {
        Update(e);
    }

    protected override Color GetColor(in CChromaLink data, int index)
    {
        var clr =  data.GetColor(index);
        return Color.FromRgb(clr.R, clr.G, clr.B);
    }
}