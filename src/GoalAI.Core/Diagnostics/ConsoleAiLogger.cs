using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Diagnostics
{
    public class ConsoleAiLogger : IAiLogger
    {
        public void GoalSelected(Entity entity, IGoal goal)
        {
            Console.WriteLine("AI "+entity.Name + " selected goal " + goal.Name);
        }

        public void ActionChosen(Entity entity, IAction action)
        {
            Console.WriteLine("AI " + entity.Name + " chose action " +  action.Name);
        }

        public void ActionStarted(Entity entity, IAction action)
        {
            Console.WriteLine("AI " + entity.Name + " started action " + action.Name);
        }



        public void ActionApplied(Entity entity, IAction action)
        {
            Console.WriteLine("AI " + entity.Name + " applied(finished) action " + action.Name );
        }

        public void PlanningFailed_NoGoalTest(Entity entity, IGoal goal)
        {
            Console.WriteLine("AI " + entity.Name + ": goal " + goal.Name + "has no goal test, cannot plan");
        }

        public void PlanningFailed_NoActions(Entity entity, IGoal goal)
        {
            Console.WriteLine("AI " + entity.Name + ": no planning actions available for goal " + goal.Name);
        }

        public void PlanningFailed_NoPlanFound(Entity entity, IGoal goal)
        {
            Console.WriteLine("AI " + entity.Name + ": planner could not find a plan for goal " + goal.Name);
        }
    }
}
