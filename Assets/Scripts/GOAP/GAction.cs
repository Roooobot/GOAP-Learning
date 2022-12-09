using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    //Action 的名字
    public string actionName = "Action";
    //Action 的消耗，倾向于进行消耗值低的 Action
    public float cost = 1.0f;
    //Action 行动的目标对象（移动过去）
    public GameObject target;
    //Action 行动的目标对象的 Tag（移动过去）
    public string targetTag;
    //Action 的停留时间
    public float duration = 0;
    //Action 的前置状态条件（因为 WorldState 可序列化，用于在 inspector 界面设置）
    public WorldState[] preConditions;
    //Action 完成后的状态条件（因为 WorldState 可序列化，用于在 inspector 界面设置）
    public WorldState[] afterEffects;
    //存放设置好的 Action 的前置状态条件
    public Dictionary<string, int> preconditions;
    //存放设置好的 Action 完成后的状态条件
    public Dictionary<string, int> effects;
    //当前 Action 的世界状态
    public WorldStates agentBeliefs;

    public GInventory inventory;
    public NavMeshAgent agent;

    public WorldStates beliefs;
    //用于判断是否正在进行某个 Action
    public bool running = false;

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        //将所有在 inspector 设置好的 Action 的前置状态条件存放在字典集 preconditions 中
        if (preConditions != null)
        {
            foreach (WorldState w in preConditions)
                preconditions.Add(w.key, w.value);
        }
        //将所有在 inspector 设置好的 Action 完成后的状态条件存放在字典集 effects 中
        if (afterEffects != null)
        {
            foreach (WorldState w in afterEffects)
                effects.Add(w.key, w.value);
        }
        inventory = this.GetComponent<GAgent>().inventory;
        beliefs = this.GetComponent<GAgent>().beliefs;
    }
    //用于判断该 Action 是否能完成
    public bool IsAchievable()
    {
        return true;
    }
    //用于判断 考虑到某些先决条件，Action 是否可以实现
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        //将该 Action 的前置状态条件与传进的 字典集 进行比较，如存在相同状态则说明 Action 能被实现
        foreach (KeyValuePair<string, int> p in preconditions)
        {
            if (!conditions.ContainsKey(p.Key))
                return false;
        }
        return true;
    }
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
