using Gazeus.DesafioMatch3.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Goal_Time", menuName = "Gameplay/Goals/Time Goal")]
    public class TimeGoalSO : AbstractLevelGoalSO
    {
        public override bool HasAchievedGoal(GoalManager _goalManager, LevelDataSO _levelDataSO)
        {
            return _goalManager.Timer <= 0;
        }
    }
}
