using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Actions
{
    //hledani jidla
    public class ForageAction : IAction, ICooldownAction, IDurationAction
    {
        public string Name => "Forage";

        public float CooldownSeconds => 2f;
        public float DurationSeconds => 2f;
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

        public float Cost(World world, Entity entity)
        {
            return 10;
        }

        public bool IsApplicable(World world, Entity entity)
        {
            return true;
        }
    }
}
