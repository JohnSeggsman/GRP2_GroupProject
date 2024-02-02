using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variables
    public static GameManager Instance;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
    }
}