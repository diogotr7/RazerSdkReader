using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using RazerEmulatorReader.Extensions;

namespace RazerEmulatorReader;

[SupportedOSPlatform("windows")]
public sealed class RazerEmulatorReader : IDisposable
{
    private readonly Queue<Mutex> _mutexes = new();
    private SemaphoreSlim? _initializationSemaphore;
    private Thread? _mutexThread;
    
    public void Start()
    {
        _initializationSemaphore = new SemaphoreSlim(0);
        _mutexThread = new Thread(Thread);
        _mutexThread.Start();
        SpinWait.SpinUntil(() => _mutexThread.IsAlive);
    }

    private void Thread()
    {
        _mutexes.Enqueue(new Mutex(true, Constants.SynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.SynapseOnlineHoldMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.HoldMutexSynapseVersion));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.ChromaEmulatorMutex));
        
        //init readers
        //todo
        
        _initializationSemaphore?.Wait();
        
        //dispose readers
        //todo
        
        while (_mutexes.TryDequeue(out var mutex))
            mutex.ReleaseMutex();
    }
    
    //todo: handle gathering data from readers and firing an event or something
    
    public void Dispose()
    {
        _initializationSemaphore?.Release();
        _mutexThread?.Join();
        _initializationSemaphore?.Dispose();
    }
}