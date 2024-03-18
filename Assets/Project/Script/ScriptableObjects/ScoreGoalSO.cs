using Gazeus.DesafioMatch3.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Goal_Score", menuName = "Gameplay/Goals/Score Goal")]
    public class ScoreGoalSO : AbstractLevelGoalSO
    {
        public override bool HasAchievedGoal(GoalManager _goalManager, LevelDataSO _levelDataSO)
        {
            return _goalManager.LevelDataSO.TotalScore > _levelDataSO.MaxScore;
        }
    }
}
