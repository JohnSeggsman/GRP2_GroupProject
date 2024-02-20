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
<<<<<<< Updated upstream
=======
        audioSource = GetComponent<AudioSource>();
>>>>>>> Stashed changes

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
<<<<<<< Updated upstream

        audioBGM.Play();
        audioSource = GetComponent<AudioSource>();
=======
>>>>>>> Stashed changes
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