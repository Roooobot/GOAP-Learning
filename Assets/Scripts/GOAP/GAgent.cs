using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//��Ŀ����
public class SubGoal
{
    public Dictionary<string, int> sgoals;
    //�����жϸ���Ŀ���ڱ����һ�κ��Ƿ�Ҫ�Ƴ�
    public bool remove;
    // int ֵ�������Ŀ������ȶȣ���ֵԽ�ߣ����ȶ�Խ��
    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}
public class GAgent : MonoBehaviour
{
    //������е� Action ���б�
    public List<GAction> actions = new();
    //���������Ŀ����ֵ伯
    public Dictionary<SubGoal, int> goals = new();
    public GInventory inventory = new();
    public WorldStates beliefs = new();

    GPlanner planner;
    //Action ����
    Queue<GAction> actionQueue;
    //��ǰ���ڽ��е� Action
    public GAction currentAction;
    //��ǰҪ��ɵ�Ŀ��
    SubGoal currentGoal;

    Vector3 destination = Vector3.zero;
    public void Start()
    {
        //��ȡ���е� Action �ű�������ŵ� actions �б�
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
            // ���� Value ��������
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
