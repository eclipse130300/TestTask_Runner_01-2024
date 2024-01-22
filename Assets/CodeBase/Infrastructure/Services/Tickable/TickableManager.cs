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

    public T CreateTickableAndRegister<T>() where T : ITickable, new()
    {
        T tickableObject = new T();
        Register(tickableObject);
        return tickableObject;
    }

    public void UnRegister(ITickable tickable)
    {
        if (tickables.Contains(tickable))
        {
            tickables.Remove(tickable);
        }
    }

    private void Register(ITickable tickable)
    {
        tickables.Add(tickable);    
    }
}