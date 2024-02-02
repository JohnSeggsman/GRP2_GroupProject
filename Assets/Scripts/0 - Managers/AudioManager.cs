using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Variables
    public static AudioManager instance;
    public AudioSource audioBGM;
    private AudioSource audioSource;
    void Start()
    {
        if (instance == null)
            instance = this;

        audioBGM.Play();
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
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