using RazerSdkReader.Structures;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RazerSdkReader.Extensions;

public static class ChromaEncryption
{
    public const string Base64Key =
        "RYZWAjSGJ/bWFgJ19ie2N4b2BwKWNwKX" +
        "9lcnAvbmVtI3R/YH0jeG9gcCR/YCdlZH" +
        "AkeGVgLW9jdHAvZXRwL2ZgKX9lcnAjSG" +
        "J/bWFgJGVmeWNlY34gICdYZWR4ZWJwKW" +
        "R3I3AjfWFidHAsaWdoZHluZ2AiYWN1ZG" +
        "AvbmApbm0vZ1ViknVgIHVzeGluZ2AkeG" +
        "VgLGltaWRzcCR/YCd4YWRwIWAnYW1pbm" +
        "dgLGFgdH9gcCNhbmAkb24gJFhlYCJRan" +
        "VicCJMYWRlYCBSf2AhNzApY3AiZXlsZH" +
        "AneWR4YCFgIH9ndWJ2ZXxgIHJ/Y2Vjc3" +
        "9icCR/YCJ1fmAgdWJ2b2EiRXlsZHApbm" +
        "R/YC9lcnAkTkFMICR4ZWAiUWp1YnAgWG" +
        "9uZWAiMCZlYWR1cnVjcCFuYClsbGV9aW" +
        "5hZHVkYCJRanVicCxvZ29gJ3lkeGAjZX" +
        "N0f21panFibGVgIlFqdWJwI0hif21haZ" +
        "AiV0JALGlnaGR5bmduIClfZXJwLW9ia7" +
        "RYZWAiUWp1YnAjSGJ/bWFgI09ubmVjZH" +
        "VkYCRFZnljZWNwIFJ/Z2JxbWAneWxsYC" +
        "VuYWJsZWAgcWJ0fmVic3Akf2AlbmFibG" +
        "VgI0hif21hYCxpZ2hkeW5nYCR4Yn9ld2" +
        "hgIWAjeW5nbGVgI2xpY2tgL25gJHhlaW" +
        "JwI39mZHd6A=";
    
    public static readonly byte[] Key = Convert.FromBase64String(Base64Key);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ChromaColor Decrypt(in ChromaColor color, ulong timestamp)
    {
        var seed = (uint)(timestamp % 128);
        if (seed >= 124)
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
}