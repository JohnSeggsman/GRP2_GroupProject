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

    [SerializeField] private int aboveCounter;

    private void Start()
    {
        bubblesChance = 95;
        SS = GameObject.Find("StickestMan").GetComponent<SwimmingScript>();
    }

    private void Update()
    {
        if (SS.gameOver)
        {
            if (SS.meterCount >= 94)
            {
                CancelInvoke("SpawningObject");
            }
        }
    }

    private void SpawningObject()
    {
        randChances = Random.Range(0, 101);
        randObs = Random.Range(0, 4);
        randSpawn = Random.Range(0, 2);
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
            Instantiate(Obstacles[randObs], Spawns[randSpawn].transform.position, Quaternion.identity);
            if (randSpawn == 0 && randChances >= bubblesChance)
            {
                Instantiate(Obstacles[4], Spawns[1].transform.position, Quaternion.identity);
            }
        }
        if (aboveCounter >= 2)
        {
            aboveCounter = 0;
            Instantiate(Obstacles[randObs], Spawns[1].transform.position, Quaternion.identity);
            if (randSpawn == 0 && randChances >= bubblesChance)
            {
                Instantiate(Obstacles[4], Spawns[0].transform.position, Quaternion.identity);
            }
        }
    }

}
