using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Variables
    public static AudioManager Instance;
    public AudioSource audioBGM;
    public AudioClip[] audioBGMArray;
    private AudioSource audioSource;

    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioBGM.Play();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBGM(int SceneID)
    {
        audioBGM.clip = audioBGMArray[SceneID];
        audioBGM.Play();
    }

    public void StopBGM()
    {
        audioBGM.Stop();
    }

    public void PauseBGM()
    {
        audioBGM.Pause();
    }

    public void UnPauseBGM()
    {
        audioBGM.UnPause();
    }
}