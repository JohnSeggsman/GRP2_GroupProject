using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScript : MonoBehaviour
{
    public Throwable throwable;
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
            Debug.Log("Lose");
            //Lose
            //SceneManager.LoadScene("Javelin");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
