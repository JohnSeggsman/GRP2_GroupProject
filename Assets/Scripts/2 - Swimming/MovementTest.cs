using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Sprites;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private float AIR;
    [SerializeField] private float timer;
    [SerializeField] private Text meterTxt;
    [SerializeField] private bool isUnder;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private GameObject BubbleSprite;
    [SerializeField] private Sprite[] Bubbles;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private GameObject LoseAnim;
    [SerializeField] private AudioManager audioSource;

    [SerializeField] private Button[] buttons; //MainMenuScene
    [SerializeField] private GameObject PauseMenu;

    private ObstacleSpawner OS;
    private SpriteRenderer SR;
    private Animator animator;
    private int meterCount;

    public bool gameOver;
    private bool isPaused;

    private void Start()
    {
        ButtonSetUp();
        ReferenceSetUp();
        VariableSetUp();
        BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[1];
        BubbleSprite.SetActive(false);
        audioSource.audioBGM.clip = audioClips[0];
        audioSource.audioBGM.Play();
    }

    #region StartSetUp

    private void VariableSetUp()
    {
        isPaused = false;
        isUnder = false;
        gameOver = false;
        AIR = 100;
        timer = 0;
        transform.position = new Vector3(-5.55f, -0.7f, 0);
    }

    private void ReferenceSetUp()
    {
        playerCollider = this.GetComponent<BoxCollider2D>();
        SR = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        OS = GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawner>();
    }

    private void ButtonSetUp()
    {
        buttons[0].onClick.AddListener(OnRetry);
        buttons[1].onClick.AddListener(OnMainMenu);
        buttons[2].onClick.AddListener(OnResume);
        buttons[3].onClick.AddListener(OnRetry);
        buttons[4].onClick.AddListener(OnOption);
        buttons[5].onClick.AddListener(OnMainMenu);
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isPaused)
        {
            PauseMenu.SetActive(true);
            isPaused = true;
            audioSource.audioBGM.Pause();
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.R) && isPaused)
        {
            PauseMenu.SetActive(false);
            isPaused = false;
            audioSource.audioBGM.Play();
            Time.timeScale = 1;
        }

        if (!gameOver)
        {
            if (!isPaused)
            {
                OxygenFunction();
                MovementFunction();
                TimerFunction();
            }
        }
    }

    private void TimerFunction()
    {
        meterTxt.text = meterCount + "M" + " / " + "1000M";
        if (timer < 5)
        {
            timer += Time.deltaTime;
        }
        else if (timer >= 5)
        {
            meterCount += 50;
            timer = 0;
        }
    }

    private void OxygenFunction()
    {

        #region Oxygen Sprites
        if (AIR > 100)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[1];
        }
        else if (AIR < 90 && AIR > 80)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[2];
        }
        else if (AIR < 80 && AIR > 70)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[3];
        }
        else if (AIR < 70 && AIR > 60)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[4];
        }
        else if (AIR < 60 && AIR > 50)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[5];
        }
        else if (AIR < 50 && AIR > 40)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[6];
        }
        else if (AIR < 40 && AIR > 30)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[7];
        }
        else if (AIR < 30 && AIR > 20)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[8];
        }
        else if (AIR < 20 && AIR > 10)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[9];
        }
        else if (AIR < 10 && AIR > 0)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[10];
        }
        else if (AIR < 0)
        {
            BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[0];
        }
        #endregion

        if (isUnder && AIR > 0)
        {
            AIR -= 20 * Time.deltaTime;
        }
        else if (!isUnder)
        {
            if (AIR >= 100)
            {
                AIR = 100;
            }
            else if (AIR <= 100)
            {
                AIR += 10 * Time.deltaTime;
            }
        }
    }

    private void MovementFunction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SR.sortingOrder = 1;
            animator.SetBool("isDive", true);
            transform.position = new Vector3(transform.position.x, -3, 0);
            playerCollider.size = new Vector2(1.5f, 3.5f);
            BubbleSprite.SetActive(true);
            isUnder = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.audioBGM.clip = audioClips[1];
            audioSource.audioBGM.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            SR.sortingOrder = 0;
            animator.SetBool("isDive", false);
            transform.position = new Vector3(transform.position.x, -0.7f, 0);
            playerCollider.size = new Vector2(5.3f, 2.6f);
            BubbleSprite.SetActive(false);
            audioSource.audioBGM.clip = audioClips[0];
            audioSource.audioBGM.Play();
            isUnder = false;
        }
    }

    private void OnResume()
    {
        Debug.Log("Your mother");
    }

    private void OnRetry()
    {
        OS.InvokeRepeating("SpawningObject", 1.0f, 1.0f);
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        AIR = 100;
        meterCount = 0;
        isUnder = false;
        gameOver = false;
        isPaused = false;
        SR.sortingOrder = 0;
        audioSource.audioBGM.clip = audioClips[0];
        audioSource.audioBGM.Play();
        animator.SetBool("isDead", false);
        LoseAnim.SetActive(false);
    }

    private void OnOption()
    {
        Debug.Log("Your father");
    }

    private void OnMainMenu()
    {
        Time.timeScale = 1;
        audioSource.audioBGM.clip = audioClips[2];
        audioSource.audioBGM.Play();
        SceneManager.LoadScene("MainMenuScene");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !gameOver)
        {
            LoseAnim.SetActive(true);
            audioSource.audioBGM.clip = audioClips[1];
            audioSource.audioBGM.Play();
            animator.SetBool("isDive", false);
            animator.SetBool("isDead", true);
            BubbleSprite.SetActive(false);
            SR.sortingOrder = 1;
            gameOver = true;
            transform.position = new Vector3(-5.55f, -1.65f, 0);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Fishes") && !gameOver)
        {
            AIR += 20;
            Destroy(collision.gameObject);
        }
    }
}
