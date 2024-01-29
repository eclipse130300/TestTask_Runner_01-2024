using System;
using System.Collections;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunnerService _coroutineRunnerService;

        public SceneLoader(ICoroutineRunnerService coroutineRunnerService)
        {
            _coroutineRunnerService = coroutineRunnerService;
        }

        public void Load(string name, Action onLoad = null) =>
            _coroutineRunnerService.StartCoroutine(LoadScene(name, onLoad));

        private IEnumerator LoadScene(string nextScene, Action onLoad = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoad?.Invoke();
                yield break;
            }
            
            AsyncOperation waitForScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitForScene.isDone)
                yield return null;

            onLoad?.Invoke();
        }
    }
}