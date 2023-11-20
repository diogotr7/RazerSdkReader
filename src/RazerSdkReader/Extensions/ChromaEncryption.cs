using RazerSdkReader.Structures;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RazerSdkReader.Extensions;

public static class ChromaEncryption
{
    public const string Base64Key =
        "RYZWAjSGJ/bWFgJ19ie2N4b2BwKWNwKX9lcnAvbmVtI3R/YH0jeG9gcCR/YCdlZH" +
        "AkeGVgLW9jdHAvZXRwL2ZgKX9lcnAjSGJ/bWFgJGVmeWNlY34gICdYZWR4ZWJwKW" +
        "R3I3AjfWFidHAsaWdoZHluZ2AiYWN1ZGAvbmApbm0vZ1ViknVgIHVzeGluZ2AkeG" +
        "VgLGltaWRzcCR/YCd4YWRwIWAnYW1pbmdgLGFgdH9gcCNhbmAkb24gJFhlYCJRan" +
        "VicCJMYWRlYCBSf2AhNzApY3AiZXlsZHAneWR4YCFgIH9ndWJ2ZXxgIHJ/Y2Vjc3" +
        "9icCR/YCJ1fmAgdWJ2b2EiRXlsZHApbmR/YC9lcnAkTkFMICR4ZWAiUWp1YnAgWG" +
        "9uZWAiMCZlYWR1cnVjcCFuYClsbGV9aW5hZHVkYCJRanVicCxvZ29gJ3lkeGAjZX" +
        "N0f21panFibGVgIlFqdWJwI0hif21haZAiV0JALGlnaGR5bmduIClfZXJwLW9ia7" +
        "RYZWAiUWp1YnAjSGJ/bWFgI09ubmVjZHVkYCRFZnljZWNwIFJ/Z2JxbWAneWxsYC" +
        "VuYWJsZWAgcWJ0fmVic3Akf2AlbmFibGVgI0hif21hYCxpZ2hkeW5nYCR4Yn9ld2" +
        "hgIWAjeW5nbGVgI2xpY2tgL25gJHhlaWJwI39mZHd6A=";

    public static readonly byte[] Key = Convert.FromBase64String(Base64Key);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ChromaColor Decrypt(in ChromaColor color, in ChromaTimestamp timestamp)
    {
        var seed = timestamp.TickCount64 % 128;
        if (seed > 124)
        {
            seed -= 3;
        }
        
        var r = Key[0 * 128 + 0 + seed];
        var g = Key[1 * 128 + 1 + seed];
        var b = Key[2 * 128 + 2 + seed];
        var a = Key[3 * 128 + 3 + seed];
        
        r ^= color.R;
        g ^= color.G;
        b ^= color.B;
        a ^= color.A;

        return new ChromaColor(r, g, b, a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Decrypt(in ReadOnlySpan<ChromaColor> input, in Span<ChromaColor> output, in ChromaTimestamp timestamp)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(output.Length, input.Length);

        var seed = timestamp.TickCount64 % 128;
        if (seed > 124)
        {
            seed -= 3;
        }

        var rKey = Key[0UL + seed];
        var gKey = Key[129UL + seed];
        var bKey = Key[258UL + seed];
        var aKey = Key[387UL + seed];

        for (var i = 0; i < input.Length; i++)
        {
            ref readonly var color = ref input[i];
            
            var r = (byte)(color.R ^ rKey);
            var g = (byte)(color.G ^ gKey);
            var b = (byte)(color.B ^ bKey);
            var a = (byte)(color.A ^ aKey);

            output[i] = new ChromaColor(r, g, b, a);
        }
    }
}