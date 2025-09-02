using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core
{
    /// <summary>
    /// Defines an action that cannot be repeated until its cooldown expires.
    /// </summary>
    internal interface ICooldownAction
    {
        //Cooldown time in seconds after the action is used
        float CooldownSeconds { get; }
    }
}
