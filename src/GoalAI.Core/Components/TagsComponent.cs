using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Components
{
    /// <summary>
    /// Component that stores a set of string tags for an entity or the world.
    /// </summary>
    public class TagsComponent : IComponent
    {
        private HashSet<string> tags = new HashSet<string>();

        public bool Has(string tag)
        {
            return tags.Contains(tag);
        }

        public void Add(string tag)
        {
            tags.Add(tag);
        }

        public void Remove(string tag)
        {
            tags.Remove(tag);
        }

    }
}
