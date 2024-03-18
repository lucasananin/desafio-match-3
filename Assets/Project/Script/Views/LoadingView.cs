using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gazeus.DesafioMatch3.Controllers;

namespace Gazeus.DesafioMatch3.Views
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup = null;

        private void Awake()
        {
            Hide();
        }

        private void OnEnable()
        {
            SceneController.onStartLoadingScene += Show;
            SceneController.onFinishLoadingScene += Hide;
        }

        private void OnDisable()
        {
            SceneController.onStartLoadingScene -= Show;
            SceneController.onFinishLoadingScene -= Hide;
        }

        private void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
    }
}
