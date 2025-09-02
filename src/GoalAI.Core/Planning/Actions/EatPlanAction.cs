using GoalAI.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Planning.Actions
{
    /// <summary>
    /// Planning action that models eating: requires food and sufficient hunger,
    /// reduces hunger by a fixed amount, and consumes one food unit.
    /// </summary>
    public class EatPlanAction : IPlanningAction
    {
        public string Name => "Eat(plan)";
        private float threshold { get; set; } //

        public IAction RuntimeAction { get; }

        public EatPlanAction(EatAction runtime, float threshold=30f)
        {
            RuntimeAction = runtime;
            this.threshold = threshold;
        }

        public bool CheckPreconditions(PlanState s)
        {
            return s.Get("food") > 0 && s.Get("hunger") >= threshold;
        }

        public void ApplyEffects(PlanState s)
        {
            var newFood = s.Get("food") - 1;
            var newHung = s.Get("hunger") - 25;
            if (newHung < 0) newHung = 0;
            s.Set("food", newFood);
            s.Set("hunger", newHung);
        }

       

        public float Cost(PlanState s)
        {
            return 1;
        }
    }
}
