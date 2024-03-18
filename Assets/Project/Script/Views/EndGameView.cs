using Gazeus.DesafioMatch3.Controllers;
using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Views
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] UiView _uiView = null;
        [SerializeField] AudioDataSO _audioDataSO = null;
        [SerializeField] bool _isVictoryView = true;

        private void Start()
        {
            _uiView.Hide();
        }

        private void OnEnable()
        {
            if (_isVictoryView)
            {
                GoalManager.onVictory += ShowView;
            }
            else
            {
                GoalManager.onDefeat += ShowView;
            }
        }

        private void OnDisable()
        {
            if (_isVictoryView)
            {
                GoalManager.onVictory -= ShowView;
            }
            else
            {
                GoalManager.onDefeat -= ShowView;
            }
        }

        private void ShowView()
        {
            _audioDataSO.PlayAsSfx();
            _uiView.Show();
        }
    }
}
