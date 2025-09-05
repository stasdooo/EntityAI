
using GoalAI.Core;
using GoalAI.Core.Actions;
using GoalAI.Core.Components;
using GoalAI.Core.Goals;
using GoalAI.Core.Diagnostics;
using GoalAI.Core.Planning.Actions;
using GoalAI.Core.Resources;
namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var world = new World();

            // resources
            world.Resources.Add(new ResourceNode(ResourceType.FoodPile, 5f, 3f, amount: 5));
            world.Resources.Add(new ResourceNode(ResourceType.FoodPile, -2f, 1f, amount: 2));

            // entity
            var peasant = new Entity("Peasant");
            peasant.AddComponent(new HungerComponent(60f) { HungerPerSecond = 0.8f });
            peasant.AddComponent(new InventoryComponent());
            peasant.AddComponent(new PositionComponent(0f, 0f));
            peasant.AddComponent(new EntityBlackBoardComponent()); // 

            // goals
            var ai = peasant.AddComponent(new AIComponent())                        
                .AddGoal(new AvoidHungerGoal(priority: 5, 30f))
                .AddGoal(new CollectFoodGoal(priority: 4, minFood: 3))
                .AddGoal(new StayAliveGoal(priority: 10, maxHunger: 90f));

          
            // actions
            ai.AddPlanningAction(new MoveToPlanAction(new MoveToAction(BlackboardKey.TargetFood), speed: 2f))
              .AddPlanningAction(new PickUpPlanAction(new PickUpAction()))
              .AddPlanningAction(new EatPlanAction(new EatAction(), threshold: 30))
              .AddPlanningAction(new MakeFoodPlanAction(new MakeFoodAction()));

            world.AddEntity(peasant);

           
            var logger = new ConsoleAiLogger();
            var sim = new Simulation(world, replanInterval: 0.25f, logger: logger);

            //  40 sekund po 0.5 s ticku 
            var pos = peasant.GetComponent<PositionComponent>()!;
            var inv = peasant.GetComponent<InventoryComponent>()!;
            var hung = peasant.GetComponent<HungerComponent>()!;

            Console.WriteLine(" Start simulation ");
            for (int i = 0; i < 80; i++)
            {
                sim.Step(0.5f);

                Console.WriteLine("[Tick " + (i + 1).ToString("00") + "] " + "Pos=(" + pos.X.ToString("0.0") + "," + pos.Y.ToString("0.0") + ")  " +
                    "Food=" + inv.Food + "  Hunger=" + hung.Hunger + "  WorldFood=" + 
                    world.Resources.Resources
                        .Where(n => n.Type == ResourceType.FoodPile)
                         .Sum(n => n.Amount)
);
            }
            Console.WriteLine(" End simulation ");


        }

    }
    
}