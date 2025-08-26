using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Goals
{


    public class AvoidHungerGoal : IGoal
    {
        public string Name => "AvoidHunger";

        public int Priority { get; }

        public AvoidHungerGoal(int priority=5)
        {
            Priority = priority;
        }

        public bool IsCompleted(World world, Entity entity)
        {
            var hunger = entity.GetComponent<HungerComponent>();

            return hunger is not null && hunger.Hunger < 40;
        }
    }
}
