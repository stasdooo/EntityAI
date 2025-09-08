using GoalAI.Core;
using GoalAI.Core.Actions;
using GoalAI.Core.Components;
using GoalAI.Core.Goals;
using GoalAI.Core.Planning.Actions;
using System.Linq;
using UnityEngine;

public class AiAgentController : MonoBehaviour 
{
    [Header("Start state")]
    public float initialHunger = 60f;
    public float hungerPerSecond = 0.8f;
    public float moveSpeed = 2f;

    [Header("Goals")]
    public float avoidHungerThreshold = 30f;
    public int FoodMin = 3;

    [Header("Visual sync")]
    public float trackingSpeed = 0f; //if 0 == teleport

    public Entity Entity { get; private set; } = null!;

    private void Start()
    {
        var world = AiWorldBootstrap.Instance.World;
        var p = transform.position;

        
        Entity = new Entity(gameObject.name);
        Entity.AddComponent(new HungerComponent(initialHunger) { HungerPerSecond = hungerPerSecond });
        Entity.AddComponent(new InventoryComponent());
        Entity.AddComponent(new PositionComponent(p.x, p.y));
        Entity.AddComponent(new EntityBlackBoardComponent());

        
        var ai = Entity.AddComponent(new AIComponent())
            .AddGoal(new AvoidHungerGoal(priority: 5, avoidHungerThreshold))
            .AddGoal(new CollectFoodGoal(priority: 4, minFood: FoodMin))
            .AddGoal(new StayAliveGoal(priority: 10, maxHunger: 90f));


        ai.AddPlanningAction(new MoveToPlanAction(new MoveToAction(BlackboardKey.TargetFood, speed: moveSpeed), speed: moveSpeed))
          .AddPlanningAction(new PickUpPlanAction(new PickUpAction()))
          .AddPlanningAction(new EatPlanAction(new EatAction(), threshold: avoidHungerThreshold))
          .AddPlanningAction(new MakeFoodPlanAction(new MakeFoodAction()));

        world.AddEntity(Entity);
    }

    private void LateUpdate()
    {

        if (Entity is null) return;
        var pos = Entity.GetComponent<PositionComponent>();
        if (pos is null) return;

        var target = new Vector3(pos.X, pos.Y, 0f);

        if (trackingSpeed > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, trackingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = target; 
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
          if (Entity is null) return;
          var inv = Entity.GetComponent<InventoryComponent>();
          var hung = Entity.GetComponent<HungerComponent>();

          string label;
          if (hung != null && hung.Hunger >= 100f)
          {
              label = "DEAD";
          }
          else
          {
              label = "Food=" + (inv?.Food ?? 0) +
                      "  Hunger=" + (hung?.Hunger.ToString("F0") ?? "0");
          }

          UnityEditor.Handles.Label(
              transform.position + Vector3.up * 0.5f + Vector3.right * 0.5f,
              label
          );
            }
#endif

}

