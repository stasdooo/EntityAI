using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    /// <summary>
    /// Defines an object that updates itself every simulation step.
    /// </summary>
    public interface ITickable
    {
        //Called each frame with the elapsed time in seconds
        void Tick(float deltaTime);
    }
}
