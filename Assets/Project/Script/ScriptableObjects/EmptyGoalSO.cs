using Gazeus.DesafioMatch3.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Goal_Empty", menuName = "Gameplay/Goals/Empty Goal")]
    public class EmptyGoalSO : AbstractLevelGoalSO
    {
        public override bool HasAchievedGoal(GoalManager _goalManager, LevelDataSO _levelDataSO)
        {
            return false;
        }
    }
}
