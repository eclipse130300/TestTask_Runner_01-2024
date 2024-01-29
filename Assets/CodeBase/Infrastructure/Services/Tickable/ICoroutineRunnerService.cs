using System.Collections;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Services
{
    public interface ICoroutineRunnerService : IService
    {
        public Coroutine StartCoroutine(IEnumerator coroutine);
    }
}