using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    public class AIComponent :IComponent
    {
        private List<IAction> actions = new List<IAction>();
        private List<IGoal> goals = new List<IGoal>();

        public IReadOnlyList<IAction> Actions 
        { 
            get { return actions; } 
        }
        public IReadOnlyList<IGoal> Goals 
        { 
            get { return goals; } 
        }

        public AIComponent AddAction(IAction action)
        {
            actions.Add(action);
            return this;
        }

        public AIComponent AddGoal(IGoal goal)
        {
            goals.Add(goal);
            return this;
        }
    }
}
