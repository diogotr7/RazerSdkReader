using RazerSdkReader.Structures;
using ReactiveUI;
using System.Reactive.Disposables;

namespace RazerSdkReader.Avalonia.ViewModels;

public class ChromaLinkGridWindowViewModel : GridViewerWindowViewModel<ChromaLink>
{
    public ChromaLinkGridWindowViewModel()
    {
        this.WhenActivated(d =>
        {
            App.Reader!.ChromaLinkUpdated += ReaderOnChromaLinkUpdated;
            Disposable.Create(() => App.Reader.ChromaLinkUpdated -= ReaderOnChromaLinkUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnChromaLinkUpdated(object? sender, in ChromaLink e)
    {
        Update(in e);
    }
}