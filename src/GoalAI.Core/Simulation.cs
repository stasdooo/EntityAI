using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    public class Simulation
    {

        private World world;
        private float replanInterval;
        private float replanTime;

        public Simulation(World world, float replanInterval)
        {
            this.world = world;
            this.replanInterval = replanInterval;
        }

        public void Step(float time)
        {
            replanTime += time;

            if (replanTime < replanInterval)
                return;
            replanTime = 0;

            foreach( var entity in world.Entities )
            {
                var ai = entity.GetComponent<AIComponent>();
                if(ai is null) continue;


                // vybrat nejdulezitejsi nesplneny cil
                //nejdulezitejsi nesplneny cil
                var goal = ai.Goals.OrderByDescending(g => g.Priority).FirstOrDefault(g=> !g.IsCompleted(world,entity));
                if (goal is null)
                    continue;

                //vyber nejlevnejsi akci
                var action = ai.Actions.Where(a => a.IsApplicable(world, entity)).OrderBy(a => a.Cost(world, entity)).FirstOrDefault();

                action?.Apply(world, entity);

            }
        }
    }
}
