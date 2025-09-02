using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{

    /// <summary>
    /// Defines a goal that an entity tries to achieve.
    /// </summary>
    public interface IGoal
    {
        string Name { get; }

        //Priority of the goal, higher means more important
        int Priority { get; }

        //Checks if the goal is already satisfied in the current world
        bool IsSatisfied(World world, Entity entity);
    }
}
