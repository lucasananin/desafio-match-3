using Gazeus.DesafioMatch3.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    public abstract class AbstractLevelGoalSO : ScriptableObject
    {
        public abstract bool HasAchievedGoal(GoalManager _goalManager, LevelDataSO _levelDataSO);
    }
}
