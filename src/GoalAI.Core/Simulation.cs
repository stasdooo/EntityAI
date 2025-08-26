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
        private float sinceReplan;
        private IAiLogger? logger;

        private float time; //herni cas v sekundach

        //cas posledniho provedeni akce na entite
        private Dictionary<(Entity, IAction), float> lastUse = new Dictionary<(Entity, IAction), float> ();

        public Simulation(World world, float replanInterval,IAiLogger? logger = null )
        {
            this.world = world;
            this.replanInterval = replanInterval;
            this.logger = logger;
        }

        public void Step(float delatTime)
        {
            sinceReplan += delatTime;
            time += delatTime;


            Tick(delatTime);


            if (sinceReplan < replanInterval)
                return;
            sinceReplan = 0;

            foreach( var entity in world.Entities )
            {
                var ai = entity.GetComponent<AIComponent>();
                if(ai is null) continue;


                // vybrat nejdulezitejsi nesplneny cil
                //nejdulezitejsi nesplneny cil podle priority
                var goal = ai.Goals.OrderByDescending(g => g.Priority).FirstOrDefault(g=> !g.IsCompleted(world,entity));
                if (goal is null)
                    continue;


                logger?.GoalSelected(entity, goal);

                //vyber nejlevnejsi akci, krerou muzeme pouzit a je dostupna(neni "cooldown")
                var action = ai.Actions.Where(a => a.IsApplicable(world, entity)).Where(a=>IsOffCoolDown(entity,a)).OrderBy(a => a.Cost(world, entity)).FirstOrDefault();

                if(action is not null)
                {
                    logger?.ActionChoosen(entity, action);
                    action.Apply(world, entity);
                    logger?.ActionApplied(entity, action);

                    lastUse[(entity, action)] = time;

                }
                

            }
        }


        private bool IsOffCoolDown(Entity entity, IAction action)
        {
            if(action is not ICoolDownAction c || c.CooldownSeconds<=0)
                return true;

            float lastuse;
            if(lastUse.TryGetValue((entity,action),out lastuse))
                return true;

            return time - lastuse > c.CooldownSeconds;
           
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
