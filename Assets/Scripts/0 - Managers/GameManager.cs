using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Variables
    public static GameManager Instance;
    
    public DashMinigameManager dashMinigameScript;
    public BikeMovementScript bikeMovementScript;
    public SwimmingScript swimmingScript;
    public BoxingMinigameManager boxingMinigameScript;
    public JavelinScript javelinScript;
    public SmoothCameraBMX smoothCameraBMX;
    
    public int buildIndex;
    Scene currentScene;

    void Start()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        /*
        Debug.Log("Running" + PlayerPrefs.GetFloat("OldRecordRunning"));
        Debug.Log("Boxing" + PlayerPrefs.GetFloat("OldRecordBoxing"));
        Debug.Log("Swimming" + PlayerPrefs.GetFloat("OldRecordSwimming"));
        Debug.Log("Javelin" + PlayerPrefs.GetFloat("OldRecordJavelin"));
        Debug.Log("Cycling" + PlayerPrefs.GetFloat("OldRecordCycling"));
        */
    }

    void Update()
    {

    }
    private void FixedUpdate()
    {
        
    }
    /*
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        currentScene = SceneManager.GetActiveScene();
        buildIndex = currentScene.buildIndex;

        if (buildIndex == 1 && dashMinigameScript == null)
        {
            dashMinigameScript = GameObject.Find("MinigameManager").GetComponent<DashMinigameManager>();
            
        }
        else if (buildIndex == 2 && swimmingScript == null)
        {
            swimmingScript = GameObject.Find("StickestMan").GetComponent<SwimmingScript>();

        }
        else if (buildIndex == 3 && javelinScript == null)
        {
            javelinScript = GameObject.Find("Javelin").GetComponent<JavelinScript>();
            
        }
        else if (buildIndex == 4 && boxingMinigameScript == null)
        {
            boxingMinigameScript = GameObject.Find("MinigameManager").GetComponent<BoxingMinigameManager>();

        }
        else if (buildIndex == 5 && bikeMovementScript == null)
        {
            bikeMovementScript = GameObject.Find("Vehicle").GetComponent<BikeMovementScript>();
            smoothCameraBMX = GameObject.Find("Main Camera").GetComponent<SmoothCameraBMX>();
            
        }
    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
    }
    */
    /*
    public void SavedCyclingData()
    {
        bikeMovementScript.newScore = smoothCameraBMX.timerStuff;
        if (bikeMovementScript.newScore < PlayerPrefs.GetFloat("OldRecordCycling", bikeMovementScript.previousScore))
        {
            bikeMovementScript.previousScore = bikeMovementScript.newScore;

            PlayerPrefs.SetFloat("OldRecordCycling", bikeMovementScript.previousScore);

            Debug.Log("Saved Score");

        }
    }
    */
}