using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Goals
{

    /// <summary>
    /// Goal that is satisfied as long as the entity's hunger stays below a critical level.
    /// </summary>
    public class StayAliveGoal : IGoal
    {
        public string Name => "StayAlive";
        public int Priority { get; }
        public float MaxHunger { get; }

        public StayAliveGoal(int priority = 10, float maxHunger = 90f)
        { 
            Priority = priority; 
            MaxHunger = maxHunger;
        }

        public bool IsSatisfied(World world, Entity entity)
        {
            var hunger = entity.GetComponent<HungerComponent>();


            return hunger is not null && hunger.Hunger < MaxHunger;
        }
           
    }
}
