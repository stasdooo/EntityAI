using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Planning
{
    /// <summary>
    /// Defines an abstract action used by the planner, with preconditions,
    /// effects, and a link to the corresponding runtime action.
    /// </summary>
    public interface IPlanningAction
    {
        string Name {  get; }

        IAction RuntimeAction { get; }

        // checks if the action can be applied in the given state
        bool CheckPreconditions(PlanState s);

        // applies the action's effects to the given state
        void ApplyEffects(PlanState s);

        // returns the cost of this action in the given state
        float Cost(PlanState s);

        
    }
}
