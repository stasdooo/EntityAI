using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Goals
{

    /// <summary>
    /// Goal that requires the entity to keep at least a minimum amount of food in its inventory.
    /// </summary>
    public class CollectFoodGoal : IGoal
    {
        public string Name => "CollectFood";

        public int Priority { get; }

        //required food amount
        public int MinFood { get; }


        //priority and required food amount
        public CollectFoodGoal(int priority=4,int minFood=3)
        {
            Priority = priority;
            MinFood = minFood;
        }


        
        // returns true if the entity has an inventory with at least the required amount of food.
        public bool IsSatisfied(World world, Entity entity)
        {
            var inv = entity.GetComponent<InventoryComponent>();
            return inv is not null && inv.Food > MinFood;
        }
    }
}
