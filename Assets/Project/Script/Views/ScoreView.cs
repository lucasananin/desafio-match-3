using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Views
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText = null;
        [SerializeField] TextMeshProUGUI _scoreMultiplierText = null;

        [Header("// Tween")]
        [SerializeField] Transform _container = null;
        [SerializeField] float _punchMultiplier = 1;
        [SerializeField] float _duration = 0.2f;
        [SerializeField] int _vibrato = 10;
        [SerializeField] float _elasticity = 1;

        [Header("// Debug")]
        [SerializeField] float _timeEnabled = 3;
        [SerializeField] float _currentTimeEnabled = 0;
        [SerializeField] bool _playAnimation = false;

        private void Awake()
        {
            _scoreText.text = $"0";
            _scoreMultiplierText.enabled = false;
        }

        private void OnEnable()
        {
            BoardView.onTilesDestroyed += BoardView_onTilesDestroyed;
        }

        private void OnDisable()
        {
            BoardView.onTilesDestroyed -= BoardView_onTilesDestroyed;
        }

        private void Update()
        {
            if (_playAnimation)
            {
                _playAnimation = false;
                PlayPunchAnimation();
            }

            if (_currentTimeEnabled > 0)
            {
                _currentTimeEnabled -= Time.deltaTime;

                if (_currentTimeEnabled <= 0)
                {
                    _scoreMultiplierText.enabled = false;
                }
            }
        }

        private void BoardView_onTilesDestroyed(Models.BoardSequence _boardSequence)
        {
            if (_boardSequence.ScoreMultiplier > 1)
            {
                _currentTimeEnabled = _timeEnabled;
                _scoreMultiplierText.enabled = true;
            }

            _scoreText.text = $"{_boardSequence.Score}";
            _scoreMultiplierText.text = $"x{_boardSequence.ScoreMultiplier}";

            PlayPunchAnimation();
        }

        private void PlayPunchAnimation()
        {
            Vector3 _punch = Vector3.one * _punchMultiplier;
            _container.transform.DOPunchScale(_punch, _duration, _vibrato, _elasticity);
        }
    }
}
