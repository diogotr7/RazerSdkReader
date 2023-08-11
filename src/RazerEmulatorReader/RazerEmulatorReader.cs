using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using RazerEmulatorReader.Extensions;

namespace RazerEmulatorReader;

[SupportedOSPlatform("windows")]
public sealed class RazerEmulatorReader : IDisposable
{
    private readonly Queue<Mutex> _mutexes = new();
    private ManualResetEventSlim? _initializationEvent;
    private Thread? _mutexThread;
    
    public void Start()
    {
        var isServiceRunning = Process.GetProcessesByName("RzSDKService").Any();
        
        if (!isServiceRunning)
            throw new InvalidOperationException("RzSdkService is not running.");
        
        _initializationEvent = new ManualResetEventSlim();
        _mutexThread = new Thread(Thread);
        _mutexThread.Start();
        SpinWait.SpinUntil(() => _mutexThread.IsAlive);
    }

    private void Thread()
    {
        _mutexes.Enqueue(new Mutex(true, Constants.SynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.OldSynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.OldSynapseVersionMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(new Mutex(true, Constants.ChromaEmulatorMutex));
        
        //init readers
        //todo

        _initializationEvent?.Wait();
        
        //dispose readers
        //todo

        while (_mutexes.TryDequeue(out var mutex))
        {
            mutex.ReleaseMutex();
            mutex.Dispose();
        }
    }
    
    //todo: handle gathering data from readers and firing an event or something
    
    public void Dispose()
    {
        _initializationEvent?.Set();
        _mutexThread?.Join();
        _initializationEvent?.Dispose();
    }
}