using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class ViewModelBase : ReactiveObject;

public abstract class ActivatableViewModelBase : ViewModelBase, IActivatableViewModel
{
    /// <inheritdoc />
    public ViewModelActivator Activator { get; } = new();
}