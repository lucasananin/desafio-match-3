using Gazeus.DesafioMatch3.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views
{
    public class StartGameView : MonoBehaviour
    {
        [SerializeField] LevelDataSO _levelDataSO = null;
        [SerializeField] UiView _uiView = null;
        [SerializeField] Button _startButton = null;

        private void Start()
        {
            _uiView.Show();
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(CloseView);
        }

        private void CloseView()
        {
            _uiView.Hide();
            _levelDataSO.IsRunning = true;
        }
    }
}
