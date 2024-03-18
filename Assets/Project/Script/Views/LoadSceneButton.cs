using Gazeus.DesafioMatch3.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] Button _button = null;
        [SerializeField] string _sceneName = null;
        [SerializeField] AudioDataSO _audioDataSO = null;

        public static event Action<string> onLoadButtonSceneClicked = null;

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(InvokeEvent);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void InvokeEvent()
        {
            _audioDataSO.PlayAsSfx();
            onLoadButtonSceneClicked?.Invoke(_sceneName);
        }
    }
}
