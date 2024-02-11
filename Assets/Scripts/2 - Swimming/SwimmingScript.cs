using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Sprites;

public class SwimmingScript : MonoBehaviour
{
    [SerializeField] private GameObject mainCam, winCam;
    [SerializeField] private float timer;
    [SerializeField] private float currenttimerCount, highesttimerCount;
    [SerializeField] private GameObject meterBox, timerBox;
    [SerializeField] private Text meterTxt, timerTxt, highScoreTxt, currentScoreTxt;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private GameObject BubbleSprite;
    [SerializeField] private Sprite[] Bubbles;
    [SerializeField] private GameObject backgroundScroll;
    [SerializeField] private GameObject LoseAnim, WinAnim;
    [SerializeField] private AudioManager audioSource;
    [SerializeField] private AudioClip[] audioClips;

    [SerializeField] private Button[] buttons; //MainMenuScene
    [SerializeField] private GameObject PauseMenu;

    [SerializeField] private bool IMMORTALITY;
    [SerializeField] private GameObject endPoint, transition, transMask;
    [SerializeField] private Animator animator, endAnimator;

    private Vector3 oriPosBack, oriPosEnd;
    private ObstacleSpawner OS;
    private SpriteRenderer SR;
    public int meterCount;
    public float LOLBUFF;
    private float AIR;
    private bool isUnder;
    public bool gameOver;
    public bool isWon;
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
        audioSource.audioBGM.loop = true;
        mainCam.SetActive(true);
        winCam.SetActive(false);

        oriPosBack = new Vector3(3.5f, -1, 0);
        oriPosEnd = new Vector3(18.8f, -2.59f, 0);

