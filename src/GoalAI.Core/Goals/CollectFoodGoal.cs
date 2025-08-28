using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Goals
{
    public class CollectFoodGoal : IGoal
    {
        public string Name => "CollectFood";

        public int Priority { get; }
        public int MinFood { get; }

        public CollectFoodGoal(int priority=4,int minFood=3)
        {
            Priority = priority;
            MinFood = minFood;
        }


        //cil je splnen pokud ma inventar a ma v nem aspon MinFood kusu jidla
        public bool IsSatisfied(World world, Entity entity)
        {
            var inv = entity.GetComponent<InventoryComponent>();
            return inv is not null && inv.Food > MinFood;
        }
    }
}
