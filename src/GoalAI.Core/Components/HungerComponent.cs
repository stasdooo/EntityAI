using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Components
{
    public class HungerComponent:IComponent
    {
        public float Hunger {  get; private set; }
        public HungerComponent(float hunger = 50)  // 0 = sytý
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
