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
    public string[] SportsID;
    public Sprite[] SportsImage;
    [SerializeReference] private string[] SportsName;


    [Header("Menu Reference - DO NOT TOUCH!")]
    public GameObject HeaderText;
    public GameObject SportsTabLocation, SportsTabPrefab, BGEffects, SettingsHeader, SettingsSlider, Volume88Image;
    public GameObject[] OptionsFrame;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        for (int i = 0; i < SportsName.Length; i++)
        {
            GameObject UISportsTab = Instantiate(SportsTabPrefab, transform.position, Quaternion.identity);
            UISportsTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SportsName[i]; // Sports Name
            UISportsTab.transform.GetChild(4).GetChild(1).GetComponent<Image>().sprite = SportsImage[i]; // Sports Image
            UISportsTab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "0:00 or 0.0M"; // Personal Best Timing
            UISportsTab.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => onButtonPressed(1));
            UISportsTab.transform.SetParent(SportsTabLocation.transform, false);
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
            try
            {
                SceneManager.LoadScene(SportsSelected);
            }
            catch(System.Exception e)
            {
                print("Scene has not been loaded! " + e);
            }
    }

    public void ChangeDifficulty()
    {
        if (GameObject.Find("Canvas/OptionsTab/SettingsFrame/SettingsDropdown2").GetComponent<TMP_Dropdown>().value > 5)
            GameObject.Find("Canvas/OptionsTab/SettingsFrame/SettingsDropdown2").GetComponent<TMP_Dropdown>().value = 0;
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