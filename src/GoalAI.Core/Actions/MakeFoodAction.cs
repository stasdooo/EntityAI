using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Actions
{

    /// <summary>
    /// Artificial action that directly creates one unit of food in the agent's inventory.
    /// Unlike PickUpAction, this does not require any resources in the world.
    /// It's expensive and has a long cooldown, so it serves only as a fallback
    /// when no natural food sources are available.
    /// </summary>
    public class MakeFoodAction : IAction, ICooldownAction, IDurationAction
    {
        public string Name => "MakeFood";

        public float CooldownSeconds => 4f;
        public float GetDuration(World entity,Entity world)
        {
            return 4f;
        }
        public void Apply(World world, Entity entity)
        {
            var inv = entity.GetComponent<InventoryComponent>();

            if (inv is null)
            {
                inv = new InventoryComponent();
                entity.AddComponent(inv);
            }

            inv.Food += 1;
        }


        public bool IsApplicable(World world, Entity entity)
        {
            return true;
        }
    }
}

