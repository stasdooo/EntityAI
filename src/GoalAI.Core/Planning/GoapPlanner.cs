using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalAI.Core.Planning
{

    /// <summary>
    /// Simple GOAP planner that searches a sequence of planning actions from a start state to a goal state.
    /// </summary>
    public class GoapPlanner
    {
        private class Node
        {
            public PlanState State
            {
                get;
            }
            public Node? Parent { get; }
            public IPlanningAction Action { get; }

            public float TotalCost;
            

            public Node(PlanState state, Node? parent, IPlanningAction? action, float cost)
            {
                State = state;
                Parent = parent;
                Action = action;
                TotalCost = cost;
            }
        }
            public bool Plan(
                PlanState start,
                Func<PlanState, bool> goalTest, //predicate: true if state is a goal
                IReadOnlyCollection<IPlanningAction> actions,  // available planning actions
                out List<IPlanningAction> plan,//  resulting sequence of actions
                int maxIterations = 256
                )
            {
                plan = new List<IPlanningAction>();

                // nodes waiting to be explored
                var open = new List<Node>();
                open.Add(new Node(start, null, null, 0));

                //states that have already been explored
                var closed = new HashSet<PlanState>();

                int iterations = 0;

                while(open.Count > 0 && iterations++ < maxIterations)
                {
                
                    //  pick the node with the lowest path cost
                    open.Sort((a, b) => a.TotalCost.CompareTo(b.TotalCost));
                    var current = open[0];
                    open.RemoveAt(0);


                
                    //current state satisfies the goal?
                    if (goalTest(current.State))
                    {
                        // Reconstruct the plan
                        var path = new List<IPlanningAction>();
                        var n = current;

                        while(n.Action is not null)
                        {
                            path.Add(n.Action);
                            n = n.Parent;
                        }

                        path.Reverse();
                        plan = path;
                        return true; // plan found
                }

                    
                    closed.Add(current.State);


                    // expand all applicable actions from the current state
                    foreach (var act in actions)
                    {

                        
                        if (!act.CheckPreconditions(current.State))
                            continue;

                        
                        var nextState= current.State.Clone();
                        act.ApplyEffects(nextState);

                        if(closed.Contains(nextState))
                            continue;

                        // new path cost from start
                        var newCost = current.TotalCost + act.Cost(current.State);

                        //skip if an equal state is already in open with a better cost
                        var weaker = open.FirstOrDefault(n=>n.State.Equals(nextState) && n.TotalCost<= newCost);
                        if(weaker is not null )
                            continue; 

                       
                        open.Add(new Node(nextState, current, act, newCost));

                    }


                }
                return false; //no found plan
        }
        

    }
}
