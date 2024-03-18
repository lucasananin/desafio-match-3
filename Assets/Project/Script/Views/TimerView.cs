using Gazeus.DesafioMatch3.Controllers;
using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] LevelDataSO _levelDataSO = null;
        [SerializeField] GoalManager _goalManager = null;
        [SerializeField] Canvas _panelCanvas = null;
        [SerializeField] Image _imageFill = null;

        private void Start()
        {
            _panelCanvas.enabled = _levelDataSO.HasTimer();
        }

        private void LateUpdate()
        {
            if (!_panelCanvas.enabled) return;

            _imageFill.fillAmount = _goalManager.GetTimeNormalized();
        }
    }
}
