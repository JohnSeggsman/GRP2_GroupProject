using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnerJ : MonoBehaviour
{
    public GameObject SpawnObject;
    //public GameObject[] SpawnObjects1;
    public int random1;
    float PositionY;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnObjects), 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        random1 = Random.Range(0, 3);
    }
    void SpawnObjects()
    {
        PositionY = Random.Range(50, 8f);
        this.transform.position = new Vector3(transform.position.x, PositionY, transform.position.z);
        //Instantiate(SpawnObjects1[random1], transform.position, transform.rotation);
        Instantiate(SpawnObject, transform.position, transform.rotation);
    }
}
