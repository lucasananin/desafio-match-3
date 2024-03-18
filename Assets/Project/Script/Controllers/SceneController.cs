using Gazeus.DesafioMatch3.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] string _firstSceneName = null;

        private WaitForSeconds _offsetWaitTime = null;
        private string _currentScene = null;

        public static event Action onStartLoadingScene = null;
        public static event Action onFinishLoadingScene = null;

        private void Awake()
        {
            _offsetWaitTime = new WaitForSeconds(0.2f);
        }

        private void Start()
        {
            LoadScene(_firstSceneName);
        }

        private void OnEnable()
        {
            LoadSceneButton.onLoadButtonSceneClicked += LoadScene;
        }

        private void OnDisable()
        {
            LoadSceneButton.onLoadButtonSceneClicked -= LoadScene;
        }

        private void LoadScene(string _sceneName)
        {
            StartCoroutine(LoadScene_routine(_sceneName));
        }

        private IEnumerator LoadScene_routine(string _sceneName)
        {
            onStartLoadingScene?.Invoke();

            yield return _offsetWaitTime;

            if (!string.IsNullOrEmpty(_currentScene))
            {
                AsyncOperation _asyncUnload = SceneManager.UnloadSceneAsync(_currentScene);

                while (!_asyncUnload.isDone)
                {
                    yield return null;
                }
            }

            AsyncOperation _asyncLoad = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);

            while (!_asyncLoad.isDone)
            {
                yield return null;
            }

            yield return _offsetWaitTime;

            _currentScene = _sceneName;
            onFinishLoadingScene?.Invoke();
        }
    }
}
