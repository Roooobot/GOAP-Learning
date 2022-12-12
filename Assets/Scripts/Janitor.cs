using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : GAgent
{
    new void Start()
    {
        base.Start();
        //false 表示完成目标后不会删除目标，会一直循环
        SubGoal s1 = new("clean", 1, false);
        goals.Add(s1, 3);
    }

}