using System;
using Avalonia.Collections;
using Avalonia.Media;
using RazerSdkReader.Structures;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    public MainWindowViewModel()
    {
        App.Reader.MouseUpdated += ReaderOnKeyboardUpdated;
        App.Reader.Start();
        
        KeyColors = new();
        for (var i = 0; i < Width * Height; i++)
        {
            KeyColors.Add(new Color());
        }
    }
    
    private DateTime _lastFrame = DateTime.Now;
    
    private void ReaderOnKeyboardUpdated(object? sender, CChromaMouse e)
    {
        var now = DateTime.Now;
        if ((now - _lastFrame).TotalMilliseconds < 15)
            return;
        
        for (var i = 0; i < Width * Height; i++)
        {
            var clr = e.GetColor(i);
            var oldClr = KeyColors[i];
            if (clr.R == oldClr.R && clr.G == oldClr.G && clr.B == oldClr.B && clr.A == oldClr.A)
                continue;
            
            KeyColors[i] = Color.FromRgb(clr.R, clr.G, clr.B);
        }
        
        _lastFrame = now;
    }

    public AvaloniaList<Color> KeyColors { get; }
    public int Width { get; set; } = 7;
    public int Height { get; set; } = 9;
    public int WidthPx => Width * 50 + 1 + 1;
    public int HeightPx => Height * 50 + 1 + 1;

    public void Dispose()
    {
        App.Reader.MouseUpdated -= ReaderOnKeyboardUpdated;
    }
}