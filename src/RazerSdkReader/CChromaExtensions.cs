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
        
        return clr ^ staticColor;
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
        return clr ^ staticColor;
    }
    
    public static CChromaColor GetColor(this CChromaMousepad data, int index)
    {
        if (index is < 0 or >= MousepadCustom2.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var clr =  snapshot.Effect.Custom2[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        return clr ^ staticColor;
    }

    public static CChromaColor GetColor(this CChromaHeadset data, int index)
    {
        if (index is < 0 or >= HeadsetCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var clr =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        return clr ^ staticColor;
    }
    
    public static CChromaColor GetColor(this CChromaKeypad data, int index)
    {
        if (index is < 0 or >= KeypadCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var clr =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        return clr ^ staticColor;
    }
    
    public static CChromaColor GetColor(this CChromaLink data, int index)
    {
        if (index is < 0 or >= LinkCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var clr =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        return clr ^ staticColor;
    }
}