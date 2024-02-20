using UnityEngine;

public class DashScrollScript : MonoBehaviour
{
    void Update()
    {
        transform.localPosition += new Vector3((DashMinigameManager.Instance.isRunning ? -DashMinigameManager.Instance.Speed : 0f) * Time.deltaTime, 0);
        if (transform.localPosition.x <= -22.5f)
            transform.localPosition = new Vector3(76.6f, 0);
    }
}