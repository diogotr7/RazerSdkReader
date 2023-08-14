using RazerSdkReader.Structures;
using RazerSdkReader.Wooting.Properties;
using Wooting;

namespace RazerSdkReader.Wooting;

public class MyApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon = new();
    private readonly ContextMenuStrip _contextMenuStrip = new();
    private readonly List<RGBDeviceInfo> _devices = new();
    private RazerSdkReader _razerEmulatorReader = new();

    public MyApplicationContext()
    {
        _notifyIcon.Icon = new Icon(Resources.wooting, 40, 40);
        _notifyIcon.Visible = true;
        _notifyIcon.ContextMenuStrip = _contextMenuStrip;

        _contextMenuStrip.Items.Add("Exit", null, Exit);

        if (!RGBControl.IsConnected())
        {
            MessageBox.Show("Wooting keyboard not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Exit(null, null);
        }

        var count = RGBControl.GetDeviceCount();
        if (count == 0)
        {
            MessageBox.Show("Wooting keyboard not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Exit(null, null);
        }

        for (byte i = 0; i < count; i++)
        {
            RGBControl.SetControlDevice(i);
            _devices.Add(RGBControl.GetDeviceInfo());
        }
        
        _razerEmulatorReader.KeyboardUpdated += RazerEmulatorReaderOnKeyboardUpdated;
        _razerEmulatorReader.Start();
    }

    private void RazerEmulatorReaderOnKeyboardUpdated(object? sender, ChromaKeyboard e)
    {
        const byte WIDTH = 22;
        const byte HEIGHT = 6;

        for (byte y = 1; y < HEIGHT; y++)
        {
            for (byte x = 1; x < WIDTH; x++)
            {
                var key = e.GetColor(y * WIDTH + x);
                for (byte i = 0; i < _devices.Count; i++)
                {
                    var wootingX = (byte)(x - 1);
                    var wootingY = (byte)(y - 0);
                    RGBControl.SetControlDevice(i);
                    RGBControl.SetKey(wootingY, wootingX, key.R, key.G, key.B);
                }
            }
        }

        for (byte i = 0; i < _devices.Count; i++)
        {
            RGBControl.SetControlDevice(i);
            RGBControl.UpdateKeyboard();
        }
    }

    private void Exit(object? sender, EventArgs e)
    {
        _notifyIcon.Visible = false;
        _razerEmulatorReader.KeyboardUpdated -= RazerEmulatorReaderOnKeyboardUpdated;
        _razerEmulatorReader.Dispose();
        for (byte i = 0; i < _devices.Count; i++)
        {
            RGBControl.SetControlDevice(i);
            RGBControl.ResetRGB();
        }
        RGBControl.Close();
        Application.Exit();
    }
}