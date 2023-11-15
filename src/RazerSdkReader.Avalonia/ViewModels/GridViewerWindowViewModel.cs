using Avalonia.Collections;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;
using System;

namespace RazerSdkReader.Avalonia.ViewModels;

public abstract class GridViewerWindowViewModel<T> : GridViewerWindowViewModel where T : unmanaged, IColorProvider
{
    protected GridViewerWindowViewModel()
    {
        _data = default;
        Title = typeof(T).Name.Replace("Chroma", "");
        Width = _data.Width;
        Height = _data.Height;
        for (var i = 0; i < Width * Height; i++)
        {
            KeyColors.Add(new());
        }
    }

    protected void Update(in T data)
    {
        _data = data;
        _cached ??= RunOnUiThread;

        Dispatcher.UIThread.Invoke(_cached);
    }

    private T _data;
    private Action? _cached;
    private void RunOnUiThread()
    {
        Dispatcher.UIThread.VerifyAccess();

        for (var i = 0; i < Width * Height; i++)
        {
            var color = _data.GetColor(i);
            KeyColors[i].Color = Color.FromRgb(color.R, color.G, color.B);
        }
    }
}

public abstract class GridViewerWindowViewModel : ActivatableViewModelBase, IScreen
{
    public AvaloniaList<SolidColorBrush> KeyColors { get; init; } = new();
    public string Title { get; init; } = string.Empty;
    public int Width { get; init; }
    public int Height { get; init; }
    public int WidthPx => Width * 50 + 1 + 1;
    public int HeightPx => Height * 50 + 1 + 1;
    public RoutingState Router { get; } = new();
}