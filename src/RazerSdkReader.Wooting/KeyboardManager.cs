using Wooting;

namespace RazerSdkReader.Wooting;

public sealed class KeyboardManager : IDisposable
{
    private static KeyboardManager? _instance;
    private RGBDeviceInfo[]? _devices;

    public KeyboardManager()
    {
        if (_instance != null)
            throw new InvalidOperationException("KeyboardManager is a singleton");

        _instance = this;
    }

    public void ActivateRgbControl()
    {
        if (_devices != null)
            return;

        if (!RGBControl.IsConnected())
            return;

        var count = RGBControl.GetDeviceCount();
        if (count == 0)
            return;

        _devices = new RGBDeviceInfo[count];

        for (byte i = 0; i < count; i++)
        {
            RGBControl.SetControlDevice(i);
            _devices[i] = RGBControl.GetDeviceInfo();
        }
    }

    public void DeactivateRgbControl()
    {
        if (_devices == null)
            return;

        for (byte i = 0; i < _devices.Length; i++)
        {
            RGBControl.SetControlDevice(i);
            RGBControl.ResetRGB();
        }

        _devices = null;
    }

    public void SetKey(byte row, byte column, byte red, byte green, byte blue)
    {
        if (_devices == null)
        {
            return;
        }

        for (byte i = 0; i < _devices.Length; i++)
        {
            RGBControl.SetControlDevice(i);
            RGBControl.SetKey(row, column, red, green, blue);
        }
    }

    public void UpdateKeyboard()
    {
        if (_devices == null)
            return;

        for (byte i = 0; i < _devices.Length; i++)
        {
            RGBControl.SetControlDevice(i);
            RGBControl.UpdateKeyboard();
        }
    }

    public void Dispose()
    {
        RGBControl.Close();
    }
}