using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScript : MonoBehaviour
{
    public Throwable throwable;
    public JavelinScript javelinscript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && throwable.toggleOnce == false)
        {
            StartCoroutine(nameof(LoseScene));
            //audio.PlayClipAtPoint(sfx.soundTest, Camera.main.transform.position, 1.0); play sound when lose
            
        }
    }
    public IEnumerator LoseScene()
    {
        throwable.audiosource.Stop();
        javelinscript.bgm.SetActive(false);
        javelinscript.loseSound.Play();
        javelinscript.loseCanvas.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(3f);

    }
}
