using System.Collections.Generic;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Services
{
    /// <summary>
    /// dedicated game object that starts coroutines and runs Tick for every POCO ITickable 
    /// </summary>
    public class TickableService : MonoBehaviour, ICoroutineRunnerService
    {
        private List<ITickable> _tickables = new();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void UnRegister(ITickable tickable)
        {
            if (_tickables.Contains(tickable))
            {
                _tickables.Remove(tickable);
            }
        }

        public void Register(ITickable tickable)
        {
            _tickables.Add(tickable);
        }

        private void Update()
        {
            foreach (var tickable in _tickables)
            {
                tickable.Tick(Time.deltaTime);
            }
        }
    }
}