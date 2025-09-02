using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Actions
{

    /// <summary>
    /// Action that picks up one unit of food from a resource node at the entity's position.
    /// </summary>
    public class PickUpAction : IAction
    {
        public string Name => "PickUp";



        // Checks if the entity has inventory and position components,
        // and if there is a food pile at the same location.
        public bool IsApplicable(World world, Entity entity)
        {
            var inv = entity.GetComponent<InventoryComponent>();
            var pos = entity.GetComponent<PositionComponent>();
            

            if (inv is null || pos is null ) 
                return false;


            var node = world.Resources.FindAat(Resources.ResourceType.FoodPile, pos.X, pos.Y);
            return node is not null;


        }
        public void Apply(World world, Entity entity)
        {

            var inv = entity.GetComponent<InventoryComponent>()!;
            var pos = entity.GetComponent<PositionComponent>()!;
            var node = world.Resources.FindAat(Resources.ResourceType.FoodPile, pos.X, pos.Y);
            if (node is null) 
                return;


            if (node.Amount > 0)
            {
                node.Amount -= 1;
                inv.Food += 1;
            }


        }

        public float Cost(World world, Entity entity)
        {
            return 2;
        }

       
    }
}
