using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    public interface IAction
    {
        string Name { get; }
        bool IsApplicable(World world,Entity entity);
        float Cost(World world, Entity entity);
        void Apply(World world, Entity entity);

    }
}
