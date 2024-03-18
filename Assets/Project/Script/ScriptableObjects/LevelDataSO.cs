using Gazeus.DesafioMatch3.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData_", menuName = "Gameplay/LevelData_")]
    public class LevelDataSO : ScriptableObject
    {
        [SerializeField] bool _isRunning = false;

        [Header("// Tiles")]
        [SerializeField, Range(3, 6)] int _tileTypeCount = 4;

        [Header("// Goals")]
        [SerializeField] List<AbstractLevelGoalSO> _victoryGoals = null;
        [SerializeField] List<AbstractLevelGoalSO> _defeatGoals = null;

        [Header("// Score")]
        [SerializeField] int _maxScore = 1000;
        [SerializeField] int _totalScore = 0;
        [SerializeField] int _totalScoreMultiplier = 0;

        [Header("// Time")]
        [SerializeField] int _maxTimeInSeconds = 180;

        [Header("// Deadzones")]
        [SerializeField] List<Vector2Int> _deadzoneInitialPositions = null;

        public bool IsRunning { get => _isRunning; set => _isRunning = value; }
        public int MaxScore { get => _maxScore; private set => _maxScore = value; }
        public int TotalScore { get => _totalScore; set => _totalScore = value; }
        public int TotalScoreMultiplier { get => _totalScoreMultiplier; set => _totalScoreMultiplier = value; }
        public int MaxTimeInSeconds { get => _maxTimeInSeconds; private set => _maxTimeInSeconds = value; }
        public List<Vector2Int> DeadzoneInitialPositions { get => _deadzoneInitialPositions; private set => _deadzoneInitialPositions = value; }
        public int TileTypeCount { get => _tileTypeCount; private set => _tileTypeCount = value; }

        public bool HasAchievedVictoryGoal(GoalManager _goalManager)
        {
            if (_victoryGoals.Count == 0)
            {
                return false;
            }

            int _count = _victoryGoals.Count;
            int _goalsAchieved = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_victoryGoals[i].HasAchievedGoal(_goalManager, this))
                {
                    _goalsAchieved++;
                }
            }

            return _goalsAchieved >= _victoryGoals.Count;
        }

        public bool HasAchievedDefeatGoal(GoalManager _goalManager)
        {
            if (_defeatGoals.Count == 0)
            {
                return false;
            }

            int _count = _defeatGoals.Count;
            int _goalsAchieved = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_defeatGoals[i].HasAchievedGoal(_goalManager, this))
                {
                    _goalsAchieved++;
                }
            }

            return _goalsAchieved >= _defeatGoals.Count;
        }

        public bool HasTimer()
        {
            return _maxTimeInSeconds > 0;
        }

        public void ResetRuntimeValues()
        {
            _isRunning = false;
            _totalScore = 0;
            _totalScoreMultiplier = 0;
        }
    }
}
