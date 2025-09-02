using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Components
{
    /// <summary>
    /// Component for storing named float properties.
    /// </summary>
    public class PropertiesComponent
    {
        private Dictionary<string,float> floats = new Dictionary<string,float>();

        public void Set(string key, float value)
        {
            floats[key] = value;
        }

        public bool TryGet(string key, out float value)
        {
            return floats.TryGetValue(key, out value);
        }
    }
}
