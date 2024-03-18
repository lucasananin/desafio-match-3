using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Views
{
    public class TileDestroyVfx : MonoBehaviour
    {
        [SerializeField] RectTransform _rectTransform = null;
        [SerializeField] float _timeToDestroy = 0.5f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_timeToDestroy);

            Destroy(gameObject);
        }

        public void SetSize(float _width, float _height)
        {
            _rectTransform.sizeDelta = new Vector2(_width, _height);
        }
    }
}
