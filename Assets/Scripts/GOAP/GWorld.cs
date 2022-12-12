using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceQueue
{
    public Queue<GameObject> que = new();
    public string tag;
    public string modState;
    #region 将 这个Tag的对象 的数量添加到当前世界状态
    public ResourceQueue(string t,string ms,WorldStates w)
    {
        tag = t;
        modState = ms;
        if (tag != "")
        {
            //查找并获取所有拥有该 Tag 的对象
            GameObject[] resources =GameObject.FindGameObjectsWithTag(tag);
            foreach(GameObject r in resources)
            {
                que.Enqueue(r);
            }
        }
        //在世界状态中添加该 状态 和它的数值
        if (modState != "")
        {
            w.ModifyState(modState, que.Count);
        }
    }
    #endregion

    #region 在世界状态中添加和移除 该对象
    public void AddResource(GameObject r)
    {
        que.Enqueue(r);
    }
    public void RemoveResource(GameObject r)
    {
        que = new Queue<GameObject>(que.Where(p => p != r));
    }

    public GameObject RemoveResource()
    {
        if(que.Count == 0)
            return null;
        return que.Dequeue();
    }
    #endregion
}


//密封类，不能被继承
public sealed class GWorld 
{
    private static readonly GWorld instance = new();
    private static WorldStates world;
    private static ResourceQueue patients;
    private static ResourceQueue cubicles;
    private static ResourceQueue offices;
    private static ResourceQueue toilets;
    private static ResourceQueue puddles;
    //方便获取
    private static Dictionary<string,ResourceQueue> resources = new();
    public static GWorld Instance { get { return instance; } }
    static GWorld()
    {
        world = new WorldStates();
        patients = new("","",world);
        resources.Add("patients", patients);
        cubicles = new("Cubicle", "FreeCubicle", world);
        resources.Add("cubicles", cubicles);
        offices = new("Office", "FreeOffice", world);
        resources.Add("offices", offices);
        toilets = new("Toilet", "FreeToilet", world);
        resources.Add("toilets", toilets);
        puddles = new("Puddle", "FreePuddle", world);
        resources.Add("puddles", puddles);
        //时间缩放，1.0为正常值，大于1为加速，小于1为减速，0为停止与Update有关的行为
        Time.timeScale = 5;
    }
    public ResourceQueue GetQueue(string type)
    {
        return resources[type];
    }
    private GWorld()
    {
    }
    #region GetWorld() 用于返回世界状态
    public WorldStates GetWorld()
    {
        return world;
    }
    #endregion
}
