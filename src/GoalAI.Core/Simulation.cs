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
        private readonly World world;
        private readonly float replanInterval;
        private readonly IAiLogger? logger;

        private float time;           // herni cas v sekundach
        private float sinceReplan;   // akumuluje cas do dalsiho planovani

        //  cas posledniho provedeni akce na entite (pro cooldown)
        private readonly Dictionary<(Entity entity, IAction action), float> lastUse = new Dictionary<(Entity entity, IAction action), float>();

        //  prave bezici akce na entite
        private readonly Dictionary<Entity, (IAction action, float remaining)> running = new Dictionary<Entity, (IAction action, float remaining)>();

        public Simulation(World world, float replanInterval, IAiLogger? logger = null)
        {
            this.world = world;
            this.replanInterval = replanInterval;
            this.logger = logger;
        }

        public void Step(float deltaTime)
        {
            sinceReplan += deltaTime;
            time += deltaTime;

            //0) Tick vsech ITickable komponent
            Tick(deltaTime);

            //1) Progres prave bezicich akci
            if (running.Count > 0)
            {
                var finished = new List<Entity>(running.Count);

                foreach (var kv in running)
                {
                    var e = kv.Key;
                    var (action, remaining) = kv.Value;

                    remaining -= deltaTime;

                    if (remaining <= 0f)
                    {
                        action.Apply(world, e);
                        logger?.ActionApplied(e, action);
                        lastUse[(e, action)] = time;
                        finished.Add(e);
                    }
                    else
                    {
                        running[e] = (action, remaining); // update zbyvajiciho casu
                    }
                }

                foreach (var e in finished)
                    running.Remove(e);
            }

            //2) Replan jen v intervalu
            if (sinceReplan < replanInterval)
                return;
            sinceReplan = 0f;

            // 3) Planovani ( jen pro entity ktere zrovna nic nedelaji)
            foreach (var entity in world.Entities)
            {
                if (running.ContainsKey(entity))
                    continue;

                var ai = entity.GetComponent<AIComponent>();
                if (ai is null || ai.Goals.Count == 0)
                    continue;

                // Nejduležitější NESPLNĚNÝ cíl
                var goal = ai.Goals
                             .OrderByDescending(g => g.Priority)
                             .FirstOrDefault(g => !g.IsSatisfied(world, entity));
                if (goal is null)
                    continue;

                logger?.GoalSelected(entity, goal);

                // Nejlevnější použitelná akce, která není na cooldownu
                var action = ai.Actions
                              .Where(a => a.IsApplicable(world, entity))
                              .Where(a => IsOffCooldown(entity, a))
                              .OrderBy(a => a.Cost(world, entity))
                              .FirstOrDefault();

                if (action is null)
                    continue;

                logger?.ActionChosen(entity, action);

                
                float duration = 0f;
                if (action is IDurationAction da)
                {
                    duration = da.GetDuration(world, entity);
                }

                if (duration > 0f)
                {
                    running[entity] = (action, duration);
                    logger?.ActionStarted(entity, action);
                }
                else
                {
                    action.Apply(world, entity);
                    logger?.ActionApplied(entity, action);
                    lastUse[(entity, action)] = time;
                }
            }
        }

        private bool IsOffCooldown(Entity entity, IAction action)
        {
            if (action is not ICooldownAction cd || cd.CooldownSeconds <= 0f)
                return true;

            if (!lastUse.TryGetValue((entity, action), out var last))
                return true; // nikdy jeste nepouzito

            return (time - last) >= cd.CooldownSeconds; // true == cooldown vyprsel
        }

        
        private void Tick(float deltaTime)
        {
            foreach (var entity in world.Entities)
            {
                foreach (var c in entity.Components)
                {
                    if (c is ITickable t)
                        t.Tick(deltaTime);
                }
            }
        }
    }
}
