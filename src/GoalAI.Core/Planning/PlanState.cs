using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Planning
{
    /// <summary>
    /// Represents an abstract world state used by the planner.
    /// </summary>
    public class PlanState : IEquatable<PlanState>
    {

        private Dictionary<string,int> values   = new Dictionary<string,int>();


        //Returns the value for the given key, or 0 if not set
        public int Get(string key)
        {
            int value = 0;
            values.TryGetValue(key, out value);
            return value;

        }


        public void Set(string key, int value)
        {
            values[key] = value;
        }


        //deep copy
        public PlanState Clone()
        {
            var s = new PlanState();
            foreach(var x in values)
            {
                s.values[x.Key]=x.Value;
            }
            return s;
        }

        public bool Equals(PlanState? other)
        {
            if (other is null || other.values.Count != values.Count) 
                return false;

            foreach (var kv in values)
            {
                if (!other.values.TryGetValue(kv.Key, out var v) || v != kv.Value)
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int h = 17;
            foreach (var kv in values.OrderBy(k => k.Key))
            {
                h = h * 31 + kv.Key.GetHashCode();
                h = h * 31 + kv.Value.GetHashCode();
            }
            return h;
        }

    }
}
