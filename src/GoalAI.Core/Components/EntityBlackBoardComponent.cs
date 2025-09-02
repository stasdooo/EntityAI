using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Components
{

    /// <summary>
    /// Component that holds a blackboard specific to the entity.
    /// Stores temporary values like goals, targets or context data.
    /// </summary>
    public class EntityBlackBoardComponent:IComponent
    {
        public Blackboard Data = new Blackboard();
    }
}
