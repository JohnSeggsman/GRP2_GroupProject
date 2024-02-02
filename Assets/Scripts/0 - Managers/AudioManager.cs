using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Variables
    public static AudioManager Instance;
    public AudioSource audioBGM;
    private AudioSource audioSource;
    void Start()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioBGM.Play();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBGM()
    {
        audioBGM.Play();
    }

    public void StopBGM()
    {
        audioBGM.Pause();
    }
}