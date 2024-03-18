using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using Gazeus.DesafioMatch3.ScriptableObjects;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core
{
    public class GameService
    {
        private List<List<Tile>> _boardTiles;
        private List<int> _tilesTypes = new List<int>();
        private int _tileCount;
        private LevelDataSO _levelDataSO = null;

        private int _height = 0;
        private int _width = 0;

        public const int NORMAL_MATCH_COUNT = 3;
        public const int BOMB_MATCH_COUNT = 5;
        public const int STRIPE_MATCH_COUNT = 4;
        public const int WRAPPER_MATCH_COUNT = 3;

        public List<List<Tile>> StartGame(int boardWidth, int boardHeight, LevelDataSO _levelDataValue)
        {
            _width = boardWidth;
            _height = boardHeight;
            _levelDataSO = _levelDataValue;

            for (int i = 0; i < _levelDataSO.TileTypeCount; i++)
            {
                _tilesTypes.Add(i);
            }

            _boardTiles = CreateBoard(boardWidth, boardHeight, _tilesTypes);

            return _boardTiles;
        }

        public List<BoardSequence> SwapTile(int fromX, int fromY, int toX, int toY)
        {
            if (_boardTiles[fromY][fromX].IsDeadZoneType() || _boardTiles[toY][toX].IsDeadZoneType())
            {
                return new List<BoardSequence>();
            }

            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);
            (newBoard[toY][toX].GridPosition, newBoard[fromY][fromX].GridPosition) = (newBoard[fromY][fromX].GridPosition, newBoard[toY][toX].GridPosition);

            List<BoardSequence> boardSequences = new();
            List<List<MatchInfo>> matchedTiles = FindMatches(newBoard);
            MatchGroupInfo _matchGroupInfo = FindSpecialMatchGroups(matchedTiles, newBoard);
            ActivateSpecialMatches(matchedTiles, newBoard);

            while (HasMatch(matchedTiles))
            {
                //Cleaning the matched tiles
                List<Vector2Int> matchedPosition = new();
                for (int y = 0; y < newBoard.Count; y++)
                {
                    for (int x = 0; x < newBoard[y].Count; x++)
                    {
                        if (matchedTiles[y][x].IsMatch)
                        {
                            matchedPosition.Add(new Vector2Int(x, y));
                            newBoard[y][x] = new Tile { Id = -1, Type = -1 };
                        }
                    }
                }

                // Dropping the tiles
                Dictionary<int, MovedTileInfo> movedTiles = new();
                List<MovedTileInfo> movedTilesList = new();
                for (int i = 0; i < matchedPosition.Count; i++)
                {
                    int x = matchedPosition[i].x;
                    int y = matchedPosition[i].y;
                    if (y > 0)
                    {
                        for (int j = y; j > 0; j--)
                        {
                            Tile movedTile = newBoard[j - 1][x];
                            newBoard[j][x] = movedTile;
                            if (movedTile.Type > -1)
                            {
                                if (movedTiles.ContainsKey(movedTile.Id))
                                {
                                    movedTiles[movedTile.Id].To = new Vector2Int(x, j);
                                }
                                else
                                {
                                    MovedTileInfo movedTileInfo = new()
                                    {
                                        From = new Vector2Int(x, j - 1),
                                        To = new Vector2Int(x, j)
                                    };
                                    movedTiles.Add(movedTile.Id, movedTileInfo);
                                    movedTilesList.Add(movedTileInfo);
                                }
                            }
                        }

                        newBoard[0][x] = new Tile
                        {
                            Id = -1,
                            Type = -1
                        };
                    }
                }

                // Filling the board
                List<AddedTileInfo> addedTiles = new();
                for (int y = newBoard.Count - 1; y > -1; y--)
                {
                    for (int x = newBoard[y].Count - 1; x > -1; x--)
                    {
                        if (newBoard[y][x].Type == -1)
                        {
                            int tileType = Random.Range(0, _tilesTypes.Count);
                            Tile tile = newBoard[y][x];
                            tile.Id = _tileCount++;
                            tile.Type = _tilesTypes[tileType];
                            tile.GridPosition = new Vector2Int(x, y);

                            addedTiles.Add(new AddedTileInfo
                            {
                                Position = new Vector2Int(x, y),
                                Type = tile.Type
                            });
                        }
                    }
                }

                int _score = matchedPosition.Count;
                int _scoreMultiplier = _matchGroupInfo.MatchGroups.Count;
                _scoreMultiplier = (int)Mathf.Clamp(_scoreMultiplier, 1, float.MaxValue);
                _levelDataSO.TotalScore += _score * _scoreMultiplier;
                _levelDataSO.TotalScoreMultiplier += _scoreMultiplier;

                BoardSequence sequence = new()
                {
                    MatchedPosition = matchedPosition,
                    MovedTiles = movedTilesList,
                    AddedTiles = addedTiles,
                    Score = _levelDataSO.TotalScore,
                    ScoreMultiplier = _levelDataSO.TotalScoreMultiplier,
                };

                boardSequences.Add(sequence);
                matchedTiles = FindMatches(newBoard);
                _matchGroupInfo = FindSpecialMatchGroups(matchedTiles, newBoard);
                ActivateSpecialMatches(matchedTiles, newBoard);
            }

            _boardTiles = newBoard;

            return boardSequences;
        }

        private static List<List<Tile>> CopyBoard(List<List<Tile>> boardToCopy)
        {
            List<List<Tile>> newBoard = new(boardToCopy.Count);
            for (int y = 0; y < boardToCopy.Count; y++)
            {
                newBoard.Add(new List<Tile>(boardToCopy[y].Count));
                for (int x = 0; x < boardToCopy[y].Count; x++)
                {
                    Tile tile = boardToCopy[y][x];
                    newBoard[y].Add(new Tile { Id = tile.Id, Type = tile.Type, GridPosition = new Vector2Int(x, y) });
                }
            }

            return newBoard;
        }

        private List<List<Tile>> CreateBoard(int width, int height, List<int> tileTypes)
        {
            List<List<Tile>> board = new(height);
            _tileCount = 0;

            for (int y = 0; y < height; y++)
            {
                board.Add(new List<Tile>(width));

                for (int x = 0; x < width; x++)
                {
                    board[y].Add(new Tile { Id = -1, Type = -1 });
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    List<int> noMatchTypes = new(tileTypes.Count);

                    for (int i = 0; i < tileTypes.Count; i++)
                    {
                        noMatchTypes.Add(_tilesTypes[i]);
                    }

                    if (x > 1 &&
                        board[y][x - 1].Type == board[y][x - 2].Type)
                    {
                        noMatchTypes.Remove(board[y][x - 1].Type);
                    }

                    if (y > 1 &&
                        board[y - 1][x].Type == board[y - 2][x].Type)
                    {
                        noMatchTypes.Remove(board[y - 1][x].Type);
                    }

                    board[y][x].Id = _tileCount++;
                    board[y][x].Type = noMatchTypes[Random.Range(0, noMatchTypes.Count)];
                    board[y][x].GridPosition = new Vector2Int(x, y);
                }
            }

            TryCreateDeadzones(board);

            return board;
        }

        private void TryCreateDeadzones(List<List<Tile>> board)
        {
            int _count = _levelDataSO.DeadzoneInitialPositions.Count;

            for (int i = 0; i < _count; i++)
            {
                Vector2Int _deadzonePosition = _levelDataSO.DeadzoneInitialPositions[i];
                board[_deadzonePosition.y][_deadzonePosition.x].SetToDeadzone();
            }
        }

        private void TryGetNeighbourTiles(List<List<Tile>> newBoard, int _x, int _y, out List<Tile> _xTiles, out List<Tile> _yTiles)
        {
            Tile _currentTile = newBoard[_y][_x];
            _xTiles = new();
            _yTiles = new();
            _xTiles.Add(_currentTile);
            _yTiles.Add(_currentTile);

            // Check - Left.
            for (int i = 1; i < _width; i++)
            {
                if (!IsValidGridPosition(newBoard, _x - i, _y)) break;
                if (!_currentTile.CanMatch(newBoard[_y][_x - i])) break;

                _xTiles.Add(newBoard[_y][_x - i]);
            }

            // Check - Right.
            for (int i = 1; i < _width; i++)
            {
                if (!IsValidGridPosition(newBoard, _x + i, _y)) break;
                if (!_currentTile.CanMatch(newBoard[_y][_x + i])) break;

                _xTiles.Add(newBoard[_y][_x + i]);
            }

            // Check - Top.
            for (int i = 1; i < _height; i++)
            {
                if (!IsValidGridPosition(newBoard, _x, _y - i)) break;
                if (!_currentTile.CanMatch(newBoard[_y - i][_x])) break;

                _yTiles.Add(newBoard[_y - i][_x]);
            }

            // Check - Down.
            for (int i = 1; i < _height; i++)
            {
                if (!IsValidGridPosition(newBoard, _x, _y + i)) break;
                if (!_currentTile.CanMatch(newBoard[_y + i][_x])) break;

                _yTiles.Add(newBoard[_y + i][_x]);
            }
        }

        private List<List<MatchInfo>> FindMatches(List<List<Tile>> newBoard)
        {
            List<List<MatchInfo>> matchedTiles = new();

            for (int y = 0; y < newBoard.Count; y++)
            {
                matchedTiles.Add(new List<MatchInfo>(newBoard[y].Count));

                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    matchedTiles[y].Add(new());
                }
            }

            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    if (CanMatchRow(newBoard, x, y))
                    {
                        matchedTiles[y][x].Setup(newBoard[y][x], true);
                        matchedTiles[y][x - 1].Setup(newBoard[y][x - 1], true);
                        matchedTiles[y][x - 2].Setup(newBoard[y][x - 2], true);
                    }

                    if (CanMatchColumn(newBoard, x, y))
                    {
                        matchedTiles[y][x].Setup(newBoard[y][x], true);
                        matchedTiles[y - 1][x].Setup(newBoard[y - 1][x], true);
                        matchedTiles[y - 2][x].Setup(newBoard[y - 2][x], true);
                    }
                }
            }

            return matchedTiles;
        }

        private MatchGroupInfo FindSpecialMatchGroups(List<List<MatchInfo>> _matchInfos, List<List<Tile>> newBoard)
        {
            MatchGroupInfo _matchGroupInfo = new();

            for (int y = 0; y < _matchInfos.Count; y++)
            {
                for (int x = 0; x < _matchInfos[y].Count; x++)
                {
                    TryGetNeighbourTiles(newBoard, x, y, out List<Tile> _xTiles, out List<Tile> _yTiles);
                    MatchInfo _currentMatchInfo = _matchInfos[y][x];

                    if (_currentMatchInfo.IsMatch is false) continue;

                    if (_matchGroupInfo.Contains(_currentMatchInfo.Tile)) continue;

                    if (_xTiles.Count >= BOMB_MATCH_COUNT)
                    {
                        _currentMatchInfo.Tile.IsBomb = true;
                        _matchGroupInfo.AddGroup(_xTiles);
                        continue;
                    }
                    else if (_xTiles.Count == STRIPE_MATCH_COUNT)
                    {
                        _currentMatchInfo.Tile.IsStripe = true;
                        _currentMatchInfo.Tile.StripeX = false;
                        _matchGroupInfo.AddGroup(_xTiles);
                        continue;
                    }
                    else if (_xTiles.Count == WRAPPER_MATCH_COUNT && _yTiles.Count == WRAPPER_MATCH_COUNT)
                    {
                        for (int i = 0; i < _xTiles.Count; i++)
                        {
                            int _distance = Mathf.Abs(_xTiles[i].GridPosition.x - _currentMatchInfo.Tile.GridPosition.x);

                            if (_distance > 1)
                                _xTiles.RemoveAt(i);
                        }

                        for (int i = 0; i < _yTiles.Count; i++)
                        {
                            int _distance = Mathf.Abs(_yTiles[i].GridPosition.x - _currentMatchInfo.Tile.GridPosition.x);

                            if (_distance > 1)
                                _yTiles.RemoveAt(i);
                        }

                        _xTiles.AddRange(_yTiles);
                        _matchGroupInfo.AddGroup(_xTiles);
                        _currentMatchInfo.Tile.IsWrapper = true;
                        continue;
                    }

                    if (_yTiles.Count >= BOMB_MATCH_COUNT)
                    {
                        _currentMatchInfo.Tile.IsBomb = true;
                        _matchGroupInfo.AddGroup(_yTiles);
                        continue;
                    }
                    else if (_yTiles.Count == STRIPE_MATCH_COUNT)
                    {
                        _currentMatchInfo.Tile.IsStripe = true;
                        _currentMatchInfo.Tile.StripeX = true;
                        _matchGroupInfo.AddGroup(_yTiles);
                        continue;
                    }
                }
            }

            return _matchGroupInfo;
        }

        private void ActivateSpecialMatches(List<List<MatchInfo>> _matchInfos, List<List<Tile>> _board)
        {
            for (int y = 0; y < _matchInfos.Count; y++)
            {
                for (int x = 0; x < _matchInfos[y].Count; x++)
                {
                    var _matchInfoTile = _matchInfos[y][x].Tile;

                    if (_matchInfoTile == null) continue;
                    if (!_matchInfoTile.IsSpecial()) continue;

                    if (_matchInfoTile.IsBomb)
                    {
                        for (int y2 = 0; y2 < _matchInfos.Count; y2++)
                        {
                            for (int x2 = 0; x2 < _matchInfos[y2].Count; x2++)
                            {
                                if (_matchInfoTile.Type == _board[y2][x2].Type)
                                {
                                    _matchInfos[y2][x2].Setup(_board[y2][x2], true);
                                }
                            }
                        }
                    }
                    else if (_matchInfoTile.IsStripe)
                    {
                        bool _stripeX = _matchInfos[y][x].Tile.StripeX;
                        int _count = _stripeX ? _width : _height;

                        for (int i = 0; i < _count; i++)
                        {
                            if (_stripeX)
                                _matchInfos[y][i].Setup(_board[y][i], true);
                            else
                                _matchInfos[i][x].Setup(_board[i][x], true);
                        }
                    }
                    else if (_matchInfoTile.IsWrapper)
                    {
                        int _maxDistance = 2;

                        for (int y2 = -_maxDistance; y2 <= _maxDistance; y2++)
                        {
                            for (int x2 = -_maxDistance; x2 <= _maxDistance; x2++)
                            {
                                Vector2Int _offset = new Vector2Int(x, y);
                                Vector2Int _position = new Vector2Int(x2, y2) + _offset;

                                int _testDistance = Mathf.Abs(x2) + Mathf.Abs(y2);

                                if (_testDistance > _maxDistance)
                                {
                                    continue;
                                }

                                if (!IsValidGridPosition(_board, _position.x, _position.y)) continue;

                                _matchInfos[_position.y][_position.x].Setup(_board[_position.y][_position.x], true);
                            }
                        }
                    }
                }
            }
        }

        private static bool HasMatch(List<List<MatchInfo>> list)
        {
            for (int y = 0; y < list.Count; y++)
            {
                for (int x = 0; x < list[y].Count; x++)
                {
                    if (list[y][x].IsMatch)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int GetDeadzonesOnBottomCount()
        {
            int _tileCount = 0;

            for (int i = 0; i < _width; i++)
            {
                if (_boardTiles[_height - 1][i].IsDeadZoneType())
                {
                    _tileCount++;
                }
            }

            return _tileCount;
        }

        private bool IsValidGridPosition(List<List<Tile>> _board, int _x, int _y)
        {
            return _y >= 0
                && _x >= 0
                && _y < _board.Count
                && _x < _board[_y].Count;
        }

        private bool CanMatchRow(List<List<Tile>> newBoard, int x, int y)
        {
            return x > 1 && newBoard[y][x].CanMatch(newBoard[y][x - 1]) && newBoard[y][x - 1].CanMatch(newBoard[y][x - 2]);
        }

        private bool CanMatchColumn(List<List<Tile>> newBoard, int x, int y)
        {
            return y > 1 && newBoard[y][x].CanMatch(newBoard[y - 1][x]) && newBoard[y - 1][x].CanMatch(newBoard[y - 2][x]);
        }
    }
}
