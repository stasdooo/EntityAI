using GoalAI.Core.Diagnostics;
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
        private IAiLogger? logger;

        public Simulation(World world, float replanInterval,IAiLogger? logger = null )
        {
            this.world = world;
            this.replanInterval = replanInterval;
            this.logger = logger;
        }

        public void Step(float time)
        {
            replanTime += time;

            Tick(time);


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


                logger?.GoalSelected(entity, goal);

                //vyber nejlevnejsi akci
                var action = ai.Actions.Where(a => a.IsApplicable(world, entity)).OrderBy(a => a.Cost(world, entity)).FirstOrDefault();

                if(action is not null)
                {
                    logger?.ActionChoosen(entity, action);
                    action.Apply(world, entity);
                    logger?.ActionApplied(entity, action);  

                }
                

            }
        }

        //Tick all ITickable components
        private void Tick(float deltaTime)
        {
            foreach( var entity in world.Entities )
            {
                foreach(var c in entity.Components )
                {
                    if (c is ITickable t)
                        t.Tick(deltaTime);
                }
            }
        }
    }
}
