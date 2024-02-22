using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // Variables
    Animator m_Animator;

    [Header("Sports Reference - Update if necessary")]
    public string[] SportsName;
    public Sprite[] SportsImage;


    [Header("Menu Reference - DO NOT TOUCH!")]
    public AudioManager _AudioMgr;
    public AudioClip BackgroundMusic;
    public GameObject HeaderText;
    public GameObject SportsTabLocation, SportsTabPrefab, BGEffects, SettingsHeader, SettingsSlider, Volume88Image;
    public GameObject[] OptionsFrame;
    public Sprite[] AwardSprites;
    public Image[] AwardImages;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        _AudioMgr.GetComponent<AudioManager>().PlayBGM(0);

        for (int i = 0; i < SportsName.Length; i++)
        {
            GameObject UISportsTab = Instantiate(SportsTabPrefab, transform.position, Quaternion.identity);
            UISportsTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SportsName[i]; // Sports Name
            UISportsTab.transform.GetChild(4).GetChild(1).GetComponent<Image>().sprite = SportsImage[i]; // Sports Image

            if (SportsName[i] == "Olympic Hurdle")
            {
                UISportsTab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("OldRecordRunning").ToString() + " points"; // Personal Best Timing
            }
            else if (SportsName[i] == "Olympic Swimming")
            {
                UISportsTab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("OldRecordSwimming").ToString("F2") + "s"; // Personal Best Timing
            }
            else if (SportsName[i] == "Olympic Javelin")
            {
                UISportsTab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("OldRecordJavelin").ToString("F2")+ "m"; // Personal Best Timing
            }
            else if (SportsName[i] == "Olympic Boxing")
            {
                UISportsTab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("OldRecordBoxing").ToString() + " KOs"; // Personal Best Timing
            }
            else if (SportsName[i] == "Olympic Cycling")
            {
                UISportsTab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("OldRecordCycling").ToString("F2") + "s"; // Personal Best Timing
            }

            UISportsTab.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => onButtonPressed(1));
            UISportsTab.transform.SetParent(SportsTabLocation.transform, false);
        }
        for (int i = 0; i < SportsName.Length; i++)
        {

            if (SportsName[i] == "Olympic Hurdle")
            {
                if (PlayerPrefs.GetFloat("OldRecordRunningTime") < 38.0f && PlayerPrefs.GetFloat("OldRecordRunning") >= 7000)
                {
                    //wr
                    AwardImages[0].sprite = AwardSprites[0];
                }
                else if ((PlayerPrefs.GetFloat("OldRecordRunningTime") >= 38.0f && PlayerPrefs.GetFloat("OldRecordRunningTime") < 39.6f && PlayerPrefs.GetFloat("OldRecordRunning") >= 6750) || (PlayerPrefs.GetFloat("OldRecordRunningTime") < 38.0f && PlayerPrefs.GetFloat("OldRecordRunning") < 7000))
                {
                    //or
                    AwardImages[0].sprite = AwardSprites[1];
                }
                else if ((PlayerPrefs.GetFloat("OldRecordRunningTime") >= 39.6f && PlayerPrefs.GetFloat("OldRecordRunningTime") < 43) || (PlayerPrefs.GetFloat("OldRecordRunningTime") < 39.6f && PlayerPrefs.GetFloat("OldRecordRunning") < 6750))
                {
                    //gold
                    AwardImages[0].sprite = AwardSprites[2];
                }
                else if (PlayerPrefs.GetFloat("OldRecordRunningTime") >= 43 && PlayerPrefs.GetFloat("OldRecordRunningTime") < 46)
                {
                    //silver
                    AwardImages[0].sprite = AwardSprites[3];
                }
                else if (PlayerPrefs.GetFloat("OldRecordRunningTime") >= 46)
                {
                    //bronze
                    AwardImages[0].sprite = AwardSprites[4];
                }
            }
            else if (SportsName[i] == "Olympic Swimming")
            {
                if (PlayerPrefs.GetFloat("OldRecordSwimming") > 0 && PlayerPrefs.GetFloat("OldRecordSwimming") <= 40)
                {
                    //wr
                    AwardImages[1].sprite = AwardSprites[5];
                }
                else if (PlayerPrefs.GetFloat("OldRecordSwimming") > 40 && PlayerPrefs.GetFloat("OldRecordSwimming") <= 43)
                {
                    //or
                    AwardImages[1].sprite = AwardSprites[6];
                }
                else if (PlayerPrefs.GetFloat("OldRecordSwimming") > 43 && PlayerPrefs.GetFloat("OldRecordSwimming") < 45)
                {
                    //gold
                    AwardImages[1].sprite = AwardSprites[7];
                }
                else if (PlayerPrefs.GetFloat("OldRecordSwimming") > 45 && PlayerPrefs.GetFloat("OldRecordSwimming") < 50)
                {
                    //silver
                    AwardImages[1].sprite = AwardSprites[8];
                }
                else if (PlayerPrefs.GetFloat("OldRecordSwimming") > 50 && PlayerPrefs.GetFloat("OldRecordSwimming") < 60)
                {
                    //bronze
                    AwardImages[1].sprite = AwardSprites[9];
                }
            }
            else if (SportsName[i] == "Olympic Javelin")
            {
                
                if (PlayerPrefs.GetFloat("OldRecordJavelin") > 130)
                {
                    //wr
                    AwardImages[2].sprite = AwardSprites[10];
                }
                else if (PlayerPrefs.GetFloat("OldRecordJavelin") > 125 && PlayerPrefs.GetFloat("OldRecordJavelin") < 130)
                {
                    //or
                    AwardImages[2].sprite = AwardSprites[11];
                }
                else if (PlayerPrefs.GetFloat("OldRecordJavelin") > 118 && PlayerPrefs.GetFloat("OldRecordJavelin") < 125)
                {
                    //gold
                    AwardImages[2].sprite = AwardSprites[12];
                }
                else if (PlayerPrefs.GetFloat("OldRecordJavelin") > 100 && PlayerPrefs.GetFloat("OldRecordJavelin") < 118)
                {
                    //silver
                    AwardImages[2].sprite = AwardSprites[13];
                }
                else if (PlayerPrefs.GetFloat("OldRecordJavelin") > 88 && PlayerPrefs.GetFloat("OldRecordJavelin") < 100)
                {
                    //bronze
                    AwardImages[2].sprite = AwardSprites[14];
                }
            }
            else if (SportsName[i] == "Olympic Boxing")
            {
                if (PlayerPrefs.GetFloat("OldRecordBoxing") >= 8 && PlayerPrefs.GetFloat("OldRecordPerfectBoxing") >= 6)
                {
                    //wr
                    AwardImages[3].sprite = AwardSprites[15];
                }
                else if ((PlayerPrefs.GetFloat("OldRecordBoxing") >= 6 && PlayerPrefs.GetFloat("OldRecordBoxing") < 8 && PlayerPrefs.GetFloat("OldRecordPerfectBoxing") >= 3) || (PlayerPrefs.GetFloat("OldRecordBoxing") >= 8 && PlayerPrefs.GetFloat("OldRecordPerfectBoxing") < 6))
                {
                    //or
                    AwardImages[3].sprite = AwardSprites[16];
                }
                else if ((PlayerPrefs.GetFloat("OldRecordBoxing") >= 5 && PlayerPrefs.GetFloat("OldRecordBoxing") < 6) || (PlayerPrefs.GetFloat("OldRecordBoxing") >= 6 && PlayerPrefs.GetFloat("OldRecordPerfectBoxing") < 3))
                {
                    //gold
                    AwardImages[3].sprite = AwardSprites[17];
                }
                else if ((PlayerPrefs.GetFloat("OldRecordBoxing") >= 4 && (PlayerPrefs.GetFloat("OldRecordBoxing") < 5)))
                {
                    //silver
                    AwardImages[3].sprite = AwardSprites[18];
                }
                else if ((PlayerPrefs.GetFloat("OldRecordBoxing") >= 3 && (PlayerPrefs.GetFloat("OldRecordBoxing") < 4)))
                {
                    //bronze
                    AwardImages[3].sprite = AwardSprites[19];
                }
            }
            else if (SportsName[i] == "Olympic Cycling")
            {
                if (PlayerPrefs.GetFloat("OldRecordCycling") > 0 && PlayerPrefs.GetFloat("OldRecordCycling") <= 26)
                {
                    //wr
                    AwardImages[4].sprite = AwardSprites[20];
                }
                else if (PlayerPrefs.GetFloat("OldRecordCycling") > 26 && PlayerPrefs.GetFloat("OldRecordCycling") <= 27)
                {
                    //or
                    AwardImages[4].sprite = AwardSprites[21];
                }
                else if (PlayerPrefs.GetFloat("OldRecordCycling") > 27 && PlayerPrefs.GetFloat("OldRecordCycling") < 30)
                {
                    //gold
                    AwardImages[4].sprite = AwardSprites[22];
                }
                else if (PlayerPrefs.GetFloat("OldRecordCycling") > 30 && PlayerPrefs.GetFloat("OldRecordCycling") < 35)
                {
                    //silver
                    AwardImages[4].sprite = AwardSprites[23];
                }
                else if (PlayerPrefs.GetFloat("OldRecordCycling") > 35 && PlayerPrefs.GetFloat("OldRecordCycling") < 40)
                {
                    //bronze
                    AwardImages[4].sprite = AwardSprites[24];
                }
            }


        }
    }

    void Update()
    {
        

        BGEffects.transform.Rotate(0, 0, -0.2f);
        Volume88Image.SetActive((Mathf.Round((SettingsSlider.GetComponent<Slider>().value * 100)) == 88));
        if (SettingsSlider.GetComponent<Slider>().value >= 0.2f)
            GameObject.Find("AudioManager/BackgroundMusic").GetComponent<AudioSource>().volume = (SettingsSlider.GetComponent<Slider>().value / 2);

        if(Mathf.Round((SettingsSlider.GetComponent<Slider>().value * 100)) == 69)
            SettingsHeader.GetComponent<TextMeshProUGUI>().text = "Volume (" + Mathf.Round((SettingsSlider.GetComponent<Slider>().value * 100)) + "% NICE)";
        else if (Mathf.Round((SettingsSlider.GetComponent<Slider>().value * 100)) == 88)
            SettingsHeader.GetComponent<TextMeshProUGUI>().text = "Volume (" + Mathf.Round((SettingsSlider.GetComponent<Slider>().value * 100)) + " AP)";
        else
            SettingsHeader.GetComponent<TextMeshProUGUI>().text = "Volume (" + Mathf.Round((SettingsSlider.GetComponent<Slider>().value * 100)) + "%)";
    }

    public void onButtonPressed(int Options)
    {
        if (Options > 1)
            for (int i = 0; i < OptionsFrame.Length; i++)
                OptionsFrame[i].SetActive(false);

        switch (Options)
        {
            case 0: // Back Button
                ResetAllOptions(0.5f);
                m_Animator.SetBool("TransOptions", false);
                break;
            case 1: // Selecting Sports
                TransitionToSports(EventSystem.current.currentSelectedGameObject.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>().text);
                break;
            case 2: // Choose Sports Tab
                OptionsFrame[0].SetActive(true);
                m_Animator.SetBool("TransOptions", true);
                HeaderText.GetComponent<TextMeshProUGUI>().text = "Choose your Sports!";
                break;
            case 3: // View Trophies Tab
                OptionsFrame[1].SetActive(true);
                m_Animator.SetBool("TransOptions", true);
                HeaderText.GetComponent<TextMeshProUGUI>().text = "Trophy Collections";
                break;
            case 4: // Settings Tab
                OptionsFrame[2].SetActive(true);
                m_Animator.SetBool("TransOptions", true);
                HeaderText.GetComponent<TextMeshProUGUI>().text = "Settings";
                break;
            default: // Quit Application
                Application.Quit(0);
                break;
        }
    }

    public void TransitionToSports(string SportsSelected)
    {
        if (System.Array.IndexOf(SportsName, SportsSelected) != -1)
        {
            _AudioMgr = AudioManager.Instance;
            _AudioMgr.GetComponent<AudioManager>().StopBGM();
            SceneManager.LoadScene(SportsSelected);

        }
    }

    public void ChangeDifficulty()
    {
        if (GameObject.Find("Canvas/OptionsTab/SettingsFrame/SettingsDropdown2").GetComponent<TMP_Dropdown>().value > 5)
            GameObject.Find("Canvas/OptionsTab/SettingsFrame/SettingsDropdown2").GetComponent<TMP_Dropdown>().value = 0;
    }
    
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);

    }

    IEnumerator ResetAllOptions(float WaitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(WaitTime);
            for (int i = 0; i < OptionsFrame.Length; i++)
                OptionsFrame[i].SetActive(false);
            yield break;
        }
    }
}