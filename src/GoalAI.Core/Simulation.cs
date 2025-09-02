using GoalAI.Core.Components;
using GoalAI.Core.Diagnostics;
using GoalAI.Core.Goals;
using GoalAI.Core.Planning;
using GoalAI.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;



namespace GoalAI.Core
{
    public class Simulation
    {
        private readonly World world;
        private readonly float replanInterval; // interval between planner re-evaluations
        private readonly IAiLogger? logger;

        private float time;           // global simulation time
        private float sinceReplan;   // accumulated time since last replan

        //  last time each action was executed by an entity (for cooldown checks)
        private readonly Dictionary<(Entity entity, IAction action), float> lastUse = new Dictionary<(Entity entity, IAction action), float>();

        //  currently running actions per entity and their remaining time
        private readonly Dictionary<Entity, (IAction action, float remaining)> running = new Dictionary<Entity, (IAction action, float remaining)>();


        private GoapPlanner planner = new GoapPlanner();

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

            Tick(deltaTime); //Tick ​​all ITickable components

            // update currently running actions
            if (running.Count > 0)
            {
                var finished = new List<Entity>(running.Count);

                foreach (var kv in running.ToList())
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
                        running[e] = (action, remaining); // still running => update remaining time
                    }
                }

                foreach (var e in finished)
                    running.Remove(e);
            }

            // replan only at given intervals
            if (sinceReplan < replanInterval)
                return;
            sinceReplan = 0f;

            // Planning actions for entities that are currently idle
            foreach (var entity in world.Entities)
            {
                if (running.ContainsKey(entity))
                    continue;

                var ai = entity.GetComponent<AIComponent>();
                if (ai is null || ai.Goals.Count == 0)
                    continue;

                // the highest priority goal that is not yet satisfied
                var goal = ai.Goals
                             .OrderByDescending(g => g.Priority)
                             .FirstOrDefault(g => !g.IsSatisfied(world, entity));
                if (goal is null)
                    continue;

                EnsureFoodTarget(entity);
                logger?.GoalSelected(entity, goal);



                if (TryPlanAndStart(entity, ai, goal))
                    continue;

                logger?.PlanningFailed_NoPlanFound(entity,goal);



                // DEPRECATED: Old greedy action selection.
                // Replaced by GOAP planner, but can be used as fallback if planning fails.

                //var action = ai.Actions
                //              .Where(a => a.IsApplicable(world, entity))
                //              .Where(a => IsOffCooldown(entity, a))
                //              .OrderBy(a => a.Cost(world, entity))
                //              .FirstOrDefault();

                // if (action is null)
                //     continue;

                // logger?.ActionChosen(entity, action);


                // float duration = 0f;
                // if (action is IDurationAction da)
                // {
                //     duration = da.GetDuration(world, entity);
                // }

                // if (duration > 0f)
                // {
                //     running[entity] = (action, duration);
                //     logger?.ActionStarted(entity, action);
                // }
                // else
                // {
                //     action.Apply(world, entity);
                //     logger?.ActionApplied(entity, action);
                //     lastUse[(entity, action)] = time;
                // }
            }
        }


        /// <summary>
        /// Returns true if the action's cooldown has expired for the given entity.
        /// </summary>
        private bool IsOffCooldown(Entity entity, IAction action)
        {
            if (action is not ICooldownAction cd || cd.CooldownSeconds <= 0f)
                return true;

            if (!lastUse.TryGetValue((entity, action), out var last))
                return true; // never used before

            return (time - last) >= cd.CooldownSeconds; // true == cooldown expired
        }



        // ticks all components that implement ITickable on every entity
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


        /// <summary>
        /// Builds an abstract planning state from the entity and world
        /// </summary>
        private PlanState BuildPlanState(Entity e)
        {
            var s = new PlanState();

            var inv = e.GetComponent<InventoryComponent>();
            var h = e.GetComponent<HungerComponent>();
            var pos = e.GetComponent<PositionComponent>();

            //var bb = e.GetComponent<EntityBlackBoardComponent>();

            s.Set("food", inv?.Food ?? 0);
            s.Set("hunger", (int)System.MathF.Round(h?.Hunger ?? 0f));



            int totalFood = 0;
            foreach (var n in world.Resources.Resources)
                if (n.Type == ResourceType.FoodPile) 
                    totalFood += n.Amount;
            s.Set("world_food_pile", totalFood);

            if(pos is not null)
            {
                var node = world.Resources.FindClosest(ResourceType.FoodPile, pos.X, pos.Y);

                if (node is not null)
                {
                    var dist = pos.DistanceTo(node.X, node.Y);
                    s.Set("distance", (int)System.MathF.Round(dist));
                    s.Set("at_target", dist <= 0.5f ? 1 : 0);
                }
                else
                {
                    s.Set("distance", 0);
                    s.Set("at_target", 1); // 
                }

            }
            else
            {
                s.Set("distance", 0);
                s.Set("at_target", 1);
            }

            return s;


        }


        // creates a goal predicate for the planner based on the provided goal
        private Func<PlanState,bool> MakeGoalTest(IGoal goal, Entity entity)
        {
            switch(goal)
            {
                case AvoidHungerGoal ah:
                    return s => s.Get("hunger") < ah.Threshold;

                case CollectFoodGoal cf:
                    return s => s.Get("food") > cf.MinFood;

                case StayAliveGoal sa:
                    return s => s.Get("hunger") < (int)sa.MaxHunger;

                default:
                    return null;
            }
        }

        private bool TryPlanAndStart(Entity e, AIComponent ai, IGoal goal)
        { 
            var test = MakeGoalTest(goal, e);

           
            if(test is null)
            {
                logger?.PlanningFailed_NoGoalTest(e,goal);
                return false;
            }
            if(ai.PlanningActions.Count==0)
            {
                logger?.PlanningFailed_NoActions(e,goal);
                return false;
            }


            var start = BuildPlanState(e);

            
            if (!planner.Plan(start, test, ai.PlanningActions, out var plan) || plan.Count == 0)
            {
                logger?.PlanningFailed_NoPlanFound(e, goal);
                return false;
            }

            //if(plan.Count == 0) 
            //    return false;

            // take the first action from the plan (which we should start)
            var first = plan[0].RuntimeAction;

            if (!first.IsApplicable(world, e))
                return false;

            // cooldown gate : if it is still running, just wait
            if (!IsOffCooldown(e, first))
                return true;

            logger?.ActionChosen(e, first);

            float duration = 0f;
            if (first is IDurationAction da)
            {
                duration = da.GetDuration(world, e);
            }

            if(duration > 0f)
            {
                running[e] = (first, duration);
                logger?.ActionStarted(e, first);
            }
            else
            {
                first.Apply(world, e);

                logger?.ActionApplied(e, first);

                lastUse[(e, first)] = time;
            }

            return true;
        }


        
        // Ensures the entity's blackboard has a target position set to the nearest food pile.
        private void EnsureFoodTarget(Entity entity)
        {
            var bb = entity.GetComponent<EntityBlackBoardComponent>()?.Data;
            var pos = entity.GetComponent<PositionComponent>();
            if (bb is null || pos is null) return;


            var node = world.Resources.FindClosest(ResourceType.FoodPile, pos.X, pos.Y);
            if (node is not null)
                bb.Set("target_pos", (node.X, node.Y));

        }
    }
}
