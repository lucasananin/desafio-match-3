using System;
using System.Collections.Generic;
using DG.Tweening;
using Gazeus.DesafioMatch3.Core;
using Gazeus.DesafioMatch3.Models;
using Gazeus.DesafioMatch3.ScriptableObjects;
using Gazeus.DesafioMatch3.Views;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BoardView _boardView;
        [SerializeField] private LevelDataSO _levelDataSO = null;
        [SerializeField, Range(4, 16)] private int _boardHeight = 10;
        [SerializeField, Range(4, 16)] private int _boardWidth = 10;

        private GameService _gameEngine;
        private bool _isAnimating;
        private int _selectedX = -1;
        private int _selectedY = -1;

        public static event Action<bool> onIsBusyChanged = null;
        public static event Action<Vector2Int> onTileSelected = null;
        public static event Action onTileDeselected = null;

        public GameService GameEngine { get => _gameEngine; private set => _gameEngine = value; }

        #region Unity
        private void Awake()
        {
            _gameEngine = new GameService();
            _boardView.TileClicked += OnTileClick;
        }

        private void OnDestroy()
        {
            _boardView.TileClicked -= OnTileClick;
        }

        private void Start()
        {
            List<List<Tile>> board = _gameEngine.StartGame(_boardWidth, _boardHeight, _levelDataSO);
            _boardView.CreateBoard(board);
        }

        private void OnValidate()
        {
            if (_boardHeight < _boardWidth)
            {
                _boardWidth = _boardHeight;
            }
        }
        #endregion

        private void AnimateBoard(List<BoardSequence> boardSequences, int index, Action onComplete)
        {
            BoardSequence boardSequence = boardSequences[index];

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_boardView.DestroyTiles(boardSequence));
            sequence.Append(_boardView.MoveTiles(boardSequence.MovedTiles));
            sequence.Append(_boardView.CreateTile(boardSequence.AddedTiles));

            index += 1;
            if (index < boardSequences.Count)
            {
                sequence.onComplete += () => AnimateBoard(boardSequences, index, onComplete);
            }
            else
            {
                sequence.onComplete += () => onComplete();
            }
        }

        private void OnTileClick(int x, int y)
        {
            if (_isAnimating) return;

            if (_selectedX > -1 && _selectedY > -1)
            {
                if (Mathf.Abs(_selectedX - x) + Mathf.Abs(_selectedY - y) > 1)
                {
                    _selectedX = -1;
                    _selectedY = -1;
                }
                else
                {
                    _isAnimating = true;
                    onIsBusyChanged?.Invoke(_isAnimating);

                    _boardView.SwapTiles(_selectedX, _selectedY, x, y).onComplete += () =>
                    {
                        List<BoardSequence> swapResult = _gameEngine.SwapTile(_selectedX, _selectedY, x, y);

                        if (swapResult.Count > 0)
                        {
                            AnimateBoard(swapResult, 0, () =>
                            {
                                _isAnimating = false;
                                _levelDataSO.TotalScoreMultiplier = 0;
                                onIsBusyChanged?.Invoke(_isAnimating);
                            });
                        }
                        else
                        {
                            _boardView.SwapTiles(x, y, _selectedX, _selectedY).onComplete += () => 
                            {
                                _isAnimating = false;
                                onIsBusyChanged?.Invoke(_isAnimating);
                            };
                        }

                        _selectedX = -1;
                        _selectedY = -1;
                    };
                }

                onTileDeselected?.Invoke();
            }
            else
            {
                _selectedX = x;
                _selectedY = y;
                onTileSelected?.Invoke(new Vector2Int(x, y));
            }
        }
    }
}
