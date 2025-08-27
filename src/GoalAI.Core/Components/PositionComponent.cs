using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Components
{
    public class PositionComponent :IComponent
    {
        public float X {  get; private set; }
        public float Y { get; private set; }

        public PositionComponent(float x, float y)
        {
            X = x;
            Y = y;
        }
        public void Set(float x, float y)
        {
            X = x;
            Y = y; 
        }

        public float DistanceTo(float x, float y)
        {
            var dx = x - X;
            var dy = y - Y;

            var d2= dx * dx + dy * dy;

            return (float)Math.Sqrt(d2);
        }

    }
}
