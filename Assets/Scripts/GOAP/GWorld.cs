using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceQueue
{
    public Queue<GameObject> que = new();
    public string tag;
    public string modState;
    #region �� ���Tag�Ķ��� ��������ӵ���ǰ����״̬
    public ResourceQueue(string t,string ms,WorldStates w)
    {
        tag = t;
        modState = ms;
        if (tag != "")
        {
            //���Ҳ���ȡ����ӵ�и� Tag �Ķ���
            GameObject[] resources =GameObject.FindGameObjectsWithTag(tag);
            foreach(GameObject r in resources)
            {
                que.Enqueue(r);
            }
        }
        //������״̬����Ӹ� ״̬ ��������ֵ
        if (modState != "")
        {
            w.ModifyState(modState, que.Count);
        }
    }
    #endregion

    #region ������״̬����Ӻ��Ƴ� �ö���
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


//�ܷ��࣬���ܱ��̳�
public sealed class GWorld 
{
    private static readonly GWorld instance = new();
    private static WorldStates world;
    private static ResourceQueue patients;
    private static ResourceQueue cubicles;
    private static ResourceQueue offices;
    private static ResourceQueue toilets;
    private static ResourceQueue puddles;
    //�����ȡ
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
        //ʱ�����ţ�1.0Ϊ����ֵ������1Ϊ���٣�С��1Ϊ���٣�0Ϊֹͣ��Update�йص���Ϊ
        Time.timeScale = 5;
    }
    public ResourceQueue GetQueue(string type)
    {
        return resources[type];
    }
    private GWorld()
    {
    }
    #region GetWorld() ���ڷ�������״̬
    public WorldStates GetWorld()
    {
        return world;
    }
    #endregion
}
