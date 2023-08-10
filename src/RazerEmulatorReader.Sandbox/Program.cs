using System;
using System.Collections.Generic;
using System.Threading;

namespace RazerEmulatorReader.Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        var mutexes = InitSequence();

        Run();

        foreach (var mutex in mutexes)
            mutex.ReleaseMutex();
    }
    
     private static void Run()
    {
        var kbReader = new SignaledReader<CChromaKeyboard>(Constants.CChromaKeyboardFileMapping, Constants.CChromaKeyboardEvent);
        kbReader.OnRead += (sender, keyboard) =>
        {
            int zoneId = 0;
            var snapshot = keyboard.Data[keyboard.WriteIndex];
            if (snapshot.EffectType is not (EffectType.Static or EffectType.CustomKey or EffectType.Custom))
            {
                Console.WriteLine($"Effect type: {snapshot.EffectType}, skipping...");
                return;
            }

            if (snapshot.EffectType is EffectType.Static or EffectType.Custom)
            {
                var clr = snapshot.Effect.Custom.Color[zoneId];
                Console.WriteLine($"Custom: {clr}");
                return;
            }

            if (snapshot.EffectType is EffectType.CustomKey)
            {
                var s = snapshot.Effect.Static.Color;
                var clr = snapshot.Effect.Custom3.Key[zoneId];

                if (clr.A == s.A && clr.R == s.R && clr.G == s.G && clr.B == s.B)
                    clr = snapshot.Effect.Custom3.Color[zoneId];

                //Note: yes, this works. No, I don't know why.
                //Somehow Razer's 'encryption' is just XORing the colors.
                var r = clr.A ^ s.A;
                var g = clr.R ^ s.R;
                var b = clr.G ^ s.G;
                var a = clr.B ^ s.B;
                var xor = CChromaColor.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
                Console.WriteLine($"Custom key: {xor} = {clr} ^ {s}");
            }
        };
        kbReader.Start();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        
        kbReader.Dispose();
    }

    public static IEnumerable<Mutex> InitSequence()
    {
        var mutexes = new Mutex[4];
        mutexes[0] = new Mutex(true, Constants.SynapseOnlineMutex);
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        mutexes[1] = new Mutex(true, Constants.SynapseOnlineHoldMutex);
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        mutexes[2] = new Mutex(true, Constants.HoldMutexSynapseVersion);
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        mutexes[3] = new Mutex(true, Constants.ChromaEmulatorMutex);
        return mutexes;
    }
}