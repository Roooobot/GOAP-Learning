using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : GAgent
{
    new void Start()
    {
        base.Start();
        //false ��ʾ���Ŀ��󲻻�ɾ��Ŀ�꣬��һֱѭ��
        SubGoal s1 = new("clean", 1, false);
        goals.Add(s1, 3);
    }

}