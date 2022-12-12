using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : GAgent
{
    new void Start()
    {
        base.Start();
        SubGoal s1 = new("research", 1, false);
        goals.Add(s1, 1);
        SubGoal s2 = new("relief", 1, false);
        goals.Add(s2, 2);
        SubGoal s3 = new("rested", 1, false);
        goals.Add(s3, 3);
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
