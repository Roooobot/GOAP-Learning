using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于放置状态
//value小于等于0则会被移除
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
    #region HasState()用于查询是否存在该状态
    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }
    #endregion
    #region AddState()用于添加新的状态
    private void AddState(string key, int value)
    {
        states.Add(key, value);
    }
    #endregion
    #region ModifyState()用于修改状态的值；如存在则加上该值（如执行后小于0则移除）；如不存在则添加该状态
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
    #region RemoveState()用于移除状态
    public void RemoveState(string key)
    {
        if (states.ContainsKey(key))
            states.Remove(key);
    }
    #endregion
    #region SetState()用于设置状态；如存在则设置它的值；如不存在则添加该状态
    public void SetState(string key, int value)
    {
        if (states.ContainsKey(key))
            states[key] = value;
        else
            states.Add(key, value);
    }
    #endregion
    #region GetStates()用于返回所有状态
    public Dictionary<string, int> GetStates()
    {
        return states;
    }
    #endregion
}
