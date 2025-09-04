using GoalAI.Core;
using GoalAI.Core.Diagnostics;
using GoalAI.Core.Actions;
using UnityEngine;
using GoalAI.Core.Components;


namespace GaoalAi.Unity
{

    public class UnityAiLogger : IAiLogger
    {
        public void ActionApplied(Entity entity, IAction action)
        {
            Debug.Log("AI " + entity.Name + " finished action " + action.Name);
        }

        public void ActionChosen(Entity entity, IAction action)
        {

            switch(action)
            {
                case MoveToAction move:
                    {
                        var bb = entity.GetComponent<EntityBlackBoardComponent>()?.Data;
                        
                        if (bb != null && bb.TryGet<(float x,float y)>(move.targetKey, out var target))
                        {
                            Debug.Log("AI " + entity.Name + " chose " + move.Name + " -> (" + target.x + ", " + target.y + ")");
                        }
                        else
                        {
                            Debug.Log("AI " +entity.Name +" chose "+ move.Name+ " (no target set)");
                        }

                        break;

                    }
                default:
                    Debug.Log("AI " + entity.Name + " chose action " + action.Name);
                    break;


            }

        }

        public void ActionStarted(Entity entity, IAction action)
        {
            Debug.Log("AI " + entity.Name + " started action " + action.Name);
        }

        public void GoalSelected(Entity entity, IGoal goal)
        {
            Debug.Log("AI " + entity.Name + " selected goal " + goal.Name);
        }

        public void PlanningFailed_NoActions(Entity entity, IGoal goal)
        {
            Debug.Log("AI " + entity.Name + ": no planning actions available for goal " + goal.Name);
        }

        public void PlanningFailed_NoGoalTest(Entity entity, IGoal goal)
        {
            Debug.Log("AI " + entity.Name + ": goal " + goal.Name + "has no goal test, cannot plan");
        }

        public void PlanningFailed_NoPlanFound(Entity entity, IGoal goal)
        {
            Debug.Log("AI " + entity.Name + ": planner could not find a plan for goal " + goal.Name);
        }
    }

}

