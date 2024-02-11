using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Sprite[] fishesSprite;
    [SerializeField] private SwimmingScript SS;

    private int tempSpeed;

    private void Start()
    {
        SS = GameObject.Find("StickestMan").GetComponent<SwimmingScript>();
        if (this.gameObject.CompareTag("Fishes"))
        {
            if (this.transform.position.y == -0.8f)
            {
                this.GetComponent<BoxCollider2D>().size = new Vector2(10.4f, 6.9f);
                this.GetComponent<SpriteRenderer>().sprite = fishesSprite[1];
            }
            else if (this.transform.position.y == -3.35f)
            {
                this.GetComponent<BoxCollider2D>().size = new Vector2(5.0f, 2.9f);
                this.GetComponent<SpriteRenderer>().sprite = fishesSprite[0];
            }
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
