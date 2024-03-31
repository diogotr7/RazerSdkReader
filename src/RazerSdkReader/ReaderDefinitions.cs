namespace RazerSdkReader;

internal record ReaderDefinition(string MemoryMappedFileName, string EventWaitHandle);

internal static class ReaderDefinitions
{
    public static ReaderDefinition Keyboard => new(Constants.KeyboardFileName, Constants.KeyboardWaitHandle);
    public static ReaderDefinition Mouse => new(Constants.MouseFileName, Constants.MouseWaitHandle);
    public static ReaderDefinition Headset => new(Constants.HeadsetFileName, Constants.HeadsetWaitHandle);
    public static ReaderDefinition Mousepad => new(Constants.MousepadFileName, Constants.MousepadWaitHandle);
    public static ReaderDefinition Keypad => new(Constants.KeypadFileName, Constants.KeypadWaitHandle);
    public static ReaderDefinition Link => new(Constants.LinkFileName, Constants.LinkWaitHandle);
    public static ReaderDefinition AppData => new(Constants.AppDataFileName, Constants.AppDataWaitHandle);
    public static ReaderDefinition AppManager => new(Constants.AppManagerFileName, Constants.AppManagerWaitHandle);
}