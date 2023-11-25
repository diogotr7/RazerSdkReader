using RazerSdkReader.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RazerSdkReader;

internal sealed class ChromaMutex : IDisposable
{
    private readonly Queue<Mutex> _mutexes = new();

    public ChromaMutex()
    {
        _mutexes.Enqueue(MutexHelper.CreateMutex(Constants.SynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(MutexHelper.CreateMutex(Constants.OldSynapseOnlineMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(MutexHelper.CreateMutex(Constants.OldSynapseVersionMutex));
        EventWaitHandleHelper.Pulse(Constants.SynapseEvent);
        _mutexes.Enqueue(MutexHelper.CreateMutex(Constants.ChromaEmulatorMutex));
    }

    public void Dispose()
    {
        while (_mutexes.TryDequeue(out var mutex))
        {
            mutex.ReleaseMutex();
            mutex.Dispose();
        }
    }
}