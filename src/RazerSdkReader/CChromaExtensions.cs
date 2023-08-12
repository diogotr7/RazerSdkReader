using System;
using RazerSdkReader.Enums;
using RazerSdkReader.Structures;

namespace RazerSdkReader;

public static class CChromaExtensions
{
    private static int GetWriteIndex(int writeIndex) => writeIndex switch
    {
        0 => 9,
        _ => writeIndex - 1
    };
    
    public static CChromaColor GetColor(this CChromaKeyboard data, int index)
    {
        if (index is < 0 or >= Color6X22.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = GetWriteIndex(data.WriteIndex);
        
        var snapshot = data.Data[targetIndex];
        
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
    
    public static CChromaColor GetColor(this CChromaMouse data, int index)
    {
        if (index is < 0 or >= MouseCustom2.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = GetWriteIndex(data.WriteIndex);
        
        var snapshot = data.Data[targetIndex];
        
        if (snapshot.EffectType is not EffectType.Custom and not EffectType.CustomKey and not EffectType.Static)
            return default;
        
        var staticColor = snapshot.Effect.Static.Color;
        var clr = snapshot.Effect.Custom2[index];

        //Note: yes, this works. No, I don't know why.
        //Somehow Razer's 'encryption' is just XORing the colors.
        var r = clr.A ^ staticColor.A;
        var g = clr.R ^ staticColor.R;
        var b = clr.G ^ staticColor.G;
        var a = clr.B ^ staticColor.B;
        var xor = CChromaColor.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        return xor;
    }
    
    public static CChromaColor GetColor(this CChromaMousepad data, int index)
    {
        if (index is < 0 or >= MousepadCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var custom =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        var a = custom.A ^ staticColor.A;
        var r = custom.R ^ staticColor.R;
        var g = custom.G ^ staticColor.G;
        var b = custom.B ^ staticColor.B;
        var xor = CChromaColor.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        return xor;
    }

    public static CChromaColor GetColor(this CChromaHeadset data, int index)
    {
        if (index is < 0 or >= HeadsetCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var custom =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        var a = custom.A ^ staticColor.A;
        var r = custom.R ^ staticColor.R;
        var g = custom.G ^ staticColor.G;
        var b = custom.B ^ staticColor.B;
        var xor = CChromaColor.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        return xor;
    }
    
    public static CChromaColor GetColor(this CChromaKeypad data, int index)
    {
        if (index is < 0 or >= KeypadCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var custom =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        var a = custom.A ^ staticColor.A;
        var r = custom.R ^ staticColor.R;
        var g = custom.G ^ staticColor.G;
        var b = custom.B ^ staticColor.B;
        var xor = CChromaColor.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        return xor;
    }
    
    public static CChromaColor GetColor(this CChromaLink data, int index)
    {
        if (index is < 0 or >= LinkCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var custom =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        var a = custom.A ^ staticColor.A;
        var r = custom.R ^ staticColor.R;
        var g = custom.G ^ staticColor.G;
        var b = custom.B ^ staticColor.B;
        var xor = CChromaColor.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        return xor;
    }
}