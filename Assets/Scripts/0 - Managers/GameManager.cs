using UnityEngine;
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
    }

    void Update()
    {

    }
}