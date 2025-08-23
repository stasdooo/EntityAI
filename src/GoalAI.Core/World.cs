using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    internal class World
    {
        
        private List<Entity> entities = new List<Entity>();

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
