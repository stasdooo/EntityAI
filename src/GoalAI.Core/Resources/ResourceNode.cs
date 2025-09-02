using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Resources
{

    /// <summary>
    /// Represents a resource node in the world with type, position, and remaining amount
    /// </summary>
    public class ResourceNode
    {
        public ResourceType Type { get; }
        public float X { get; set; }
        public float Y { get; set; }
        public int Amount { get; set; }

        public ResourceNode(ResourceType type, float x, float y, int amount)
        {
            Type = type; 
            X = x;
            Y = y; 
            Amount = amount;
        }
    }
}
