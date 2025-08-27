using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Components
{
    public class EntityBlackBoardComponent:IComponent
    {
        public Blackboard Data = new Blackboard();
    }
}
