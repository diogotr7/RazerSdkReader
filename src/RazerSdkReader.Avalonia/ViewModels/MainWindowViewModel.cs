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
        App.Reader.KeyboardUpdated += ReaderOnKeyboardUpdated;
        App.Reader.Start();
        
        KeyColors = new();
        for (var i = 0; i < Width * Height; i++)
        {
            KeyColors.Add(new Color());
        }
    }
    
    private DateTime _lastFrame = DateTime.Now;
    
    private void ReaderOnKeyboardUpdated(object? sender, CChromaKeyboard e)
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
    public int Width { get; set; } = 22;
    public int Height { get; set; } = 6;

    public void Dispose()
    {
        App.Reader.KeyboardUpdated -= ReaderOnKeyboardUpdated;
    }
}