        backgroundScroll.transform.position = oriPosBack;
        endPoint.transform.position = oriPosEnd;
    }

    #region StartSetUp

    private void VariableSetUp()
    {
        isPaused = false;
        isWon = false;
        isUnder = false;
        gameOver = false;
        currenttimerCount = 0;
        LOLBUFF = 1;
        AIR = 100;
        timer = 0;
        transform.position = new Vector3(-5.55f, -0.7f, 0);
    }

    private void ReferenceSetUp()
    {
        if (GameObject.Find("AudioManager") == null)
        {
            audioSource = GameObject.Find("TempAudioManager").GetComponent<AudioManager>();
        }
        else
        {
            audioSource = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }


        playerCollider = this.GetComponent<BoxCollider2D>();
        SR = this.GetComponent<SpriteRenderer>();
        OS = GameObject.Find("ObstacleManager").GetComponent<ObstacleSpawner>();
        endPoint = GameObject.Find("EndPoint");
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
        if (!gameOver)
        {
            if (!isPaused)
            {
                InputFunctions();
                OxygenFunction();
                MovementFunction();
                TimerFunction();
                EndPointFunction();
            }
        }
    }

    private void InputFunctions()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            meterCount += 50;
        }
        #endif

        if (Input.GetKey(KeyCode.Space))
        {
            LOLBUFF = 2.0f;
            animator.speed = 1.5f;
            audioSource.audioBGM.pitch = 1.5f;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            LOLBUFF = 1.0f;
            animator.speed = 1.0f;
            audioSource.audioBGM.pitch = 1.0f;
        }

        #region Pause Menu
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
        #endregion
    }

    private void TimerFunction()
    {
        meterTxt.text = meterCount + "M" + " / " + "100M";

        timerTxt.text = currenttimerCount.ToString("00:##.##");

        currenttimerCount += Time.deltaTime;

        if (backgroundScroll.transform.position.x > -3.5)
        {
            backgroundScroll.transform.position -= new Vector3(((0.15f * LOLBUFF) * Time.deltaTime), 0, 0);
        }

        if (meterCount >= 100)
        {

        }
        else
        {
            if (timer < 1)
            {
                timer += Time.deltaTime;
            }
            else if (timer >= 1)
            {
                if (LOLBUFF == 1)
                {
                    meterCount += 2;
                }
                else
                {
                    meterCount += 4;
                }
                timer = 0;
            }
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
                AIR += 20 * Time.deltaTime;
            }
        }
    }

    private void MovementFunction()
    {
        if (Input.GetKey(KeyCode.S))
        {
            SR.sortingOrder = 1;
            animator.SetBool("isDive", true);
            transform.position = new Vector3(transform.position.x, -3, 0);
            //playerCollider.size = new Vector2(1.5f, 3.5f);
            BubbleSprite.SetActive(true);
            isUnder = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            audioSource.audioBGM.clip = audioClips[1];
            audioSource.audioBGM.Play();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            SR.sortingOrder = 0;
            animator.SetBool("isDive", false);
            transform.position = new Vector3(transform.position.x, -0.7f, 0);
            //playerCollider.size = new Vector2(5.3f, 2.6f);
            BubbleSprite.SetActive(false);
            audioSource.audioBGM.clip = audioClips[0];
            audioSource.audioBGM.Play();
            isUnder = false;
        }
    }

    private void EndPointFunction()
    {
        if (endPoint.transform.position.x >= 7.0f)
        {
            if (meterCount >= 96)
            {
                endPoint.transform.position -= new Vector3((5 * Time.deltaTime), 0, 0);
            }
        }
        else if (endPoint.transform.position.x <= 7.0f)
        {
            if (!isWon)
            {
                if (highesttimerCount <= 0)
                {
                    highesttimerCount = currenttimerCount;
                    highScoreTxt.text = highesttimerCount.ToString("00:##.##");
                    currentScoreTxt.text = currenttimerCount.ToString("00:##.##");
                }
                else if (highesttimerCount > currenttimerCount)
                {
                    highesttimerCount = currenttimerCount;
                    highScoreTxt.text = highesttimerCount.ToString("00:##.##");
                    currentScoreTxt.text = currenttimerCount.ToString("00:##.##");
                }
                else if (highesttimerCount < currenttimerCount)
                {
                    highScoreTxt.text = highesttimerCount.ToString("00:##.##");
                    currentScoreTxt.text = currenttimerCount.ToString("00:##.##");
                }

                animator.speed = 1.0f;
                Time.timeScale = 1.0f;
                audioSource.audioBGM.pitch = 1.0f;
                gameOver = true;
                isWon = true;
                meterBox.SetActive(false);
                timerBox.SetActive(false);
                audioSource.audioBGM.loop = false;
                audioSource.StopBGM();
                StartCoroutine(endPointAnimation());
            }
            else
            {
                Debug.Log("Your mother");
            }
        }
    }

    private IEnumerator endPointAnimation()
    {
        endAnimator.SetTrigger("endPlay");
        yield return new WaitForSeconds(0.75f);
        endAnimator.SetTrigger("endEnd");
        mainCam.SetActive(false);
        winCam.SetActive(true);
        audioSource.audioBGM.clip = audioClips[3];
        yield return new WaitForSeconds(0.1f);
        audioSource.audioBGM.PlayOneShot(audioSource.audioBGM.clip);
        WinAnim.SetActive(true);
    }

    private void OnResume()
    {
        LOLBUFF = 1.0f;
        isPaused = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        audioSource.audioBGM.Play();
    }

    private void OnRetry()
    {
        GameObject destroyObj = GameObject.FindGameObjectWithTag("Obstacle");
        Destroy(destroyObj);
        GameObject destroyDude = GameObject.FindGameObjectWithTag("ObstacleDude");
        Destroy(destroyDude);
        OS.CancelInvoke("SpawningObject");
        OS.InvokeRepeating("SpawningObject", 1.0f, 1.0f);
        endPoint.transform.position = oriPosEnd;
        backgroundScroll.transform.position = oriPosBack;
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
        if (collision.gameObject.CompareTag("Obstacle") && !gameOver && !IMMORTALITY)
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
        if (collision.gameObject.CompareTag("ObstacleDude") && !gameOver && !IMMORTALITY)
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
        }
        if (collision.gameObject.CompareTag("Fishes") && !gameOver)
        {
            AIR += 20;
            Destroy(collision.gameObject);
        }
    }
}
