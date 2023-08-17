namespace RazerSdkReader;

public delegate void InStructEventHandler<T>(object? sender, in T data) where T : unmanaged;