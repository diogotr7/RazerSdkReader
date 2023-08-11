using System;
using RazerEmulatorReader.Enums;
using RazerEmulatorReader.Structures;

namespace RazerEmulatorReader;

public static class CChromaExtensions
{
    public static CChromaColor GetColor(this CChromaKeyboard keyboard, int index)
    {
        if (index is < 0 or >= Color6X22.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var snapshot = keyboard.Data[keyboard.WriteIndex];

        switch (snapshot.EffectType)
        {
            case EffectType.CustomKey:
            {
                var s = snapshot.Effect.Static.Color;
                var clr = snapshot.Effect.Custom2.Key[index];
            
                if (clr.A == s.A && clr.R == s.R && clr.G == s.G && clr.B == s.B)
                    clr = snapshot.Effect.Custom2.Color[index];
            
                //Note: yes, this works. No, I don't know why.
                //Somehow Razer's 'encryption' is just XORing the colors.
                var r = clr.A ^ s.A;
                var g = clr.R ^ s.R;
                var b = clr.G ^ s.G;
                var a = clr.B ^ s.B;
                var xor = new CChromaColor((byte)a, (byte)r, (byte)g, (byte)b);
                return xor;
                break;
            }
            case EffectType.Custom:
            case EffectType.Static:
            {
                return snapshot.Effect.Custom.Color[index];
            }
            default:
            {
                return default;
            }
        }
    }
}