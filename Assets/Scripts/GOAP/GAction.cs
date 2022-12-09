using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    //Action ������
    public string actionName = "Action";
    //Action �����ģ������ڽ�������ֵ�͵� Action
    public float cost = 1.0f;
    //Action �ж���Ŀ������ƶ���ȥ��
    public GameObject target;
    //Action �ж���Ŀ������ Tag���ƶ���ȥ��
    public string targetTag;
    //Action ��ͣ��ʱ��
    public float duration = 0;
    //Action ��ǰ��״̬��������Ϊ WorldState �����л��������� inspector �������ã�
    public WorldState[] preConditions;
    //Action ��ɺ��״̬��������Ϊ WorldState �����л��������� inspector �������ã�
    public WorldState[] afterEffects;
    //������úõ� Action ��ǰ��״̬����
    public Dictionary<string, int> preconditions;
    //������úõ� Action ��ɺ��״̬����
    public Dictionary<string, int> effects;
    //��ǰ Action ������״̬
    public WorldStates agentBeliefs;

    public GInventory inventory;
    public NavMeshAgent agent;

    public WorldStates beliefs;
    //�����ж��Ƿ����ڽ���ĳ�� Action
    public bool running = false;

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        //�������� inspector ���úõ� Action ��ǰ��״̬����������ֵ伯 preconditions ��
        if (preConditions != null)
        {
            foreach (WorldState w in preConditions)
                preconditions.Add(w.key, w.value);
        }
        //�������� inspector ���úõ� Action ��ɺ��״̬����������ֵ伯 effects ��
        if (afterEffects != null)
        {
            foreach (WorldState w in afterEffects)
                effects.Add(w.key, w.value);
        }
        inventory = this.GetComponent<GAgent>().inventory;
        beliefs = this.GetComponent<GAgent>().beliefs;
    }
    //�����жϸ� Action �Ƿ������
    public bool IsAchievable()
    {
        return true;
    }
    //�����ж� ���ǵ�ĳЩ�Ⱦ�������Action �Ƿ����ʵ��
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        //���� Action ��ǰ��״̬�����봫���� �ֵ伯 ���бȽϣ��������ͬ״̬��˵�� Action �ܱ�ʵ��
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
