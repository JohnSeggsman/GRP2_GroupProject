using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundObjs;
    [SerializeField] private GameObject[] Spawns;
    [SerializeField] private SwimmingScript MT;

    private int randObs;
    private int randSpawn;

    private void Start()
    {
        InvokeRepeating("SpawningBackground", 5.0f, 2.5f);
        MT = GameObject.Find("StickestMan").GetComponent<SwimmingScript>();
    }

    private void Update()
    {
        if (MT.gameOver)
        {
            CancelInvoke("SpawningObject");
        }
    }

    private void SpawningBackground()
    {
        randObs = Random.Range(0, 2);
        randSpawn = Random.Range(0, 2);

        Instantiate(backgroundObjs[randObs], Spawns[1].transform.position, Quaternion.identity);
    }
}
