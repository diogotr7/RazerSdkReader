using System;
using RazerSdkReader.Enums;
using RazerSdkReader.Structures;

namespace RazerSdkReader;

public static class CChromaExtensions
{
    public static CChromaColor GetColor(this CChromaKeyboard keyboard, int index)
    {
        if (index is < 0 or >= Color6X22.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = keyboard.WriteIndex switch
        {
            0 => 9,
            _ => keyboard.WriteIndex - 1
        };
        
        var snapshot = keyboard.Data[targetIndex];
        
        if (snapshot.EffectType is not EffectType.Custom and not EffectType.CustomKey and not EffectType.Static)
            return default;
        
        CChromaColor clr;
        var staticColor = snapshot.Effect.Static.Color;

        if (snapshot.EffectType == EffectType.CustomKey)
        {
            clr = snapshot.Effect.Custom2.Key[index];

            if (clr == staticColor)
                clr = snapshot.Effect.Custom2.Color[index];
        }
        else if (snapshot.EffectType is EffectType.Custom or EffectType.Static)
        {
            clr = snapshot.Effect.Custom.Color[index];
        }
        else
        {
            clr = default;
        }

        //Note: yes, this works. No, I don't know why.
        //Somehow Razer's 'encryption' is just XORing the colors.
        var r = clr.A ^ staticColor.A;
        var g = clr.R ^ staticColor.R;
        var b = clr.G ^ staticColor.G;
        var a = clr.B ^ staticColor.B;
        var xor = CChromaColor.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        return xor;
    }
}