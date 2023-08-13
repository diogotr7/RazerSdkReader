using System;

namespace RazerSdkReader.ConsoleApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var reader = new RazerSdkReader();
        reader.Exception += (sender, exception) =>
        {
            Console.WriteLine($"Exception: {exception}");
        };
        reader.KeyboardUpdated += (sender, keyboard) =>
        {
            var clr = keyboard.GetColor(0);
            Console.WriteLine($"Keyboard Color: {clr}");
        };
        reader.MouseUpdated += (sender, mouse) =>
        {
            var clr = mouse.GetColor(0);
            Console.WriteLine($"Mouse Color: {clr}");
        };
        reader.MousepadUpdated += (sender, mousepad) =>
        {
            var clr = mousepad.GetColor(0);
            Console.WriteLine($"Mousepad Color: {clr}");
        };
        reader.KeypadUpdated += (sender, keypad) =>
        {
            var clr = keypad.GetColor(0);
            Console.WriteLine($"Keypad Color: {clr}");
        };
        reader.HeadsetUpdated += (sender, headset) =>
        {
            var clr = headset.GetColor(0);
            Console.WriteLine($"Headset Color: {clr}");
        };
        reader.ChromaLinkUpdated += (sender, chromaLink) =>
        {
            var clr = chromaLink.GetColor(0);
            Console.WriteLine($"ChromaLink Color: {clr}");
        };
        reader.AppDataUpdated += (sender, appData) =>
        {
            var currentAppName = "";
            for (int i = 0; i < appData.AppCount; i++)
            {
                if (appData.CurrentAppId != appData.AppInfo[i].AppId) continue;
                
                currentAppName = appData.AppInfo[i].AppName;
                break;
            }
            Console.WriteLine($"AppData active: {currentAppName}");
        };
        reader.AppManagerUpdated += (sender, appManager) =>
        {
            Console.WriteLine($"AppManager updated");
        };
        
        
        reader.Start();
        Console.ReadLine();
        reader.Dispose();
        Console.WriteLine("Disposed reader successfully...");
    }
}