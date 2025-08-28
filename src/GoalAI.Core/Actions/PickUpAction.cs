using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Actions
{
    public class PickUpAction : IAction
    {
        public string Name => "PickUp";


        public bool IsApplicable(World world, Entity entity)
        {
            var inv = entity.GetComponent<InventoryComponent>();
            var pos = entity.GetComponent<PositionComponent>();
            var bb = entity.GetComponent<EntityBlackBoardComponent>();

            if (inv is null || pos is null || bb is null) 
                return false;


            float count;
            world.State.Properties.TryGet("food_pile", out count);

            bool target_position = bb.Data.TryGet<(float x, float y)>("target_pos", out _);

            return count>0 && target_position;


        }
        public void Apply(World world, Entity entity)
        {
            var inv = entity.GetComponent<InventoryComponent>()!;

            float count;
            world.State.Properties.TryGet("food_pile", out count);

            if (count <= 0) return;

            world.State.Properties.Set("food_pile", count - 1);
            inv.Food += 1;

        }

        public float Cost(World world, Entity entity)
        {
            return 2;
        }

       
    }
}
