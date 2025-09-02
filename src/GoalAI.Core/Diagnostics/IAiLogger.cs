using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Diagnostics
{
    /// <summary>
    /// Interface for logging AI decisions and action execution.
    /// </summary>
    public interface IAiLogger
    {
        void GoalSelected(Entity entity,IGoal goal);
        void ActionChosen(Entity entity,IAction action);
        void ActionStarted(Entity self, IAction action);
        void ActionApplied(Entity entity,IAction action);
    }
}
