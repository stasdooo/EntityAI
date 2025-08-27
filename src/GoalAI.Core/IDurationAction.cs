using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    public interface IDurationAction
    {
        float GetDuration(World world, Entity entity);
    }
}
