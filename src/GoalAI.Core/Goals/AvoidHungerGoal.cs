using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Goals
{


    /// <summary>
    /// Goal that becomes active when the entity's hunger rises above a threshold
    /// </summary>
    public class AvoidHungerGoal : IGoal
    {
        public string Name => "AvoidHunger";

        public int Priority { get; }


        
        //hunger level above which this goal is considered unsatisfied
        public float Threshold { get; }

        public AvoidHungerGoal(int priority=5,float threshold=40)
        {
            Priority = priority;
            Threshold = threshold;
        }

        public bool IsSatisfied(World world, Entity entity)
        {
            var hunger = entity.GetComponent<HungerComponent>();

            return hunger is not null && hunger.Hunger < Threshold;
        }
    }
}
