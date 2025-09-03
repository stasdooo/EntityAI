using GoalAI.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{

    /// <summary>
    /// Represents the game world holding entities, global state, and resources.
    /// </summary>
    public class World
    {
        
        private List<Entity> entities = new List<Entity>();

        public WorldState State = new WorldState();

        public ResourceRegistry Resources = new ResourceRegistry();

        public IReadOnlyList<Entity> Entities 
        { 
            get { return entities; }
        } 
        
        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

    }
}
