using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using UnityEngine;

public class TickableManager : MonoBehaviour, ICoroutineRunnerService
{
    private List<ITickable> tickables = new ();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void UnRegister(ITickable tickable)
    {
        if (tickables.Contains(tickable))
        {
            tickables.Remove(tickable);
        }
    }

    public void Register(ITickable tickable)
    {
        tickables.Add(tickable);    
    }

    private void Update()
    {
        foreach (var tickable in tickables)
        {
            tickable.Tick(Time.deltaTime);
        }
    }
}