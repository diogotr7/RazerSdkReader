namespace RazerSdkReader.Generators;

public class StructInfo
{
    public string Namespace { get; set; }
    public string ParentStruct { get; set; }
    public string ChildStruct { get; set; }
    public int Count { get; set; }
    public bool IsRecordStruct { get; set; }
    public bool IsReadOnly { get; set; }
}