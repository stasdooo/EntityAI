using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Components
{

    /// <summary>
    /// Component that represents an entity's inventory.
    /// Stores quantities of items or resources the entity carries.
    /// </summary>
    public class InventoryComponent:IComponent
    {
        public int Food { get; set; }
    }
}
