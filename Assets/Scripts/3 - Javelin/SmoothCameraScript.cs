using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SmoothCameraScript : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Transform player;
    public Throwable throwable;
    public Text distanceText;
    public JavelinScript javelinscript;
    public Image leftKey;
    public Image rightKey;
    public Image spaceKey;
    public GameObject pauseMenu;
    public bool togglePause;
    public Text newScoreText;
    public Text oldScoreText;
    public Text titleText;
    public Text birdText;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceText.text = javelinscript.distanceTraveled.ToString("F2") + " M";
        if(throwable.toggleOnce == true)
        {
            //target = throwable.weaponInst.transform;
            smoothTime = 0f;
            Vector3 targetPosition = target.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            smoothTime = 0.01f;
            Vector3 targetPosition = target.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        if(Input.GetKeyDown(KeyCode.Escape) && togglePause == false)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            togglePause = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && togglePause == true)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            togglePause = false;
        }

    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            leftKey.GetComponent<Image>().color = new Color32(126, 126, 126, 255);
        }
        else if (!Input.GetKeyDown(KeyCode.A))
        {
            leftKey.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rightKey.GetComponent<Image>().color = new Color32(126, 126, 126, 255);
        }
        else if (!Input.GetKeyDown(KeyCode.D))
        {
            rightKey.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            spaceKey.GetComponent<Image>().color = new Color32(126, 126, 126, 255);
        }
        else if (!Input.GetKeyDown(KeyCode.Space))
        {
            spaceKey.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
