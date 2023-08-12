using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RazerSdkReader.Enums;
using RazerSdkReader.Extensions;
using RazerSdkReader.Structures;

namespace RazerSdkReader.Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        var reader = new RazerSdkReader();
        reader.KeyboardUpdated += (sender, keyboard) =>
        {
            var clr = keyboard.GetColor(0);
            Console.WriteLine($"Keyboard Color: {clr}");
        };
        reader.MouseUpdated += (sender, mouse) =>
        {
            var clr = mouse.Data[(int)mouse.WriteIndex].Effect.Custom2[0];
            Console.WriteLine($"Mouse Color: {clr}");
        };
        reader.MousepadUpdated += (sender, mousepad) =>
        {
            var clr = mousepad.Data[(int)mousepad.WriteIndex].Effect.Custom2[0];
            Console.WriteLine($"Mousepad Color: {clr}");
        };
        reader.KeypadUpdated += (sender, keypad) =>
        {
            var clr = keypad.Data[(int)keypad.WriteIndex].Effect.Custom[0];
            Console.WriteLine($"Keypad Color: {clr}");
        };
        reader.HeadsetUpdated += (sender, headset) =>
        {
            var clr = headset.Data[(int)headset.WriteIndex].Effect.Custom[0];
            Console.WriteLine($"Headset Color: {clr}");
        };
        reader.ChromaLinkUpdated += (sender, chromaLink) =>
        {
            var clr = chromaLink.Data[(int)chromaLink.WriteIndex].Effect.Custom[0];
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