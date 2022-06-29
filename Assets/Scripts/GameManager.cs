using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;

    Vector3 pos = new Vector3();

    void Start()
    {
        Instantiate(enemy);
        StartCoroutine("TimeToSpawn");
    }

    IEnumerator TimeToSpawn()
    {
        for (float i = 5; i > 0; i--)
        {
            yield return new WaitForSeconds(1f); 
        }
        Instantiate(enemy);
        StartCoroutine("TimeToSpawn");
    }
}
