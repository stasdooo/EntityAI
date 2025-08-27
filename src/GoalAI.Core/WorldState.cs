using GoalAI.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    public class WorldState
    {
        public Blackboard Blackboard = new Blackboard();
        public TagsComponent Tags = new TagsComponent();
        public PropertiesComponent Properties = new PropertiesComponent();
    }
}
