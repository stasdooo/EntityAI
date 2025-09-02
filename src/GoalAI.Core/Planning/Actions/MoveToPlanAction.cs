using GoalAI.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Planning.Actions
{
    /// <summary>
    /// Planning action that moves the agent towards a target.
    /// Preconditions require the agent not being at the target,
    /// effects mark it as reached, and cost depends on distance.
    /// </summary>
    public class MoveToPlanAction : IPlanningAction
    {
        public string Name => "MoveTo(plan)";
        private float speed;
        public IAction RuntimeAction {  get; }

        public MoveToPlanAction(MoveToAction runtime, float speed = 2)
        {
            RuntimeAction = runtime;
            this.speed = speed;
        }

        public void ApplyEffects(PlanState s)
        {
            s.Set("at_target", 1);
            s.Set("distance", 0);
        }

        public bool CheckPreconditions(PlanState s)
        {
            return s.Get("at_target") == 0 && s.Get("distance") > 0;
        }

        public float Cost(PlanState s)
        {
            return s.Get("distance"); // pouzit rychlost
        }
    }
}
