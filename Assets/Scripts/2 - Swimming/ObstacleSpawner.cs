using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Obstacles;
    [SerializeField] private GameObject[] Spawns;
    [SerializeField] private MovementTest MT;
    [SerializeField] private int fishChance;


    private int randChances;
    private int randObs;
    private int randSpawn;

    private int aboveCounter;

    private void Start()
    {
        fishChance = 95;
        InvokeRepeating("SpawningObject", 1.0f, 1.0f);
        MT = GameObject.Find("StickestMan").GetComponent<MovementTest>();
    }

    private void Update()
    {
        if (MT.gameOver)
        {
            CancelInvoke("SpawningObject");
        }
    }

    private void SpawningObject()
    {
        randChances = Random.Range(0, 101);
        randObs = Random.Range(0, 2);
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
            Debug.Log(randChances);
            if (randSpawn == 0 && randChances >= fishChance)
            {
                Instantiate(Obstacles[2], Spawns[1].transform.position, Quaternion.identity);
            }
            else if (randSpawn == 1 && randChances >= fishChance)
            {
                Instantiate(Obstacles[2], Spawns[0].transform.position, Quaternion.identity);
            }
        }
        if (aboveCounter >= 2)
        {
            randSpawn = 1;
            aboveCounter = 0;
            Instantiate(Obstacles[randObs], Spawns[randSpawn].transform.position, Quaternion.identity);
            Debug.Log(randChances);
            if (randSpawn == 0 && randChances >= fishChance)
            {
                Instantiate(Obstacles[2], Spawns[1].transform.position, Quaternion.identity);
            }
            else if (randSpawn == 1 && randChances >= fishChance)
            {
                Instantiate(Obstacles[2], Spawns[0].transform.position, Quaternion.identity);
            }
        }
    }

}
