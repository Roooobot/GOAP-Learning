using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Resource Data"), fileName= ("resourcedata"),order =51)]
public class ResourceData : ScriptableObject
{
    public string resourceTag;
    public string resourceQueue;
    public string resourceState;
}
