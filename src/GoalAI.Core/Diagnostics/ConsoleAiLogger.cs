using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Diagnostics
{
    public class ConsoleAiLogger : IAiLogger
    {
        public void GoalSelected(Entity entity, IGoal goal)
        {
            Console.WriteLine("AI "+entity.Name + " selected goal " + goal.Name);
        }

        public void ActionChoosen(Entity entity, IAction action)
        {
            Console.WriteLine("AI " + entity.Name + " chose action " +  action.Name);
        }

        public void ActionApplied(Entity entity, IAction action)
        {
            Console.WriteLine("AI " + entity.Name + " applied action " + action.Name );
        }
    }
}
