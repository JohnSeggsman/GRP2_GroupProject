using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelinScript : MonoBehaviour
{
    public Vector3 lastPosition;
    public float distanceTraveled;
    Rigidbody2D rb;
    public Throwable throwable;
    public AudioSource audiosource;
    bool toggleOnce = false;
    public Transform head;
    public GameObject flag;
    public GameObject loseCanvas;
    public GameObject winCanvas;
    public SmoothCameraScript smoothcamscript;
    public AudioSource loseSound;
    public AudioSource winSound;
    public GameObject bgm;

    public float birdScore;
    public float previousScore;
    public float newScore;
    //PowerBarScript powerbarscript;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.GetComponent<Rigidbody2D>().isKinematic = true;
        audiosource = GetComponent<AudioSource>();
        loseSound = loseSound.GetComponent<AudioSource>();
        winSound = winSound.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void SetStraightVelocity()
    {
        rb.GetComponent<Rigidbody2D>().isKinematic = false;
        rb.gravityScale = 1;
        transform.parent = null;
        rb.velocity = throwable.speed * 500 * Time.deltaTime * transform.right;
        //2000
    }
    private void FixedUpdate()
    {
        rb.AddForceAtPosition(3 * Time.deltaTime * -transform.up, head.position);
        if (rb.velocity.y < -0.001f && transform.position.y <= 3 && distanceTraveled > 0)
        {
            rb.AddForceAtPosition(70 * Time.deltaTime * -transform.up, head.position);
        }
        if (rb.velocity.x <= 1f && rb.velocity.y < -0.001f && distanceTraveled > 0)
        {
            HandleSpearRotation();
            rb.AddForceAtPosition(70 * Time.deltaTime * -transform.up, head.position);
        }
        //HandleSpearRotation();
        if (throwable.toggleOnce == true && toggleOnce == false)
            
        {
            SetStraightVelocity();
            toggleOnce = true;
        }
        if(throwable.toggleOnce == true)
        {
            //distanceTraveled += Vector3.Distance(transform.position, flag.transform.position);
            //distanceTraveled += Vector3.Distance(transform.position, lastPosition);
            distanceTraveled = transform.position.x;
            if (distanceTraveled <= 0)
            {
                distanceTraveled = 0;
                if (rb.velocity.x <= 1f)
                {
                    StartCoroutine(nameof(LoseScene));
                }
            }
            else
            {

            }

            if(rb.simulated == false && distanceTraveled >= 0 && distanceTraveled < 88)
            {
                StartCoroutine(nameof(LoseScene));
            }
            if(rb.simulated == false && distanceTraveled >= 88)
            {
                StartCoroutine(nameof(WinScene));
            }
            
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (distanceTraveled > 0)
        {
            if (collision.CompareTag("Ground"))
            {

                rb.simulated = false;
                //newScore = PlayerPrefs.SetFloat("Distance",distanceTraveled);
            }
        }
        
        if(collision.CompareTag("Bird"))
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            //collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            birdScore += 1;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bird"))
        {
            birdScore += 1;
        }
    }

    public void HandleSpearRotation()
    {
        Vector3 euler = transform.eulerAngles;
        if (euler.z > 180) euler.z -= 360;
        euler.z = Mathf.Clamp(euler.z, -90, 90);
        transform.eulerAngles = euler;
    }

    public IEnumerator WinScene()
    {
        
        winSound.Play();
        //old record upon playerprefs
        newScore = distanceTraveled;
        if (distanceTraveled > PlayerPrefs.GetFloat("OldRecord", previousScore))
        {
            previousScore = distanceTraveled;
            PlayerPrefs.SetFloat("OldRecord", previousScore);

            Debug.Log("Saved Score");

        }
        if (distanceTraveled > 88 && distanceTraveled < 100)
        {
            smoothcamscript.titleText.text = "GOOD JOB \nYOU GOT BRONZE MEDAL!";
        }
        else if (distanceTraveled > 100 && distanceTraveled < 118)
        {
            smoothcamscript.titleText.text = "AMAZING! \nYOU GOT SILVER MEDAL!";
        }
        else if (distanceTraveled > 118 && distanceTraveled < 125)
        {
            smoothcamscript.titleText.text = "INCREDIBLE! \n YOU GOT GOLD MEDAL!";
        }
        else if (distanceTraveled > 125 && distanceTraveled < 130)
        {
            smoothcamscript.titleText.text = "YOWZERS! \n YOU BEAT THE OLYMPIC RECORD!";
        }
        else if (distanceTraveled > 130)
        {
            smoothcamscript.titleText.text = "SUPERCALIFRAGILISTICEXPIALIDOCIOUS! \nNEW WORLD RECORD!";
        }
        smoothcamscript.oldScoreText.text = "PREVIOUS RECORD: " + PlayerPrefs.GetFloat("OldRecord").ToString("F2");
        smoothcamscript.newScoreText.text = "LATEST RECORD: " + distanceTraveled.ToString("F2");
        smoothcamscript.birdText.text = "BIRDS HIT: " + birdScore;
        //audiosource.Play();
        throwable.audiosource.Stop();
        previousScore = distanceTraveled;
        yield return new WaitForSeconds(3f);
        bgm.SetActive(false);
        winCanvas.SetActive(true);
    }
    public IEnumerator LoseScene()
    {
        bgm.SetActive(false);
        throwable.audiosource.Stop();
        loseSound.Play();
        loseCanvas.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(3f);

    }
}
