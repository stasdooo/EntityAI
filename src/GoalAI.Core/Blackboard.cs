using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    /// <summary>
    // key-value storage for arbitrary data
    /// </summary>
    public class Blackboard
    {
        private Dictionary<string,object?> data = new Dictionary<string,object?>();

        public void Set<T>(string key, T value)
        {
            data[key] = value;
        }


        //Tries to get a value by key and cast it to the given type
        //  Returns true if the key exists and the type matches; otherwise sets to default and retur false
        public bool TryGet<T>(string key,out T? value)
        {
            if (data.TryGetValue(key, out var result))
            {
                if (result is T t)
                {
                    value = t;
                    return true;
                }
            }
            value = default(T);
            return false;
        }

    }
}
