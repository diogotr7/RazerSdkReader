using System.Windows.Input;
using RazerSdkReader.Avalonia.Views;
using ReactiveUI;

namespace RazerSdkReader.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        KeyboardCommand = ReactiveCommand.Create(() =>
        {
            if (_keyboard is not null)
            {
                _keyboard.Close();
                _keyboard = null;
                return;
            }
            
            _keyboard = new GridViewerWindow()
            {
                DataContext = new KeyboardGridWindowViewModel()
            };
            _keyboard.Show();
        });
        
        MouseCommand = ReactiveCommand.Create(() =>
        {
            if (_mouse is not null)
            {
                _mouse.Close();
                _mouse = null;
                return;
            }
            
            _mouse = new GridViewerWindow()
            {
                DataContext = new MouseGridWindowViewModel()
            };
            _mouse.Show();
        });
        
        MousepadCommand = ReactiveCommand.Create(() =>
        {
            if (_mousepad is not null)
            {
                _mousepad.Close();
                _mousepad = null;
                return;
            }
            
            _mousepad = new GridViewerWindow()
            {
                DataContext = new MousepadGridWindowViewModel()
            };
            _mousepad.Show();
        });
        
        HeadsetCommand = ReactiveCommand.Create(() =>
        {
            if (_headset is not null)
            {
                _headset.Close();
                _headset = null;
                return;
            }
            
            _headset = new GridViewerWindow()
            {
                DataContext = new HeadsetGridWindowViewModel()
            };
            _headset.Show();
        });
        
        KeypadCommand = ReactiveCommand.Create(() =>
        {
            if (_keypad is not null)
            {
                _keypad.Close();
                _keypad = null;
                return;
            }
            
            _keypad = new GridViewerWindow()
            {
                DataContext = new KeypadGridWindowViewModel()
            };
            _keypad.Show();
        });
        
        ChromaLinkCommand = ReactiveCommand.Create(() =>
        {
            if (_chromaLink is not null)
            {
                _chromaLink.Close();
                _chromaLink = null;
                return;
            }
            
            _chromaLink = new GridViewerWindow()
            {
                DataContext = new ChromaLinkGridWindowViewModel()
            };
            _chromaLink.Show();
        });
    }

    private GridViewerWindow? _keyboard;
    private GridViewerWindow? _mouse;
    private GridViewerWindow? _mousepad;
    private GridViewerWindow? _headset;
    private GridViewerWindow? _keypad;
    private GridViewerWindow? _chromaLink;
    public ICommand KeyboardCommand { get; }
    public ICommand MouseCommand { get; }
    public ICommand MousepadCommand { get; }
    public ICommand HeadsetCommand { get; }
    public ICommand KeypadCommand { get; }
    public ICommand ChromaLinkCommand { get; }
}