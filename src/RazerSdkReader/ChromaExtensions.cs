using System;
using System.Diagnostics;
using RazerSdkReader.Enums;
using RazerSdkReader.Structures;

namespace RazerSdkReader;

public static class ChromaExtensions
{
    private static int GetWriteIndex(int writeIndex) => writeIndex switch
    {
        < 0 or > 9 => throw new ArgumentOutOfRangeException(nameof(writeIndex)),
        0 => 9,
        _ => writeIndex - 1
    };

    public static ChromaColor GetColor(this ChromaKeyboard data, int index)
    {
        if (index is < 0 or >= Color6X22.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = GetWriteIndex(data.WriteIndex);
        
        var snapshot = data.Data[targetIndex];
        
        if (snapshot.EffectType is not EffectType.Custom and not EffectType.CustomKey and not EffectType.Static)
            return default;

        ChromaColor clr = default;
        var staticColor = snapshot.Effect.Static.Color;

        if (snapshot.EffectType == EffectType.CustomKey)
        {
            clr = snapshot.Effect.Custom2.Key[index];
            
            //this next part is required for some effects to work properly.
            //For example, the chroma example app ambient effect.
            if (clr == staticColor)
                clr = snapshot.Effect.Custom2.Color[index];
        }
        else if (snapshot.EffectType is EffectType.Custom or EffectType.Static)
        {
            clr = snapshot.Effect.Custom.Color[index];
        }
        
        return clr ^ staticColor;
    }
    
    public static ChromaColor GetColor(this ChromaMouse data, int index)
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
    
    public static ChromaColor GetColor(this ChromaMousepad data, int index)
    {
        if (index is < 0 or >= MousepadCustom2.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var clr =  snapshot.Effect.Custom2[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        return clr ^ staticColor;
    }

    public static ChromaColor GetColor(this ChromaHeadset data, int index)
    {
        if (index is < 0 or >= HeadsetCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var clr =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        return clr ^ staticColor;
    }
    
    public static ChromaColor GetColor(this ChromaKeypad data, int index)
    {
        if (index is < 0 or >= KeypadCustom.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var targetIndex = GetWriteIndex(data.WriteIndex);
        var snapshot = data.Data[targetIndex];
        
        var clr =  snapshot.Effect.Custom[index];
        var staticColor = snapshot.Effect.Static.Color;
        
        return clr ^ staticColor;
    }
    
    public static ChromaColor GetColor(this ChromaLink data, int index)
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