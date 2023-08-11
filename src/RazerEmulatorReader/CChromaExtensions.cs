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
        var clr = CChromaColor.FromArgb(0, 0, 0, 0);
        var s = snapshot.Effect.Static.Color;

        switch (snapshot.EffectType)
        {
            case EffectType.CustomKey:
            {
                clr = snapshot.Effect.Custom2.Key[index];
                
                if (clr == s)
                    clr = snapshot.Effect.Custom2.Color[index];

                break;
            }
            case EffectType.Custom:
            case EffectType.Static:
            {
                clr = snapshot.Effect.Custom.Color[index];
                break;
            }
            default:
            {
                clr = default;
                break;
            }
        }
        
        //Note: yes, this works. No, I don't know why.
        //Somehow Razer's 'encryption' is just XORing the colors.
        var r = clr.A ^ s.A;
        var g = clr.R ^ s.R;
        var b = clr.G ^ s.G;
        var a = clr.B ^ s.B;
        var xor = CChromaColor.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        return xor;
    }
}