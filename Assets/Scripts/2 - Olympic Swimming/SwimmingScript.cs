using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Sprites;
using TMPro;

public class SwimmingScript : MonoBehaviour
{
    [Header("Game Object References")]
    [SerializeField] private GameObject BubbleSprite;
    [SerializeField] private GameObject mainCam, winCam;
    [SerializeField] private GameObject meterBox, timerBox;
    [SerializeField] private GameObject endPoint, transition, transMask;
    [SerializeField] private Image spriteWin;

    [Header("Array References")]
    [SerializeField] private Sprite[] Bubbles;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private Button[] buttons; //MainMenuScene
    [SerializeField] private Sprite[] winSprites;

    [Header("UI References")]
    [SerializeField] private GameObject instructionUI;
    [SerializeField] private TextMeshProUGUI meterTxt, timerTxt, highScoreTxt, currentScoreTxt, avoidedText, divedText;
    [SerializeField] private GameObject currentM, currentT;
    [SerializeField] private GameObject PauseMenu, WTDManual;
    [SerializeField] private GameObject backgroundScroll;
    [SerializeField] private TextMeshProUGUI WRText, ORText;

    [Header("Miscs")]
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private Animator animator, endAnimator, recordAnimator;
    [SerializeField] private GameObject LoseAnim, WinAnim;

    [Header("Cheats")]
    [SerializeField] private bool IMMORTALITY;
    private float currenttimerCount, highesttimerCount, secondhighCount;
    private Vector3 oriPosBack, oriPosEnd;
    private AudioManager audioSource;
    private ObstacleSpawner OS;
    private PlapAnim PA;
    private SpriteRenderer SR;
    private bool isPaused;
    private bool isUnder;
    private float timer;
    private float AIR;
    private float WR;
    private float OR;

    [HideInInspector] public int timeDived;
    [HideInInspector] public int objectAvoided;
    [HideInInspector] public int meterCount;
    [HideInInspector] public float LOLBUFF;
    [HideInInspector] public bool gameOver;
    [HideInInspector] public bool isWon;
    [HideInInspector] public bool isStart;

    private bool pressedA;
    private bool pressedD;
    private bool isMoving;
    private bool isSwimming;
    private bool isSwimming2;

    [SerializeField] private float movingCount;

    private void Start()
    {
        ButtonSetUp();
        ReferenceSetUp();
        VariableSetUp();
        Instructions();
        BubbleSprite.GetComponent<SpriteRenderer>().sprite = Bubbles[1];
        BubbleSprite.SetActive(false);
        mainCam.SetActive(true);
        winCam.SetActive(false);
        oriPosBack = new Vector3(3.5f, -1, 0);
        oriPosEnd = new Vector3(18.8f, -2.59f, 0);
        audioSource.audioBGM.Stop();
        backgroundScroll.transform.position = oriPosBack;
        endPoint.transform.position = oriPosEnd;
    }

    private void Instructions()
    {
        instructionUI.SetActive(true);
        Time.timeScale = 0;
    }

    #region StartSetUp

    private void VariableSetUp()
    {
        isPaused = false;
        isStart = false;
        isWon = false;
        isUnder = false;
        gameOver = false;
        pressedA = false;
        pressedD = false;
        isMoving = false;
        timeDived = 0;
        objectAvoided = 0;
        PA.Plapping = 0;
        WR = 40.0f;
        OR = 43.0f;
        WRText.text = WR.ToString();
        ORText.text = OR.ToString();
        currenttimerCount = 0;
        LOLBUFF = 1;
        AIR = 100;
        timer = 0;
        transform.position = new Vector3(-5.55f, -0.7f, 0);
    }

    private void ReferenceSetUp()
    {
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        playerCollider = this.GetComponent<BoxCollider2D>();
        SR = this.GetComponent<SpriteRenderer>();
        OS = GameObject.Find("ObstacleManager").GetComponent<ObstacleSpawner>();
        PA = GameObject.Find("Coach").GetComponent<PlapAnim>();
        endPoint = GameObject.Find("EndPoint");
    }

    private void ButtonSetUp()
    {
        buttons[0].onClick.AddListener(OnRetry);
        buttons[1].onClick.AddListener(OnMainMenu);
        buttons[2].onClick.AddListener(OnResume);
        buttons[3].onClick.AddListener(OnRetry);
        buttons[4].onClick.AddListener(OnWTD);
        buttons[5].onClick.AddListener(OnMainMenu);
        buttons[6].onClick.AddListener(OnRetry);
        buttons[7].onClick.AddListener(OnMainMenu);
    }

