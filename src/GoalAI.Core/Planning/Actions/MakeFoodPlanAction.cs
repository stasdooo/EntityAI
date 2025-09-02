using GoalAI.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Planning.Actions
{

    /// <summary>
    /// Planning action that represents creating food (foraging).
    /// Always applicable, increases food by one, but has high cost
    /// so it is chosen only as a last resort.
    /// </summary>
    public class MakeFoodPlanAction : IPlanningAction
    {
        public string Name => "MakeFood(plan)";

        public IAction RuntimeAction {  get; }

        public MakeFoodPlanAction(MakeFoodAction runtime)
        {
            RuntimeAction = runtime;
        }

        public void ApplyEffects(PlanState s)
        {
            s.Set("food", s.Get("food")+1);
        }

        public bool CheckPreconditions(PlanState s)
        {
            return true;
        }

        public float Cost(PlanState s)
        {
            return 10;
        }
    }
}
