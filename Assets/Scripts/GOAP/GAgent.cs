using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//子目标类
public class SubGoal
{
    public Dictionary<string, int> sgoals;
    //用于判断该子目标在被完成一次后是否要移除
    public bool remove;
    // int 值代表该子目标的优先度，数值越高，优先度越高
    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}
public class GAgent : MonoBehaviour
{
    //存放所有的 Action 的列表
    public List<GAction> actions = new();
    //存放所有子目标的字典集
    public Dictionary<SubGoal, int> goals = new();
    public GInventory inventory = new();
    public WorldStates beliefs = new();

    GPlanner planner;
    //Action 队列
    Queue<GAction> actionQueue;
    //当前正在进行的 Action
    public GAction currentAction;
    //当前要达成的目标
    SubGoal currentGoal;

    Vector3 destination = Vector3.zero;
    public void Start()
    {
        //获取所有的 Action 脚本，并存放到 actions 列表
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a);
        }
    }

    bool invoked = false;
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    private void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(destination, this.transform.position);
            //Debug.Log(currentAction.agent.hasPath + " " + distanceToTarget);
            if (distanceToTarget < 2f)
            {
                if (!invoked)
                {
                    Invoke(nameof(CompleteAction), currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();
            // 根据 Value 降序排序
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions, sg.Key.sgoals, beliefs);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if (currentAction.target != null)
                {
                    currentAction.running = true;

                    destination = currentAction.target.transform.position;
                    Transform dest = currentAction.target.transform.Find("Destination");
                    if (dest != null)
                    {
                        destination = dest.position;
                    }

                    currentAction.agent.SetDestination(destination);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
