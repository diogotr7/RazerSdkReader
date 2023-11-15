using RazerSdkReader.Structures;
using System.Diagnostics;
using System.Runtime.InteropServices;

var bytes = File.ReadAllBytes("keyboard.bin");
var _keyboard = MemoryMarshal.Read<ChromaKeyboard>(bytes);
var arr = new ChromaColor[_keyboard.Width * _keyboard.Height];
var sw = Stopwatch.StartNew();
while (sw.ElapsedMilliseconds < 10000)
{
    for (var i = 0; i < _keyboard.Width; i++)
    {
        for (var j = 0; j < _keyboard.Height; j++)
        {
            var index = i + (j * _keyboard.Width);
            arr[index] = _keyboard.GetColor(index);
        }
    }
}