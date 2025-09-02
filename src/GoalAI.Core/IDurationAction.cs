using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{

    /// <summary>
    /// Defines an action that has a duration before it completes.
    /// </summary
    public interface IDurationAction
    {
        // Returns the time in seconds this action should take for the given entity in the world
        float GetDuration(World world, Entity entity);
    }
}
