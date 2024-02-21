using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DashMinigameManager : MonoBehaviour
{
    public static DashMinigameManager Instance;
    [Header("Minigame Reference - DO NOT TOUCH!")]
    Animator m_Animator;
    public AudioClip[] MinigameSFX;
    public GameObject[] Obstacles, GameObjects;
    public Sprite[] PodiumSprites;
    public float Distance, Speed, RaceTimer;
    public bool isGameEnded, isKnockedOut, isRunning, isJumping, isSliding, KeyRotation;
    private int ScoreObtained, TapCount, JumpCount, SlideCount;
    private float SprintDuration, RunTimer, JumpTimer, SlideTimer, ObstacleTimer;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        m_Animator = GameObject.Find("Player").GetComponent<Animator>();
        StartCoroutine(RaceCountdown());
    }

    void Update()
    {
        // Timer Updates
        Distance += RunTimer < 0 ? 0 : (Time.deltaTime * (Speed / 5));
        RaceTimer += (isGameEnded || isKnockedOut) ? 0 : Time.deltaTime;
        RunTimer -= RunTimer < 0 ? 0 : Time.deltaTime;
        JumpTimer -= JumpTimer < 0 ? 0 : Time.deltaTime;
        SlideTimer -= SlideTimer < 0 ? 0 : Time.deltaTime;
        ObstacleTimer += RunTimer < 0 ? 0 : Speed > 7.5f ? Time.deltaTime * (1 + (Speed / 15)) : Time.deltaTime;
        SprintDuration += Speed > 7.5f ? Time.deltaTime : 0;
        Speed -= Speed > 7.5f ? (Time.deltaTime * 2) : 0;
        isRunning = RunTimer > 0 ? true : false;
        isJumping = JumpTimer > 0 ? true : false;
        isSliding = SlideTimer > 0 ? true : false;
        m_Animator.SetBool("isRunning", isRunning);
        m_Animator.SetBool("isJumping", isJumping);
        m_Animator.SetBool("isSliding", isSliding);
        m_Animator.SetBool("isKnockout", isKnockedOut);

        // Update UI Statistics
        if (RaceTimer > 0 && isGameEnded == false && isKnockedOut == false)
        {
            GameObjects[0].GetComponent<TextMeshProUGUI>().text = System.Math.Round(Distance, 1).ToString() + "M";
            GameObjects[1].GetComponent<TextMeshProUGUI>().text = System.Math.Round(RaceTimer, 1).ToString() + "s";

            // Player Inputs
            if (Input.GetKeyDown(KeyCode.A) && KeyRotation == false)
            {
                TapCount++;
                if (RunTimer < 0.13f && Speed < 14f)
                    Speed += 0.45f;
                RunTimer = 0.25f;
                KeyRotation = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && KeyRotation == true)
            {
                TapCount++;
                if (RunTimer < 0.13f && Speed < 14f)
                    Speed += 0.45f;
                RunTimer = 0.25f;
                KeyRotation = false;
            }
            if (isRunning == true && JumpTimer <= 0 && SlideTimer <= 0)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[0]);
                    JumpCount++;
                    JumpTimer = 0.9f;
                }

                if (Input.GetKeyDown(KeyCode.S) && isRunning == true && JumpTimer <= 0 && SlideTimer <= 0)
                {
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[1]);
                    SlideCount++;
                    SlideTimer = 0.9f;
                }
            }
            // Stride Speed
            if (Input.GetKeyDown(KeyCode.Space) && RunTimer >= 0 && Speed < 10f)
                Speed += 1f;
            // Pause Menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = GameObjects[3].gameObject.activeInHierarchy == false ? 0 : 1;
                GameObjects[3].gameObject.SetActive(GameObjects[3].gameObject.activeInHierarchy == false ? true : false);
            }
        }

        // Walking Sound Controller
        if (isRunning && !isJumping && !isSliding && !isGameEnded)
            GameObject.Find("MinigameManager/MovingSFX").GetComponent<AudioSource>().UnPause();
        else
            GameObject.Find("MinigameManager/MovingSFX").GetComponent<AudioSource>().Pause();

        // Spawn Obstacles
        if (ObstacleTimer >= 2)
        {
            if (Distance < 95)
            {
                GameObject Hurdle = Instantiate(Obstacles[Random.Range(0, Obstacles.Length - 1)], new Vector3(20, -1.85f, 0), Quaternion.identity);
                Hurdle.GetComponent<ObstaclesScrollScript>().ObstacleType = "Obstacles";
                ObstacleTimer = 0;
            }
            else
            {
                GameObject EndPoint = Instantiate(Obstacles[2], new Vector3(20, -2.9f, 0), Quaternion.identity);
                EndPoint.GetComponent<ObstaclesScrollScript>().ObstacleType = "Endpoint";
                Instantiate(Obstacles[2], new Vector3(24.5f, -5.8f, 0), Quaternion.identity);
                ObstacleTimer = -100;
            }
        }

        // Hard Timer Limit
        if (RaceTimer >= 99.9f && isGameEnded == false)
            StartCoroutine(RaceCompleted("Failure"));
    }

    public void EndMinigame()
    {
        RunTimer = 1;
        isGameEnded = true;
        AudioManager.Instance.StopBGM();
        GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("TriggerOutcome");
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult1").GetComponent<TextMeshProUGUI>().text = "Race Time: " + System.Math.Round(RaceTimer, 1).ToString() + "s";
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult2").GetComponent<TextMeshProUGUI>().text = "Steps Taken: " + TapCount;
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult3").GetComponent<TextMeshProUGUI>().text = "Times Jumped: " + JumpCount;
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult4").GetComponent<TextMeshProUGUI>().text = "Times Slidden: " + SlideCount;
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult5").GetComponent<TextMeshProUGUI>().text = "Score Obtained: " + ScoreObtained;
    }

    public void EndSceneButtons(int Options)
    {
        Time.timeScale = 1;
        switch (Options)
        {
            case 1:
                SceneManager.LoadScene("Olympic Hurdle");
                break;
            default:
                AudioManager.Instance.PlayBGM(0);
                SceneManager.LoadScene(0);
                break;
        }
    }

    private IEnumerator RaceCountdown()
    {
        GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[3]);
        yield return new WaitForSeconds(1.75f);
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(1f);
                GameObjects[2].GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[6 + i]);
            }
            try
            {
                AudioManager.Instance.PlayBGM(1);
            }
            catch (System.Exception e)
            {
                Debug.Log("Error while loading audio! " + e);
            }
            GameObjects[2].GetComponent<TextMeshProUGUI>().text = "GO!";
            RaceTimer = 0;
            yield return new WaitForSeconds(1f);
            GameObjects[2].GetComponent<TextMeshProUGUI>().text = "";
            yield break;
        }
    }

    public IEnumerator RaceCompleted(string Outcome)
    {
        switch (Outcome)
        {
            case "Victory":
                try
                {
                    if (isGameEnded == false)
                        if (RaceTimer < 50)
                        {
                            ScoreObtained = (int)((9999 - TapCount) * (SprintDuration / RaceTimer) * (100 / (100 + RaceTimer)));
                            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[4]);
                            if (RaceTimer < 38.0f && ScoreObtained >= 7000)
                            {
                                GameObject.Find("Canvas/OutcomeResults/RaceOutcome").GetComponent<TextMeshProUGUI>().text = "NEW WORLD RECORD!";
                                GameObject.Find("Canvas/HurdlePodium").GetComponent<Image>().sprite = PodiumSprites[0];
                            }
                            else if ((RaceTimer >= 38.0f && RaceTimer < 39.6f && ScoreObtained >= 6750) || (RaceTimer < 38.0f && ScoreObtained < 7000))
                            {
                                GameObject.Find("Canvas/OutcomeResults/RaceOutcome").GetComponent<TextMeshProUGUI>().text = "NEW OLYMPIC RECORD!";
                                GameObject.Find("Canvas/HurdlePodium").GetComponent<Image>().sprite = PodiumSprites[1];
                            }
                            else if ((RaceTimer >= 39.6f && RaceTimer < 43) || (RaceTimer < 39.6f && ScoreObtained < 6750))
                            {
                                GameObject.Find("Canvas/OutcomeResults/RaceOutcome").GetComponent<TextMeshProUGUI>().text = "YOU PLACED FIRST!";
                                GameObject.Find("Canvas/HurdlePodium").GetComponent<Image>().sprite = PodiumSprites[2];
                            }
                            else if (RaceTimer >= 43 && RaceTimer < 46)
                            {
                                GameObject.Find("Canvas/OutcomeResults/RaceOutcome").GetComponent<TextMeshProUGUI>().text = "YOU PLACED SECOND!";
                                GameObject.Find("Canvas/HurdlePodium").GetComponent<Image>().sprite = PodiumSprites[3];
                            }
                            else if (RaceTimer >= 46)
                            {
                                GameObject.Find("Canvas/OutcomeResults/RaceOutcome").GetComponent<TextMeshProUGUI>().text = "YOU PLACED THIRD!";
                                GameObject.Find("Canvas/HurdlePodium").GetComponent<Image>().sprite = PodiumSprites[4];
                            }
                        }
                        else
                        {
                            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[5]);
                            GameObject.Find("Canvas/OutcomeResults/RaceOutcome").GetComponent<TextMeshProUGUI>().text = "RACE COMPLETED!";
                            GameObject.Find("Canvas/HurdlePodium").GetComponent<Image>().sprite = PodiumSprites[5];
                        }
                    EndMinigame();
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error while loading audio! " + e);
                }
                yield break;
            case "Failure":
                try
                {
                    if (isGameEnded == false)
                    {
                        ScoreObtained = (int)((9999 - TapCount) * (SprintDuration / RaceTimer) * (100 / (100 + RaceTimer)) / Random.Range(10, 20));
                        GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[5]);
                        GameObject.Find("Canvas/OutcomeResults/FrameHeader").GetComponent<TextMeshProUGUI>().text = "YOU'RE DISQUALIFIED!";
                        GameObject.Find("Canvas/OutcomeResults/RaceOutcome").GetComponent<TextMeshProUGUI>().text = "RACE FAILED!";
                        GameObject.Find("Canvas/HurdlePodium").GetComponent<Image>().sprite = PodiumSprites[5];
                    }
                    EndMinigame();
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error while loading audio! " + e);
                }
                yield break;
            default:
                yield break;
        }
    }
}