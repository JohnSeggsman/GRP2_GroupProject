using UnityEngine;

public class ObstaclesScrollScript : MonoBehaviour
{
    [Header("Minigame Reference - DO NOT TOUCH!")]
    public string ObstacleType;

    void Update()
    {
        transform.position += new Vector3((DashMinigameManager.Instance.isRunning ? -DashMinigameManager.Instance.Speed : 0f) * Time.deltaTime, 0);
        if (transform.position.x <= -12.5f)
            Destroy(gameObject);

        if (transform.position.x <= 0)
            transform.GetComponent<SpriteRenderer>().sortingOrder = 69420;
        else
            transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (ObstacleType == "Obstacles")
            {
                DashMinigameManager.Instance.isKnockedOut = true;
                StartCoroutine(DashMinigameManager.Instance.RaceCompleted("Failure"));
                GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(DashMinigameManager.Instance.MinigameSFX[2]);
            }
            else
            {
                StartCoroutine(DashMinigameManager.Instance.RaceCompleted("Victory"));
            }
        }
    }
}