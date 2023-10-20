using System.Diagnostics;
using RazerSdkReader.Structures;
using RazerSdkReader.Wooting.Properties;

namespace RazerSdkReader.Wooting;
// ReSharper disable LocalizableElement
public class MyApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon;
    private readonly KeyboardManager _keyboardManager;
    private readonly ChromaReader? _razerEmulatorReader;
    private readonly object _lock;
    private string? _currentApp;
    
    public MyApplicationContext()
    {
        _lock = new object();
        _notifyIcon = new NotifyIcon();
        _notifyIcon.Text = "Razer SDK Reader for Wooting";
        _notifyIcon.Icon = new Icon(Resources.wooting, 40, 40);
        _notifyIcon.Visible = true;
        _notifyIcon.ContextMenuStrip = new();
        _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, TrayIconExit);
        
        _keyboardManager = new KeyboardManager();
        try
        {
            _razerEmulatorReader = new ChromaReader();
            _razerEmulatorReader.AppDataUpdated += RazerEmulatorReaderOnAppDataUpdated;
            _razerEmulatorReader.KeyboardUpdated += RazerEmulatorReaderOnKeyboardUpdated;
            _razerEmulatorReader.Start();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }

    private void RazerEmulatorReaderOnAppDataUpdated(object? sender, in ChromaAppData data)
    {
        lock (_lock)
        {
            _currentApp = data.CurrentAppName;
        
            if (string.IsNullOrEmpty(_currentApp))
            {
                _notifyIcon.Text = "Razer SDK Reader for Wooting";
                _notifyIcon.Icon = new Icon(Resources.wooting, 40, 40);
                _keyboardManager.DeactivateRgbControl();
            }
            else
            {
                _notifyIcon.Text = $"Razer SDK Reader for Wooting - {_currentApp}";
                _notifyIcon.Icon = new Icon(Resources.chroma, 40, 40);
                _keyboardManager.ActivateRgbControl();
            }
        }
    }

    private void RazerEmulatorReaderOnKeyboardUpdated(object? sender, in ChromaKeyboard e)
    {
        if (string.IsNullOrEmpty(_currentApp))
            return;
        
        lock (_lock)
        {
            for (byte y = 1; y < e.Height; y++)
            {
                for (byte x = 1; x < e.Width; x++)
                {
                    var key = e.GetColor(y * e.Width + x);
                    var wootingX = (byte)(x - 1);
                    var wootingY = (byte)(y - 0);
                    
                    _keyboardManager.SetKey(wootingY, wootingX, key.R, key.G, key.B);
                }
            }
    
            _keyboardManager.UpdateKeyboard();
        }
    }

    private void TrayIconExit(object? sender, EventArgs e)
    {
        _notifyIcon.Visible = false;
        _razerEmulatorReader!.KeyboardUpdated -= RazerEmulatorReaderOnKeyboardUpdated;
        _razerEmulatorReader!.AppDataUpdated -= RazerEmulatorReaderOnAppDataUpdated;
        _razerEmulatorReader.Dispose();
        _keyboardManager.DeactivateRgbControl();
        _keyboardManager.Dispose();

        Application.Exit();
    }
}