using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BoxingMinigameManager : MonoBehaviour
{
    public static BoxingMinigameManager Instance;
    [Header("Minigame Reference - DO NOT TOUCH!")]
    Animator m_Animator;
    public AudioClip[] MinigameSFX;
    public GameObject[] GameObjects;
    public Sprite[] PodiumSprites;
    public float FightTimer, EnemyActionTimer;
    public bool isGameEnded, isKnockedOut, PerfectRound;
    private int CurrentAnimID, HitCount, PunchCount, BlockCount, DodgeCount, PlayerHP, EnemyHP, EnemyKO, EnemyPKO, EnemyHitStun, bestEnemyKO, bestEnemyPKO;
    [SerializeReference] private float AttackTimer, BlockTimer, HitTimer;
    private string AttackSide;
    private bool isDodging, EnemyKnockout, KeyRotation, SwingLeft, SwingRight, AttackingLeft, AttackingRight;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        m_Animator = GameObject.Find("Player").GetComponent<Animator>();
        StartCoroutine(FightCountdown());
    }


    void Update()
    {
        FightTimer -= ((isGameEnded || isKnockedOut) || EnemyHP <= 0) ? 0 : Time.deltaTime;
        AttackTimer -= AttackTimer < 0 ? 0 : Time.deltaTime;
        BlockTimer -= BlockTimer < 0 ? 0 : Time.deltaTime;
        HitTimer -= HitTimer < 0 ? 0 : Time.deltaTime;
        EnemyActionTimer -= (EnemyActionTimer < 0 && EnemyHP > 0) ? 0 : Time.deltaTime;
        m_Animator.SetInteger("PunchAnimID", AttackTimer < 0.2f ? 0 : CurrentAnimID);
        m_Animator.SetInteger("BlockAnimID", BlockTimer < 0.2f ? 0 : CurrentAnimID);
        m_Animator.SetInteger("HitAnimID", HitTimer < 0.2f ? 0 : CurrentAnimID);
        if (BlockTimer < 0 && isDodging == true)
            isDodging = false;

        // Update UI Statistics
        if ((FightTimer > 0 && FightTimer < 999) && isGameEnded == false && isKnockedOut == false)
        {
            GameObjects[1].GetComponent<RectTransform>().sizeDelta = new Vector2(PlayerHP, 60);
            GameObjects[2].GetComponent<RectTransform>().sizeDelta = new Vector2(EnemyHP, 60);
            GameObjects[3].GetComponent<TextMeshProUGUI>().text = System.Math.Round(FightTimer).ToString();

            if (AttackTimer <= 0.1f && BlockTimer <= 0.1f && HitTimer < 0.1f)
            {
                // Player Inputs (Punch)
                if (Input.GetKeyDown(KeyCode.A))
                {
                    PunchCount++;
                    CurrentAnimID = 3;
                    AttackTimer = KeyRotation == false ? 0.4f : 0.6f;
                    KeyRotation = true;
                    InteractionOutcome("LowPunch");
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[5]);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    PunchCount++;
                    CurrentAnimID = 4;
                    AttackTimer = KeyRotation == true ? 0.4f : 0.6f;
                    KeyRotation = false;
                    InteractionOutcome("LowPunch");
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[5]);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    PunchCount++;
                    CurrentAnimID = 1;
                    AttackTimer = KeyRotation == false ? 0.4f : 0.6f;
                    KeyRotation = true;
                    InteractionOutcome("HighPunch");
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[5]);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PunchCount++;
                    CurrentAnimID = 2;
                    AttackTimer = KeyRotation == true ? 0.4f : 0.6f;
                    KeyRotation = false;
                    InteractionOutcome("HighPunch");
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[5]);
                }
                // Player Inputs (Block)
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.X))
                {
                    BlockCount++;
                    CurrentAnimID = 1;
                    BlockTimer = 0.5f;
                    InteractionOutcome("Block");
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[8]);
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    DodgeCount++;
                    CurrentAnimID = 2;
                    BlockTimer = 0.5f;
                    isDodging = true;
                    InteractionOutcome("Dodge");
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[8]);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    DodgeCount++;
                    CurrentAnimID = 3;
                    BlockTimer = 0.5f;
                    isDodging = true;
                    InteractionOutcome("Dodge");
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[8]);
                }
                // Pause Menu
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = GameObjects[6].gameObject.activeInHierarchy == false ? 0 : 1;
                    GameObjects[6].gameObject.SetActive(GameObjects[6].gameObject.activeInHierarchy == false ? true : false);
                }
            }
        }

        // Enemy Actions
        if (EnemyActionTimer < 0.1f && EnemyHP > 0 && PlayerHP > 0 && (FightTimer > 0 && FightTimer < 300))
        {
            AttackSide = Random.Range(1, 10) <= 5 ? "AttackLeftID" : "AttackRightID";
            StartCoroutine(EnemyAnimations(AttackSide));
            EnemyActionTimer = Random.Range(1.5f, 3f);
        }

        // Enemy Knockout
        if (EnemyHP <= 0 && EnemyKnockout == false && (FightTimer > 0 && FightTimer < 300))
        {
            GameObject.Find("Canvas").GetComponent<Animator>().enabled = true;
            GameObject.Find("Enemy").GetComponent<Animator>().SetBool("isKnockedOut", true);
            GameObject.Find("Canvas").GetComponent<Animator>().SetBool("KnockoutAnim", true);
            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[11]);
            StartCoroutine(FightCompleted("Knockout"));
        }

        // Hard Timer Limit
        if (FightTimer <= 0 && isGameEnded == false)
            StartCoroutine(FightCompleted("Victory"));
    }

    public void InteractionOutcome(string Outcome)
    {
        switch (Outcome)
        {
            case "HighPunch":
            case "LowPunch":
                if(EnemyHP > 0)
                    if ((!SwingLeft && !SwingRight) && EnemyHitStun == 0)
                    {
                        StartCoroutine(EnemyAnimations("isBlocking"));
                        GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[7]);
                    }
                    else
                    {
                        if (EnemyHitStun == 0)
                        {
                            HitCount++;
                            EnemyActionTimer += 1.5f;
                            if (EnemyHitStun < 12)
                                EnemyHitStun += Random.Range(1, 2);
                            StopCoroutine(EnemyAnimations(AttackSide));
                            StartCoroutine(EnemyAnimations("HitID"));
                            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[6]);
                            if (Outcome == "HighPunch")
                            {
                                EnemyHP -= Random.Range(20, 40);
                                GameObject HitParticle = Instantiate(GameObjects[4], new Vector3(0, 1, 0), Quaternion.identity);
                                Destroy(HitParticle, 1);
                            }
                            else
                            {
                                EnemyHP -= Random.Range(10, 20);
                                GameObject HitParticle = Instantiate(GameObjects[4], new Vector3(0, -0.5f, 0), Quaternion.identity);
                                Destroy(HitParticle, 1);
                            }
                        }
                        else
                        {
                            HitCount++;
                            if (EnemyHitStun < 12)
                                EnemyHitStun--;
                            else
                                EnemyHitStun = 11;
                            StartCoroutine(EnemyAnimations("HitID"));
                            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[6]);
                            if (Outcome == "HighPunch")
                            {
                                EnemyHP -= Random.Range(20, 40);
                                EnemyActionTimer += Random.Range(0.1f, 0.3f);
                                GameObject HitParticle = Instantiate(GameObjects[4], new Vector3(0, 1, 0), Quaternion.identity);
                                Destroy(HitParticle, 1);
                            }
                            else
                            {
                                EnemyHP -= Random.Range(10, 20);
                                EnemyActionTimer += Random.Range(0.2f, 0.6f);
                                GameObject HitParticle = Instantiate(GameObjects[4], new Vector3(0, -0.5f, 0), Quaternion.identity);
                                Destroy(HitParticle, 1);
                            }
                        }
                    }
                break;
            default:
                break;
        }
    }

    private IEnumerator EnemyAnimations(string BoolName)
    {
        if (GameObject.Find("Enemy").GetComponent<Animator>().GetInteger(BoolName) == 0)
            if (BoolName == "isBlocking")
            {
                if (EnemyHitStun > 0)
                    EnemyHitStun--;

                GameObject.Find("Enemy").GetComponent<Animator>().SetInteger(BoolName, 1);
                yield return new WaitForSeconds(0.35f);
                GameObject.Find("Enemy").GetComponent<Animator>().SetInteger(BoolName, 0);
                yield break;
            }
            else if (BoolName == "AttackLeftID" || BoolName == "AttackRightID")
            {
                GameObject.Find("Enemy").GetComponent<Animator>().SetInteger(BoolName, 1);
                if (BoolName == "AttackLeftID")
                    SwingLeft = true;
                else
                    SwingRight = true;
                GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[9]);
                yield return new WaitForSeconds(Random.Range(0.4f, 0.6f));
                GameObject.Find("Enemy").GetComponent<Animator>().SetInteger(BoolName, 2);

                if (BoolName == "AttackLeftID")
                    AttackingLeft = true;
                else
                    AttackingRight = true;

                if(BlockTimer >= 0.2)
                {
                    if (isDodging)
                    {
                        if (EnemyHitStun < 12)
                            EnemyHitStun += Random.Range(4, 12);
                    }
                    else
                    {
                        if (EnemyHitStun < 12)
                        {
                            PlayerHP -= PlayerHP > 20 ? 20 : 0;
                            BlockTimer -= BlockTimer > 0.2f ? 0.4f : 0f;
                            EnemyHitStun += Random.Range(2, 4);
                        }
                    }
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[7]);
                }
                else
                {
                    PlayerHP -= Random.Range(40, 120);
                    HitTimer = 0.5f;
                    EnemyHitStun = 0;
                    PerfectRound = false;
                    GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[6]);

                    // Player Knockout
                    if (PlayerHP <= 0 && EnemyKnockout == false && (FightTimer > 0 && FightTimer < 300))
                    {
                        GameObject.Find("Canvas").GetComponent<Animator>().enabled = true;
                        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("KnockoutAnim", true);
                        GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[11]);
                        m_Animator.SetBool("isKnockout", true);
                        StartCoroutine(FightCompleted("Failure"));
                    }
                }

                SwingLeft = false;
                SwingRight = false;
                yield return new WaitForSeconds(0.4f);
                GameObject.Find("Enemy").GetComponent<Animator>().SetInteger(BoolName, 0);
                AttackingLeft = false;
                AttackingRight = false;
                yield break;
            }
            else if (BoolName == "HitID")
            {
                GameObject.Find("Enemy").GetComponent<Animator>().SetInteger(BoolName, Random.Range(1, 3));
                yield return new WaitForSeconds(0.35f);
                GameObject.Find("Enemy").GetComponent<Animator>().SetInteger(BoolName, 0);
                yield break;
            }
            else
                yield break;
    }

    public void EndMinigame()
    {
        isGameEnded = true;
        AudioManager.Instance.StopBGM();
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult1").GetComponent<TextMeshProUGUI>().text = "Hit Accuracy: " + System.Math.Round(((float)HitCount / PunchCount) * 100, 1) + "%";
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult2").GetComponent<TextMeshProUGUI>().text = "Punches Landed: " + HitCount;
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult3").GetComponent<TextMeshProUGUI>().text = "Knockouts: " + EnemyKO;
        GameObject.Find("Canvas/OutcomeResults/OutcomeResult4").GetComponent<TextMeshProUGUI>().text = "Perfect Knockouts: " + EnemyPKO;
    }

    public void EndSceneButtons(int Options)
    {
        Time.timeScale = 1;
        switch (Options)
        {
            case 1:
                SceneManager.LoadScene("Olympic Boxing");
                break;
            default:
                AudioManager.Instance.PlayBGM(0);
                SceneManager.LoadScene(0);
                break;
        }
    }

    private IEnumerator FightCountdown()
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("BoxingGameStarted", true);
        GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[0]);
        yield return new WaitForSeconds(1.75f);
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1f);
            GameObjects[0].GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[1 + i]);
        }
        try
        {
            AudioManager.Instance.PlayBGM(4);
        }
        catch (System.Exception e)
        {
            Debug.Log("Error while loading audio! " + e);
        }
        GameObjects[0].GetComponent<TextMeshProUGUI>().text = "GO!";
        PlayerHP = 512; EnemyHP = 512;
        EnemyActionTimer = Random.Range(1.5f, 3f);
        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("BoxingGameStarted", false);
        GameObject.Find("Canvas").GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(1f);
        GameObjects[0].GetComponent<TextMeshProUGUI>().text = "";
        FightTimer = 180;
        PerfectRound = true;
        GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[10]);
        yield break;
    }

    public IEnumerator FightCompleted(string Outcome)
    {
        switch (Outcome)
        {
            case "Victory":
                try
                {
                    m_Animator.SetBool("isGameOver", true);
                    GameObject.Find("Canvas").GetComponent<Animator>().enabled = true;
                    GameObject.Find("Canvas").GetComponent<Animator>().SetBool("GameEnded", true);
                    if (isGameEnded == false)
                    {
                        if (EnemyKO > PlayerPrefs.GetFloat("OldRecordBoxing", bestEnemyKO))
                        {
                            bestEnemyKO = EnemyKO;
                            PlayerPrefs.SetFloat("OldRecordBoxing", bestEnemyKO);

                            Debug.Log("Saved Score" + bestEnemyKO);

                        }
                        if (EnemyPKO > PlayerPrefs.GetFloat("OldRecordPerfectBoxing", bestEnemyPKO))
                        {
                            bestEnemyPKO = EnemyPKO;
                            PlayerPrefs.SetFloat("OldRecordPerfectBoxing", bestEnemyPKO);

                            Debug.Log("Saved Score" + bestEnemyPKO);

                        }
                        if (EnemyKO > 3)
                        {
                            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[13]);
                            if (EnemyKO >= 8 && EnemyPKO >= 6)
                            {
                                GameObject.Find("Canvas/OutcomeResults/MatchOutcome").GetComponent<TextMeshProUGUI>().text = "NEW WORLD RECORD!";
                                GameObject.Find("Canvas/BoxingPodium").GetComponent<Image>().sprite = PodiumSprites[0];
                            }
                            else if ((EnemyKO >= 6 && EnemyKO < 8 && EnemyPKO >= 3) || (EnemyKO >= 8 && EnemyPKO < 6))
                            {
                                GameObject.Find("Canvas/OutcomeResults/MatchOutcome").GetComponent<TextMeshProUGUI>().text = "NEW OLYMPIC RECORD!";
                                GameObject.Find("Canvas/BoxingPodium").GetComponent<Image>().sprite = PodiumSprites[1];
                            }
                            else if (EnemyKO >= 5 && EnemyKO < 6 || (EnemyKO >= 6 && EnemyPKO < 3))
                            {
                                GameObject.Find("Canvas/OutcomeResults/MatchOutcome").GetComponent<TextMeshProUGUI>().text = "YOU PLACED FIRST!";
                                GameObject.Find("Canvas/BoxingPodium").GetComponent<Image>().sprite = PodiumSprites[2];
                            }
                            else if (EnemyKO >= 4 && EnemyKO < 5)
                            {
                                GameObject.Find("Canvas/OutcomeResults/MatchOutcome").GetComponent<TextMeshProUGUI>().text = "YOU PLACED SECOND!";
                                GameObject.Find("Canvas/BoxingPodium").GetComponent<Image>().sprite = PodiumSprites[3];
                            }
                            else if (EnemyKO >= 3 && EnemyKO < 4)
                            {
                                GameObject.Find("Canvas/OutcomeResults/MatchOutcome").GetComponent<TextMeshProUGUI>().text = "YOU PLACED THIRD!";
                                GameObject.Find("Canvas/BoxingPodium").GetComponent<Image>().sprite = PodiumSprites[4];
                            }
                        }
                        else
                        {
                            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[14]);
                            GameObject.Find("Canvas/OutcomeResults/MatchOutcome").GetComponent<TextMeshProUGUI>().text = "FIGHT COMPLETED!";
                            GameObject.Find("Canvas/BoxingPodium").GetComponent<Image>().sprite = PodiumSprites[5];
                        }
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
                    isKnockedOut = true;
                    if (isGameEnded == false)
                    {
                        GameObject.Find("Canvas/OutcomeResults/FrameHeader").GetComponent<TextMeshProUGUI>().text = "YOU'RE DISQUALIFIED!";
                        GameObject.Find("Canvas/OutcomeResults/MatchOutcome").GetComponent<TextMeshProUGUI>().text = "KNOCKED OUT!";
                        GameObject.Find("Canvas/BoxingPodium").GetComponent<Image>().sprite = PodiumSprites[5];
                        EndMinigame();
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error while loading audio! " + e);
                }
                yield break;
            case "Knockout":
                EnemyKnockout = true;
                EnemyKO++;
                if (PerfectRound == true)
                    EnemyPKO++;

                var KnockoutDelay = Random.Range(50, 100);
                for (int i = 0; i < KnockoutDelay; i++)
                {
                    PlayerHP += PlayerHP < 512 ? 1 : 0;
                    yield return new WaitForSeconds(0.1f);
                }
                PerfectRound = true;
                EnemyHP = 512;
                EnemyHitStun = 0;
                EnemyKnockout = false;
                EnemyActionTimer = Random.Range(0.5f, 2f);
                GameObject.Find("Enemy").GetComponent<Animator>().SetBool("isKnockedOut", false);
                GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(MinigameSFX[10]);
                yield break;
            default:
                print("Default");
                yield break;
        }
    }
}