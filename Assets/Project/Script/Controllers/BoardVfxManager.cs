using Gazeus.DesafioMatch3.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class BoardVfxManager : MonoBehaviour
    {
        [SerializeField] Transform _parent = null;
        [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
        [SerializeField] TileDestroyVfx _prefab = null;

        private void OnEnable()
        {
            BoardView.onTileViewsDestroyed += BoardView_onTileViewsDestroyed;
        }

        private void BoardView_onTileViewsDestroyed(List<GameObject> _objectsDestroyed)
        {
            int _count = _objectsDestroyed.Count;

            for (int i = 0; i < _count; i++)
            {
                Transform _transform = _objectsDestroyed[i].transform;
                TileDestroyVfx _instance = Instantiate(_prefab, _transform.position, _transform.rotation, _parent);
                _instance.SetSize(_gridLayoutGroup.cellSize.x, _gridLayoutGroup.cellSize.y);
            }
        }
    }
}
