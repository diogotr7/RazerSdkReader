using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using Avalonia.Media;
using RazerEmulatorReader.Structures;
using RazerEmulatorReader.Extensions;
using ReactiveUI;

namespace RazerEmulatorReader.Avalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private RazerEmulatorReader reader;
    
    public MainWindowViewModel()
    {
        reader = new RazerEmulatorReader();
        reader.KeyboardUpdated += ReaderOnKeyboardUpdated;
        reader.Start();
        
        KeyColors = new ObservableCollection<Color>();
        for (var i = 0; i < Width * Height; i++)
        {
            KeyColors.Add(new Color());
        }
    }
    
    private void ReaderOnKeyboardUpdated(object? sender, CChromaKeyboard e)
    {
        for (int i = 0; i < Width * Height; i++)
        {
            var clr = e.GetColor(i);
            KeyColors[i] = Color.FromRgb(clr.R, clr.G, clr.B);
        }
    }

    public ObservableCollection<Color> KeyColors { get; }

    public int Width { get; set; } = 22;
    public int Height { get; set; } = 6;
}