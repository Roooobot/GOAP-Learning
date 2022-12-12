using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse : GAgent
{
    new void Start()
    {
        base.Start();
        //false 表示完成目标后不会删除目标，会一直循环
        SubGoal s1 = new("treatPatient", 1, false);
        goals.Add(s1, 3);
        SubGoal s2 = new("rested", 1, false);
        goals.Add(s2, 1);
        SubGoal s3 = new("relief", 1, false);
        goals.Add(s3, 5);

        Invoke(nameof(GetTired), Random.Range(10, 20));
        Invoke(nameof(NeedRelief), Random.Range(10, 20));
    }
    void GetTired()
    {
        beliefs.ModifyState("exhausted", 0);
        Invoke(nameof(GetTired), Random.Range(10, 20));
    }
    void NeedRelief()
    {
        beliefs.ModifyState("busting", 0);
        Invoke(nameof(NeedRelief), Random.Range(10, 20));
    }

}
