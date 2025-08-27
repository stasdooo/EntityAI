using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Actions
{
    public class EatAction: IAction, ICooldownAction, IDurationAction
    {
        public string Name => "Eat";

        public float CooldownSeconds => 0.5f;
        public float GetDuration(World world,Entity entity)
        {
            return 0.5f;
        }

        public void Apply(World world, Entity entity)
        {

            var inv = entity.GetComponent<InventoryComponent>();
            var hunger = entity.GetComponent<HungerComponent>();

            if(inv is null || hunger is null || inv.Food<=0)
                return;

            inv.Food -=1;
            hunger.Reduce(25);



        }

        public float Cost(World world, Entity entity)
        {
            return 1; //levnejsi nez hledat
        }

        public bool IsApplicable(World world, Entity entity)
        {
            var inv = entity.GetComponent<InventoryComponent>();
            var hunger = entity.GetComponent<HungerComponent>();

            if ((inv is null) || (hunger is null))
                return false;

            return inv.Food > 0 && hunger.Hunger > 40;

        }
    }
}
