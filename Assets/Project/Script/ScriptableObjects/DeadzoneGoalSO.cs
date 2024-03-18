using Gazeus.DesafioMatch3.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Goal_DeadzoneOnBottom", menuName = "Gameplay/Goals/Deadzone On Bottom Goal")]
    public class DeadzoneGoalSO : AbstractLevelGoalSO
    {
        public override bool HasAchievedGoal(GoalManager _goalManager, LevelDataSO _levelDataSO)
        {
            Core.GameService _gameService = _goalManager.GameController.GameEngine;
            List<Vector2Int> _deadzonePositions = _goalManager.LevelDataSO.DeadzoneInitialPositions;
            return _gameService.GetDeadzonesOnBottomCount() >= _deadzonePositions.Count;
        }
    }
}
