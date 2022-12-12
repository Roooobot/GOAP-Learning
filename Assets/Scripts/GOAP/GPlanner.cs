using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//每个 Action 都会成为一个节点
public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;
    public Node(Node parent, float cost, Dictionary<string, int> allStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);
        this.action = action;
    }
    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefstates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);
        foreach(KeyValuePair<string, int> b in beliefstates)
        {
            if(!this.state.ContainsKey(b.Key))
                this.state.Add(b.Key, b.Value);
        }
        this.action = action;
    }

}
public class GPlanner
{
    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefstates)
    {
        //在该对象所有的 Action 中，找出可以被完成的 Action
        List<GAction> usableActions = new();
        foreach (GAction a in actions)
        {
            if (a.IsAchievable())
                usableActions.Add(a);
        }
        List<Node> leaves = new();
        //创建 开始查找的节点
        Node start = new(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefstates.GetStates(), null);
        //是否已经计划好
        bool success = BuildGraph(start, leaves, usableActions, goal);
        //如果没有计划则返回
        if (!success)
        {
            //Debug.Log("NO PLAN");
            return null;
        }
        //在所有分支中找到 消耗最小的那条链路
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.cost < cheapest.cost)
                    cheapest = leaf;
            }
        }
        //最小消耗 支路存放在 result 列表中
        List<GAction> result = new();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }
        //将 result 列表的内容转到队列中
        Queue<GAction> queue = new();
        foreach (GAction a in result)
        {
            queue.Enqueue(a);
        }
        //Debug.Log("The Plan is: ");
        //foreach (GAction a in queue)
        //{
        //    Debug.Log("Q:" + a.actionName);
        //}
        return queue;
    }
    #region BuildGraph()利用递归构建 Action 计划树
    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (GAction action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new(parent.state);
                foreach (KeyValuePair<string, int> eff in action.effects)
                {
                    if (!currentState.ContainsKey(eff.Key))
                        currentState.Add(eff.Key, eff.Value);
                }
                Node node = new(parent, parent.cost + action.cost, currentState, action);
                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundPath = true;
                }
            }
        }
        return foundPath;
    }
    #endregion
    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new();
        foreach (GAction a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }
}
