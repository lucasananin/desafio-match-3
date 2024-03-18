using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3
{
    public class TileSpriteAnimator : MonoBehaviour
    {
        [SerializeField] Image _image = null;
        [SerializeField] float _changeTimer = 1;
        [SerializeField] Sprite[] _sprites = null;

        private WaitForSeconds _waitForSeconds = null;
        private int _index = 0;

        private IEnumerator Start()
        {
            _index = Random.Range(0, _sprites.Length);
            _image.sprite = _sprites[_index];
            _waitForSeconds = new WaitForSeconds(_changeTimer);

            while (true)
            {
                yield return _waitForSeconds;

                _index++;

                if (_index >= _sprites.Length)
                {
                    _index = 0;
                }

                _image.sprite = _sprites[_index];
            }
        }

        private void OnValidate()
        {
            _image = GetComponent<Image>();
        }
    }
}
