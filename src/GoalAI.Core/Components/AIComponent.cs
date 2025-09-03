using GoalAI.Core.Planning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    /// <summary>
    /// Component that holds an entity's actions and goals
    /// </summary>
    public class AIComponent :IComponent
    {


        private List<IGoal> goals = new List<IGoal>();

        //planning actions used by the GOAP planner
        private List<IPlanningAction> plannings = new List<IPlanningAction>();


        public IReadOnlyList<IGoal> Goals 
        { 
            get { return goals; } 
        }

        public IReadOnlyList<IPlanningAction> PlanningActions
        {
            get { return plannings; }
        }

      
        public AIComponent AddGoal(IGoal goal)
        {
            goals.Add(goal);
            return this;
        }

        public AIComponent AddPlanningAction(IPlanningAction planningAction)
        {
            plannings.Add(planningAction);
            return this;
        }
    }
}
