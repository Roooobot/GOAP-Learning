using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//密封类，不能被继承
public sealed class GWorld 
{
    private static readonly GWorld instance = new();
    private static WorldStates world;
    private static Queue<GameObject> paients;
    private static Queue<GameObject> cubicles;
    public static GWorld Instance { get { return instance; } }
    static GWorld()
    {
        world = new WorldStates();
        paients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach (GameObject c in cubes)
            cubicles.Enqueue(c);

        if (cubes.Length > 0)
        {
            world.ModifyState("FreeCubicle",cubes.Length);
        }
    }
    private GWorld()
    {

    }
    public void AddPatient(GameObject p)
    {
        paients.Enqueue(p);
    }
    public GameObject RemovePatient()
    {
        if(paients.Count == 0)
            return null;
        return paients.Dequeue();
    }
    public void AddCubicle(GameObject p)
    {
        paients.Enqueue(p);
    }
    public GameObject RemoveCubicle()
    {
        if (cubicles.Count == 0)
            return null;
        return cubicles.Dequeue();
    }

    #region GetWorld() 用于返回世界状态
    public WorldStates GetWorld()
    {
        return world;
    }
    #endregion
}
