using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    class Simulation
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
                //TODO vybrat nejdulezitejsi nesplneny cil, vybrat nejlevnejsi akci a provest

            }
        }
    }
}
