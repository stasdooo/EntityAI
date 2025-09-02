using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Resources
{

    /// <summary>
    /// Registry that keeps track of all resource nodes in the world.
    /// </summary>
    public class ResourceRegistry
    {
        private List<ResourceNode> resources = new List<ResourceNode>();

        public IReadOnlyList<ResourceNode> Resources { get { return resources; } }

        public ResourceNode Add(ResourceNode node)
        {
            resources.Add(node);
            return node;
        }


        // Finds the closest resource of the given type to the specified position,
        // or null if none exist.
        public ResourceNode FindClosest(ResourceType type, float x, float y)
        {
            ResourceNode? best = null;
            float bestD2 = float.MaxValue;

            foreach( var resource in resources )
            {
                if( resource.Type != type || resource.Amount<=0)
                    continue;
                var dx = resource.X - x;
                var dy = resource.Y - y;

                var d2 = dx * dx + dy * dy;

                if(d2<bestD2)
                {
                    bestD2 = d2;
                    best = resource;
                }
                    


            }
            return best;

        }


        // Finds a resource of the given type at the specified position (within 0.5 units),
        // or null if none is present. ( The distance between the source and the point is less than 0.5 units => ok)
        public ResourceNode FindAat(ResourceType type, float x, float y)
        {
            foreach( var resource in resources )
            {
                if(resource.Type != type || resource.Amount<=0)
                    continue;
                var dx = resource.X - x;
                var dy = resource.Y - y;

                if(dx*dx + dy*dy<0.25f)
                    return resource;
            }
            return null;

        }


    }
}
