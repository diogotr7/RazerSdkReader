using RazerSdkReader.Structures;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RazerSdkReader.Extensions;

public static class ChromaEncryption
{
    private const string Base64Key =
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

    private static readonly byte[] Key = Convert.FromBase64String(Base64Key);

    private static readonly uint[] PreProcessedKey = PreProcessKey();

    private static uint[] PreProcessKey()
    {
        // I do this processing so I can take advantage of SIMD instructions,
        // the data layout is much easier to work with.
        
        //I could preprocess it into a base64 string like the original key, but
        // I'm keeping it like this incase the key changes in the future, and so
        // i have a reference for how the data conversion works. It's also only
        // converted once per program run, so it's not a big deal :)
        var key = new uint[Key.Length / 4];

        for (var i = 0; i < key.Length; i++)
        {
            var correctedIndex = i;
            if (correctedIndex > 124)
            {
                correctedIndex -= 3;
            }

            var r = Key[0 * 128 + 0 + correctedIndex];
            var g = Key[1 * 128 + 1 + correctedIndex];
            var b = Key[2 * 128 + 2 + correctedIndex];
            var a = Key[3 * 128 + 3 + correctedIndex];

            key[i] = BitConverter.ToUInt32([r, g, b, a]);
        }
        
        return key;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ChromaColor Decrypt(ChromaColor color, ChromaTimestamp timestamp)
    {
        var key = PreProcessedKey[timestamp.TickCount64 % 128];

        var colorAsInt = Unsafe.As<ChromaColor, uint>(ref color);

        var xor = colorAsInt ^ key;

        return Unsafe.As<uint, ChromaColor>(ref xor);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Decrypt(ReadOnlySpan<ChromaColor> input, Span<ChromaColor> output, ChromaTimestamp timestamp)
    {
        var smallest = Math.Min(input.Length, output.Length);

        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(smallest, 0);

        var key = PreProcessedKey[timestamp.TickCount64 % 128];
        var inputAsInt = MemoryMarshal.Cast<ChromaColor, uint>(input);
        var outputAsInt = MemoryMarshal.Cast<ChromaColor, uint>(output);

        for (var i = 0; i < smallest; i++)
        {
            outputAsInt[i] = inputAsInt[i] ^ key;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DecryptSimd(ReadOnlySpan<ChromaColor> input, Span<ChromaColor> output, ChromaTimestamp timestamp)
    {
        var smallest = Math.Min(input.Length, output.Length);

        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(smallest, 0);

        var key = PreProcessedKey[timestamp.TickCount64 % 128];
        var inputAsInt = MemoryMarshal.Cast<ChromaColor, uint>(input);
        var outputAsInt = MemoryMarshal.Cast<ChromaColor, uint>(output);

        var remaining = smallest % Vector<uint>.Count;

        for (var i = 0; i < smallest - remaining; i += Vector<uint>.Count)
        {
            var v1 = new Vector<uint>(inputAsInt[i..]);
            var v2 = new Vector<uint>(key);
            (v1 ^ v2).CopyTo(outputAsInt[i..]);
        }
    }
}