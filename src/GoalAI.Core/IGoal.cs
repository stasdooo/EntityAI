using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    public interface IGoal
    {
        string Name { get; }
        int Priority { get; }

        bool IsCompleted (World world, Entity entity);
    }
}
