using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoad = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoad));

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