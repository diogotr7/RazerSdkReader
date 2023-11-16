using RazerSdkReader.Structures;
using System;
using System.Diagnostics;
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
    public static ChromaColor Decrypt(in ChromaColor color, ulong timestamp)
    {
        var seed = timestamp % 128;
        if (seed > 124)
        {
            seed -= 3;
        }

        //0 * 128  + 0
        //1 * 128  + 1
        //2 * 128  + 2
        //3 * 128  + 3

        var r = (byte)(color.R ^ Key[0U + seed]);
        var g = (byte)(color.G ^ Key[129U + seed]);
        var b = (byte)(color.B ^ Key[258U + seed]);
        var a = (byte)(color.A ^ Key[387U + seed]);

        return new ChromaColor(r, g, b, a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Decrypt(in ReadOnlySpan<ChromaColor> input, in Span<ChromaColor> output, ulong timestamp)
    {
        Debug.Assert(input.Length == output.Length);

        var seed = timestamp % 128;
        if (seed > 124)
        {
            seed -= 3;
        }

        for (var i = 0; i < input.Length; i++)
        {
            var r = (byte)(input[i].R ^ Key[0U + seed]);
            var g = (byte)(input[i].G ^ Key[129U + seed]);
            var b = (byte)(input[i].B ^ Key[258U + seed]);
            var a = (byte)(input[i].A ^ Key[387U + seed]);

            output[i] = new ChromaColor(r, g, b, a);
        }
    }
}