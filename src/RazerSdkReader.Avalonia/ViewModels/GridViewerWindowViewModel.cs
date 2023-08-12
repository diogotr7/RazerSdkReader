using System;
using Avalonia.Collections;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public abstract class GridViewerWindowViewModel<T> : GridViewerWindowViewModel where T : unmanaged
{
    protected GridViewerWindowViewModel(int width, int height, string title) : base(width, height, title)
    {
    }
    
    private DateTime _lastFrame = DateTime.Now;
    
    protected void Update(T data)
    {
        var now = DateTime.Now;
        if ((now - _lastFrame).TotalMilliseconds < 15)
            return;
        
        Dispatcher.UIThread.Invoke(() =>
        {
            for (var i = 0; i < Width * Height; i++)
            {
                var clr = GetColor(data, i);
                var oldClr = KeyColors[i];
                if (clr.R == oldClr.R && clr.G == oldClr.G && clr.B == oldClr.B && clr.A == oldClr.A)
                    continue;

                KeyColors[i] = Color.FromRgb(clr.R, clr.G, clr.B);
            }

            _lastFrame = now;
        });
    }
    
    protected abstract Color GetColor(in T data, int index);
}

public abstract class GridViewerWindowViewModel : ActivatableViewModelBase, IScreen
{
    protected GridViewerWindowViewModel(int width, int height, string title)
    {
        Title = title;
        Width = width;
        Height = height;
        KeyColors = new();
        for (var i = 0; i < Width * Height; i++)
        {
            KeyColors.Add(new Color());
        }
    }
    
    public AvaloniaList<Color> KeyColors { get; }
    public string Title { get; }
    public int Width { get; }
    public int Height { get; }
    public int WidthPx => Width * 50 + 1 + 1;
    public int HeightPx => Height * 50 + 1 + 1;
    public RoutingState Router { get; } = new();
}