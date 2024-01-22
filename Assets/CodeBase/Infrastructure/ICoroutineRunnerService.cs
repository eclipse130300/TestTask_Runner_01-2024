using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public interface ICoroutineRunnerService : IService
    {
        public Coroutine StartCoroutine(IEnumerator coroutine);
    }
}