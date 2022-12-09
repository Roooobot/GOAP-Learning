using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ڷ���״̬
//valueС�ڵ���0��ᱻ�Ƴ�
[System.Serializable]
public class WorldState
{
    public string key;
    public int value;
}

public class WorldStates
{
    public Dictionary<string, int> states;
    public WorldStates()
    {
        states = new Dictionary<string, int>();
    }
    #region HasState()���ڲ�ѯ�Ƿ���ڸ�״̬
    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }
    #endregion
    #region AddState()��������µ�״̬
    private void AddState(string key, int value)
    {
        states.Add(key, value);
    }
    #endregion
    #region ModifyState()�����޸�״̬��ֵ�����������ϸ�ֵ����ִ�к�С��0���Ƴ������粻��������Ӹ�״̬
    public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
            if (states[key] <= 0)
                RemoveState(key);
        }
        else
        {
            states.Add(key, value);
        }
    }
    #endregion
    #region RemoveState()�����Ƴ�״̬
    public void RemoveState(string key)
    {
        if (states.ContainsKey(key))
            states.Remove(key);
    }
    #endregion
    #region SetState()��������״̬�����������������ֵ���粻��������Ӹ�״̬
    public void SetState(string key, int value)
    {
        if (states.ContainsKey(key))
            states[key] = value;
        else
            states.Add(key, value);
    }
    #endregion
    #region GetStates()���ڷ�������״̬
    public Dictionary<string, int> GetStates()
    {
        return states;
    }
    #endregion
}
