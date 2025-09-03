using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    /// <summary>
    /// Defines an action an entity can perform in the world.
    /// </summary>
    public interface IAction
    {
        string Name { get; }

        //Checks if the action can run for the entity in the current world
        bool IsApplicable(World world,Entity entity);


        //Applies the action's effects to the entity and world
        void Apply(World world, Entity entity);

    }
}
