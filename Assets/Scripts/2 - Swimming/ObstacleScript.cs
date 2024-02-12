using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private SwimmingScript SS;

    private int tempSpeed;

    private void Start()
    {
        SS = GameObject.Find("StickestMan").GetComponent<SwimmingScript>();

        if (this.transform.position.y > -1)
        {
            print("Test");
            this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        if (this.transform.position.y < -1)
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 1;
            this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
        }
    }

    private void Update()
    {
        if (SS.LOLBUFF == 2)
        {
            tempSpeed = speed + 10;
        }
        else
        {
            tempSpeed = speed;
        }

        this.transform.position -= new Vector3(tempSpeed * Time.deltaTime, 0, 0);

        if (transform.position.x < -11.4f)
        {
            Destroy(gameObject);
        }
    }

}
