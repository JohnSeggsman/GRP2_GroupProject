using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variables
    public static GameManager Instance;

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