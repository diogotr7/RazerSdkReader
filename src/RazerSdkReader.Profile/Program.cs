using RazerSdkReader.Structures;
using System.Diagnostics;
using System.Runtime.InteropServices;

var bytes = File.ReadAllBytes("keyboard.bin");
var _keyboard = MemoryMarshal.Read<ChromaKeyboard>(bytes);

var sw = Stopwatch.StartNew();
while (true)
{
    ChromaColor x = default;
    for (var i = 0; i < _keyboard.Width; i++)
    {
        for (var j = 0; j < _keyboard.Height; j++)
        {
            var index = i + j * _keyboard.Width;
            x ^= _keyboard.GetColor(index);
        }
    }
    if (sw.ElapsedMilliseconds > 5000)
    {
        return;
    }
}