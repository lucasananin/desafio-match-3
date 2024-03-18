using Gazeus.DesafioMatch3.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class GoalManager : MonoBehaviour
    {
        [SerializeField] LevelDataSO _levelDataSO = null;
        [SerializeField] GameController _gameController = null;
        [SerializeField] float _maxTimer = 0;
        [SerializeField] float _timer = 0;
        [SerializeField] bool _isPlayerBusy = false;

        public static event Action onVictory = null;
        public static event Action onDefeat = null;

        public LevelDataSO LevelDataSO { get => _levelDataSO; private set => _levelDataSO = value; }
        public GameController GameController { get => _gameController; private set => _gameController = value; }
        public float Timer { get => _timer; private set => _timer = value; }

        private void Start()
        {
            _levelDataSO.ResetRuntimeValues();
            _maxTimer = _levelDataSO.MaxTimeInSeconds;
            _timer = _maxTimer;
        }

        private void OnEnable()
        {
            GameController.onIsBusyChanged += GameController_onIsBusyChanged;
        }

        private void OnDisable()
        {
            GameController.onIsBusyChanged -= GameController_onIsBusyChanged;
        }

        private void Update()
        {
            if (!_levelDataSO.IsRunning) return;

            _timer -= Time.deltaTime;

            if (_isPlayerBusy) return;

            if (_levelDataSO.HasAchievedVictoryGoal(this))
            {
                _levelDataSO.IsRunning = false;
                onVictory?.Invoke();
            }
            else if (_levelDataSO.HasAchievedDefeatGoal(this))
            {
                _levelDataSO.IsRunning = false;
                onDefeat?.Invoke();
            }
        }

        private void GameController_onIsBusyChanged(bool _isBusy)
        {
            _isPlayerBusy = _isBusy;
        }

        public float GetTimeNormalized()
        {
            return _timer / _maxTimer;
        }
    }
}
