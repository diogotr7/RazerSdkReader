using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia.Collections;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public abstract class GridViewerWindowViewModel<T> : GridViewerWindowViewModel where T : unmanaged, IColorProvider
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
                var clr = data.GetColor(i);
                var oldClr = KeyColors[i];
                if (clr.R == oldClr.R && clr.G == oldClr.G && clr.B == oldClr.B && clr.A == oldClr.A)
                    continue;

                KeyColors[i] = Color.FromRgb(clr.R, clr.G, clr.B);
            }
            KeyColors.FireCollectionChanged();
            _lastFrame = now;
        });
    }
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
    
    public MyCustomList<Color> KeyColors { get; }
    public string Title { get; }
    public int Width { get; }
    public int Height { get; }
    public int WidthPx => Width * 50 + 1 + 1;
    public int HeightPx => Height * 50 + 1 + 1;
    public RoutingState Router { get; } = new();
}

public class MyCustomList<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public void FireCollectionChanged()
    { 
        CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
    }
}