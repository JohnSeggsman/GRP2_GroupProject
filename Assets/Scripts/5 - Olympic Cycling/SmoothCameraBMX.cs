using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SmoothCameraBMX : MonoBehaviour
{
    public BikeMovementScript bikemovementscript;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    public float timerStuff;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI distanceText;
    public Transform player;

    private Vector2 startPosition;

    public Image staminaImage;

    public GameObject instructionCanvas;

    public bool togglePause;
    public GameObject pauseMenu;

    public bool toggleOnce;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = player.position;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        smoothTime = 0.1f;
        Vector3 targetPosition = target.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        timerStuff += Time.deltaTime;
        Vector2 distance = (Vector2)player.position - startPosition;
        distance.y = 0f;

        if(distance.x < 0)
        {
            distance.x = 0;
        }

        distanceText.text = distance.x.ToString("F0") + "M";
        timerText.text = timerStuff.ToString("F1") + "s";
        UpdateUI();

        if(Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            instructionCanvas.SetActive(false);
        }

        if(distance.x >= 200 && toggleOnce == false)
        {
            // player win
            Time.timeScale = 0;
            bikemovementscript.winGame = true;
            toggleOnce = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && togglePause == false)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            togglePause = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && togglePause == true)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            togglePause = false;
        }
    }
    void UpdateUI()
    {
        staminaImage.fillAmount = (bikemovementscript.currentStaminaAmount / bikemovementscript.maxStaminaAmount);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        //AudioManager temp = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        //temp.PlayBGM(0);
        SceneManager.LoadScene(0);
    }
}
