using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UiView : MonoBehaviour
    {
        private CanvasGroup _canvasGroup = null;

        private void OnValidate()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
