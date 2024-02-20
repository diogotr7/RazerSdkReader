using RazerSdkReader.Structures;
using System.Diagnostics;
using System.Runtime.InteropServices;

var bytes = File.ReadAllBytes("keyboard.bin");
var _keyboard = MemoryMarshal.Read<ChromaKeyboard>(bytes);
var arr = new ChromaColor[_keyboard.Width * _keyboard.Height];
var sw = Stopwatch.StartNew();
while (sw.ElapsedMilliseconds < 10000)
{
    _keyboard.GetColors(arr);
}