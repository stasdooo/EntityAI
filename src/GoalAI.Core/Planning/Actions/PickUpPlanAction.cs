using GoalAI.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Planning.Actions
{

    /// <summary>
    /// Planning action for picking up food from a resource pile.
    /// Requires being at the target and at least one unit of food available,
    /// increases inventory food and decreases the world pile.
    /// </summary>
    public class PickUpPlanAction : IPlanningAction
    {
        public string Name => "PickUp(plan)";

        public IAction RuntimeAction { get; }

        public PickUpPlanAction(PickUpAction runtime)
        {
            RuntimeAction = runtime;
        }

        public void ApplyEffects(PlanState s)
        {
            s.Set("food", s.Get("food") + 1);
            s.Set("world_food_pile", s.Get("world_food_pile") - 1);
        }

        public bool CheckPreconditions(PlanState s)
        {
            return s.Get("at_target") == 1 && s.Get("world_food_pile") > 0;
        }

        public float Cost(PlanState s)
        {
            return 2f;
        }
    }
}
