using Gazeus.DesafioMatch3.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class Tests : MonoBehaviour
    {
        //[SerializeField] GameController _gameController = null;

        //private void Update()
        //{
        //    int _count = _gameController.GameEngine.BoardTiles.Count;

        //    for (int y = 0; y < _count; y++)
        //    {
        //        List<Models.Tile> _rows = _gameController.GameEngine.BoardTiles[y];

        //        for (int x = 0; x < _rows.Count; x++)
        //        {
        //            Models.Tile _tile = _gameController.GameEngine.BoardTiles[y][x];
        //            //Debug.Log($"// _tile: id = {_tile.Id}, type = {_tile.Type}, column = {_tile.Column}, row = {_tile.Row}");
        //            Debug.Log($"// _tile: id = {_tile.Id}, type = {_tile.Type}, pos = {_tile.GridPosition}");
        //        }
        //    }
        //}

        [SerializeField] float _timeScale = 1f;

        private void Update()
        {
            Time.timeScale = _timeScale;
        }
    }
}
