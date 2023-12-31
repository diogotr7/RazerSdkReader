using RazerSdkReader.Structures;
using ReactiveUI;
using System.Reactive.Disposables;

namespace RazerSdkReader.Avalonia.ViewModels;

public class HeadsetGridWindowViewModel : GridViewerWindowViewModel<ChromaHeadset>
{
    public HeadsetGridWindowViewModel()
    {
        this.WhenActivated(d =>
        {
            App.Reader!.HeadsetUpdated += ReaderOnHeadsetUpdated;
            Disposable.Create(() => App.Reader.HeadsetUpdated -= ReaderOnHeadsetUpdated).DisposeWith(d);
        });
    }

    private void ReaderOnHeadsetUpdated(object? sender, in ChromaHeadset e)
    {
        Update(in e);
    }
}