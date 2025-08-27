using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Actions
{
    public class MoveToAction : IAction, IDurationAction
    {
        public string Name => throw new NotImplementedException();

        private string targetKey;
        private float speed;


        //TODO logic with coordinates?
        public MoveToAction(string targetKey, float speed = 2)
        {
            this.targetKey = targetKey;
            this.speed = speed;
        }

        public float GetDuration(World world, Entity entity)
        {
            var pos = entity.GetComponent<PositionComponent>();
            var bb = entity.GetComponent<EntityBlackBoardComponent>();

            (float tx, float ty) target = (pos.X, pos.Y);
            bb.Data.TryGet<(float, float)>(targetKey, out target);
           

            return pos.DistanceTo(target.tx,target.ty)/speed;

        }

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


        public bool IsApplicable(World world, Entity entity)
        {

            var pos = entity.GetComponent<PositionComponent>();
            var bb = entity.GetComponent<EntityBlackBoardComponent>();
            return pos is not null && bb is not null && bb.Data.TryGet<(float x, float y)>(targetKey, out _);


        }
    }
}
