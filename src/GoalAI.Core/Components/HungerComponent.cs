using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Components
{

    /// <summary>
    /// Component that tracks the hunger level of an entity (0 = full, 100 = starving).
    /// Increases over time and can be reduced by eating.
    /// </summary>
    public class HungerComponent:IComponent,ITickable
    {
        public float Hunger {  get; private set; }
        public float HungerPerSecond { get; set; } = 1;

        public HungerComponent(float hunger = 60) 
        {
            Hunger = HungerComponent.ClampHunger(hunger);
        }

        public void Add(float amount)
        {
            Hunger = HungerComponent.ClampHunger(Hunger+amount);
        }

        public void Reduce(float amount)
        {
            Hunger = HungerComponent.ClampHunger(Hunger - amount);
        }

        public void Tick(float deltaTime)
        {
            Add(deltaTime*HungerPerSecond);
        }


        private static float ClampHunger(float hunger)
        {
            if (hunger < 0)
                return 0;
            else if (hunger > 100)
                return 100;
            return hunger;
        }
    }
}
