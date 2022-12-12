using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject patientPrefab;
    public int numPaients;
    public bool keepSpawning = false;

    private void Start()
    {
        for(int i= 0; i < numPaients; i++)
        {
            Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        }
        if (keepSpawning)
        {
            Invoke(nameof(Spawnpatient), 5);
        }
    }

    void Spawnpatient()
    {
        Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        Invoke(nameof(Spawnpatient), Random.Range(2, 10));
    }
}