    #endregion

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) && !isStart)
        {
            recordAnimator.SetTrigger("RecordShow");
            Time.timeScale = 1;
            isStart = true;
            isPaused = false;
            instructionUI.SetActive(false);
            OS.InvokeRepeating("SpawningObject", 2.0f, 2.0f);
            audioSource.audioBGM.clip = audioClips[4];
            audioSource.audioBGM.Play();
            audioSource.audioBGM.loop = true;
        }

        if (!gameOver && isStart)
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
        if (Input.GetKeyDown(KeyCode.A) && !pressedA && !isUnder)
        {
            pressedA = true;
            pressedD = false;
            isMoving = true;
            isSwimming = true;
            if (movingCount > 0.25f && movingCount < 0.45f)
            {
                movingCount = 0.0f;
                LOLBUFF = 1.5f;
            }
            else if (movingCount < 0.25f)
            {
                movingCount = 0.0f;
                LOLBUFF = 1.0f;
            }
            else if (movingCount > 0.45f)
            {
                movingCount = 0.0f;
                LOLBUFF = 1.0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && !pressedD && !isUnder)
        {
            pressedA = false;
            pressedD = true;
            isMoving = true;
            isSwimming = true;
            if (movingCount > 0.25f && movingCount < 0.45f)
            {
                movingCount = 0.0f;
                LOLBUFF = 1.5f;
            }
            else if (movingCount < 0.25f)
            {
                movingCount = 0.0f;
                LOLBUFF = 1.0f;
            }
            else if (movingCount > 0.45f)
            {
                movingCount = 0.0f;
                LOLBUFF = 1.0f;
            }
        }
        if (isSwimming)
        {
            if (!isSwimming2)
            {
                audioSource.audioBGM.clip = audioClips[0];
                audioSource.audioBGM.Play();
                isSwimming = false;
                isSwimming2 = true;
            }
        }

        if (isMoving)
        {
            animator.SetBool("isSwimming", true);
            if (movingCount < 0.80f)
            {
                movingCount += Time.deltaTime;
            }
            else if (movingCount >= 0.80f)
            {
                if (!isUnder)
                {
                    audioSource.audioBGM.clip = audioClips[4];
                    audioSource.audioBGM.Play();
                }
                isMoving = false;
            }
        }
        if (!isMoving)
        {
            isSwimming = false;
            isSwimming2 = false;
            animator.SetBool("isSwimming", false);
        }



        #region Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            PauseMenu.SetActive(true);
            isPaused = true;
            audioSource.audioBGM.Pause();
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
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

        timerTxt.text = currenttimerCount.ToString("00:00");

        currenttimerCount += Time.deltaTime;

        if (backgroundScroll.transform.position.x > -3.5 && isMoving)
        {
            backgroundScroll.transform.position -= new Vector3(((0.15f * LOLBUFF) * Time.deltaTime), 0, 0);
        }

        if (meterCount <= 100 && isMoving)
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

        if (AIR <= 0 && !IMMORTALITY)
        {
            GameLose();
        }
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
                AIR += 30 * Time.deltaTime;
            }
        }
    }

    private void MovementFunction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SR.sortingOrder = 1;
            animator.SetBool("isDive", true);
            animator.SetBool("isSwimming", false);
            transform.position = new Vector3(transform.position.x, -3, 0);
            //playerCollider.size = new Vector2(1.5f, 3.5f);
            BubbleSprite.SetActive(true);
            isUnder = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSwimming = false;
            isSwimming2 = false;
            timeDived++;
            audioSource.audioBGM.clip = audioClips[1];
            audioSource.audioBGM.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            SR.sortingOrder = 0;
            animator.SetBool("isDive", false);
            animator.SetBool("isSwimming", false);
            transform.position = new Vector3(transform.position.x, -0.7f, 0);
            //playerCollider.size = new Vector2(5.3f, 2.6f);
            BubbleSprite.SetActive(false);
            audioSource.audioBGM.clip = audioClips[4];
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
                RecordAdjustment();
                animator.speed = 1.0f;
                Time.timeScale = 1.0f;
                audioSource.audioBGM.pitch = 1.0f;
                gameOver = true;
                isWon = true;
                meterBox.SetActive(false);
                timerBox.SetActive(false);
                currentM.SetActive(false);
                currentT.SetActive(false);
                audioSource.audioBGM.loop = false;
                audioSource.StopBGM();
                StartCoroutine(endPointAnimation());
            }
            else
            {
                
            }
        }
    }

    private void RecordAdjustment()
    {
        if (highesttimerCount <= 0)
        {
            if (currenttimerCount < WR)
            {
                spriteWin.sprite = winSprites[0];
                highesttimerCount = currenttimerCount;
                RecordSettings(2);
            }
            else if (currenttimerCount < OR)
            {
                spriteWin.sprite = winSprites[0];
                highesttimerCount = currenttimerCount;
                RecordSettings(3);
            }
            else if (currenttimerCount > OR && currenttimerCount > WR)
            {
                spriteWin.sprite = winSprites[0];
                highesttimerCount = currenttimerCount;
                RecordSettings(1);
            }
        }
        else if (currenttimerCount < highesttimerCount) // If fastest time is slower than current time
        {
            if (currenttimerCount < WR && currenttimerCount < OR)
            {
                spriteWin.sprite = winSprites[0];
                WR = currenttimerCount;
                highesttimerCount = currenttimerCount;
                RecordSettings(2);
            }
            else if (currenttimerCount > OR && currenttimerCount > WR)
            {
                spriteWin.sprite = winSprites[0];
                highesttimerCount = currenttimerCount;
                RecordSettings(1);
            }
        }
        else if (currenttimerCount > highesttimerCount) // If fastest time is faster than current time
        {
            if (currenttimerCount < OR)
            {
                spriteWin.sprite = winSprites[0];
                OR = currenttimerCount;
                RecordSettings(3);
            }
            else if (currenttimerCount > OR && currenttimerCount > highesttimerCount)
            {
                if (secondhighCount <= 0)
                {
                    spriteWin.sprite = winSprites[1];
                    secondhighCount = currenttimerCount;
                    RecordSettings(1);
                }
                else if (currenttimerCount > secondhighCount) // if second fastest time is faster than current time
                {
                    spriteWin.sprite = winSprites[2];
                    RecordSettings(1);
                }
                else if (currenttimerCount < secondhighCount)
                {
                    spriteWin.sprite = winSprites[1];
                    secondhighCount = currenttimerCount;
                    RecordSettings(1);
                }
            }
        }
    }

    private void RecordSettings(int num)
    {
        avoidedText.text = "Objects Avoided: " + objectAvoided.ToString();
        divedText.text = "Dived Amount: " + timeDived.ToString();

        if (num == 1)
        {
            highScoreTxt.text = "Fastest Record: " + highesttimerCount.ToString("00:##.##");
            currentScoreTxt.text = "Current Record: " + currenttimerCount.ToString("00:##.##");
        }
        else if (num == 2)
        {
            highScoreTxt.text = "NEW WORLD RECORD!";
            currentScoreTxt.text = "Current Record: " + currenttimerCount.ToString("00:##.##");
        }
        else if (num == 3)
        {
            highScoreTxt.text = "NEW OLYMPIC RECORD!";
            currentScoreTxt.text = "Current Record: " + currenttimerCount.ToString("00:##.##");
        }
    }

    private IEnumerator endPointAnimation()
    {
        endAnimator.SetTrigger("endPlay");
        yield return new WaitForSeconds(0.75f);
        mainCam.SetActive(false);
        winCam.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        endAnimator.SetTrigger("endEnd");
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
        WTDManual.SetActive(false);
        Time.timeScale = 1;
        audioSource.audioBGM.Play();
    }

    private void OnRetry()  
    {
        GameObject destroyObj = GameObject.FindGameObjectWithTag("Obstacle");
        GameObject destroyDude = GameObject.FindGameObjectWithTag("ObstacleDude");
        Destroy(destroyObj);
        Destroy(destroyDude);
        OS.CancelInvoke("SpawningObject");
        OS.InvokeRepeating("SpawningObject", 2.0f, 2.0f);
        endPoint.transform.position = oriPosEnd;
        backgroundScroll.transform.position = oriPosBack;
        meterBox.SetActive(true);
        timerBox.SetActive(true);
        currentM.SetActive(true);
        currentT.SetActive(true);
        PauseMenu.SetActive(false);
        WTDManual.SetActive(false);
        WinAnim.SetActive(false);
        LoseAnim.SetActive(false);
        winCam.SetActive(false);
        BubbleSprite.SetActive(false);
        mainCam.SetActive(true);
        animator.SetBool("isDead", false);
        animator.SetBool("isDive", false);
        animator.SetBool("isSwimming", false);
        endAnimator.SetTrigger("endReplay");

        Time.timeScale = 1;
        AIR = 100;
        timeDived = 0;
        objectAvoided = 0;
        currenttimerCount = 0;
        meterCount = 0;
        PA.Plapping = 0;
        isWon = false;
        isUnder = false;
        gameOver = false;
        isPaused = false;
        SR.sortingOrder = 0;
        audioSource.audioBGM.Stop();
        audioSource.audioBGM.clip = audioClips[0];
        audioSource.audioBGM.Play();
    }

    private void OnWTD()
    {
        WTDManual.SetActive(true);
    }

    private void OnMainMenu()
    {
        Time.timeScale = 1;
        audioSource.audioBGM.clip = audioClips[2];
        audioSource.audioBGM.Play();
        SceneManager.LoadScene("MainMenuScene");
    }

    private void GameLose()
    {
        LoseAnim.SetActive(true);
        audioSource.audioBGM.clip = audioClips[1];
        audioSource.audioBGM.Play();
        animator.SetBool("isSwimming", false);
        animator.SetBool("isDive", false);
        animator.SetBool("isDead", true);
        BubbleSprite.SetActive(false);
        SR.sortingOrder = 1;
        gameOver = true;
        transform.position = new Vector3(-5.55f, -1.65f, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Obstacle") && !gameOver && !IMMORTALITY)
        {
            GameLose();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("ObstacleDude") && !gameOver && !IMMORTALITY)
        {
            GameLose();
        }
        if (collision.gameObject.CompareTag("Fishes") && !gameOver)
        {
            AIR += 20;
            Destroy(collision.gameObject);
        }
    }
}
