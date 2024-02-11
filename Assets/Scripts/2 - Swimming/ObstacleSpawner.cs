using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Obstacles;
    [SerializeField] private GameObject[] Spawns;
    [SerializeField] private SwimmingScript SS;
    [SerializeField] private int bubblesChance;

    private int randChances;
    private int randObs;
    private int randSpawn;

    private int aboveCounter;

    private void Start()
    {
        bubblesChance = 95;
        InvokeRepeating("SpawningObject", 1.0f, 1.0f);
        SS = GameObject.Find("StickestMan").GetComponent<SwimmingScript>();
    }

    private void Update()
    {
        if (SS.gameOver || SS.meterCount >= 94)
        {
            CancelInvoke("SpawningObject");
        }

        //if (SS.LOLBUFF == 1)
        //{
        //    CancelInvoke("SpawningObject");
        //    InvokeRepeating("SpawningObject", 1.0f, 1.0f);
        //}
        //else
        //{
        //    CancelInvoke("SpawningObject");
        //    InvokeRepeating("SpawningObject", 0.5f, 0.5f);
        //}
    }

    private void SpawningObject()
    {
        randChances = Random.Range(0, 101);
        randObs = Random.Range(0, 5);
        randSpawn = Random.Range(0, 1);
        if (randSpawn == 0)
        {
            aboveCounter++;
        }
        else if (randSpawn == 1)
        {
            aboveCounter = 0;
        }
        if (aboveCounter < 2)
        {
            Instantiate(Obstacles[3], Spawns[randSpawn].transform.position, Quaternion.identity);
            if (randSpawn == 0 && randChances >= bubblesChance)
            {
                Instantiate(Obstacles[4], Spawns[1].transform.position, Quaternion.identity);
            }
        }
        if (aboveCounter >= 2)
        {
            randSpawn = 1;
            aboveCounter = 0;
            Instantiate(Obstacles[3], Spawns[randSpawn].transform.position, Quaternion.identity);
            if (randSpawn == 0 && randChances >= bubblesChance)
            {
                Instantiate(Obstacles[4], Spawns[1].transform.position, Quaternion.identity);
            }
        }
    }

}
