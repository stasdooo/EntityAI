using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Actions
{

    /// <summary>
    /// Action that moves the entity to a target position stored in its blackboard.
    /// The duration and cost are calculated from the distance and movement speed.
    /// </summary>
    public class MoveToAction : IAction, IDurationAction
    {
        public string Name => "MoveToAction";

        private string targetKey;
        private float speed;

        
        // Creates a new move action that reads the target position from the given blackboard key.
        public MoveToAction(string targetKey, float speed = 2)
        {
            this.targetKey = targetKey;
            this.speed = speed;
        }

        // Calculates how long the move will take based on distance and speed
        public float GetDuration(World world, Entity entity)
        {
            var pos = entity.GetComponent<PositionComponent>();
            var bb = entity.GetComponent<EntityBlackBoardComponent>();

            (float tx, float ty) target = (pos.X, pos.Y);
            bb.Data.TryGet<(float, float)>(targetKey, out target);
           

            return pos.DistanceTo(target.tx,target.ty)/speed;

        }

        // Moves the entity instantly to the target position
        public void Apply(World world, Entity entity)
        {
            var pos = entity.GetComponent<PositionComponent>();
            var bb = entity.GetComponent<EntityBlackBoardComponent>();

            (float tx, float ty) target = (pos.X, pos.Y);
            bb.Data.TryGet<(float, float)>(targetKey, out target);

            pos.Set(target.tx, target.ty);
        }

        public float Cost(World world, Entity entity)
        {
            return GetDuration(world,entity);
        }


        // Checks if the entity has a position component and a valid target in the blackboard
        public bool IsApplicable(World world, Entity entity)
        {

            var pos = entity.GetComponent<PositionComponent>();
            var bb = entity.GetComponent<EntityBlackBoardComponent>();
            return pos is not null && bb is not null && bb.Data.TryGet<(float x, float y)>(targetKey, out _);


        }
    }
}
