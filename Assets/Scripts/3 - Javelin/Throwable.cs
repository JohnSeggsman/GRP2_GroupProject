﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Throwable : MonoBehaviour
{
    public float mashDelay;

    float mash;
    bool pressed;

    public float power;
    float angle;
    public float speed;
    float buttonCooler = 0.5f;
    int buttonCount = 0;

    public Vector2 minPower, maxPower;

    public TrajectoryLine tl;

    public Rigidbody2D rb;

    Camera cam;

    public GameObject weapon;
    public GameObject weaponInst;
    public GameObject player;
    public Transform weaponSpawnPoint;
    public bool toggleOnce = false;

    public Animator animator;
    public Animator playerAnimator;
    public AudioSource audiosource;
    public AudioSource audiosourcethrow;

    public float armChangeSpeed = 1;
    float maxArmValue = 80f;
    public float currentArmValue;
    bool armIncreasing;
    public bool armOn;
    Vector2 velocity = Vector2.zero;
    bool isRun = false;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();
        animator = GetComponent<Animator>();
        playerAnimator = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
        currentArmValue = maxArmValue;
        armIncreasing = false;
        armOn = true;
        mash = mashDelay;
        audiosource = player.GetComponent<AudioSource>();
        audiosourcethrow = GetComponent<AudioSource>();
        StartCoroutine(UpdateDirection());
    }
    // Update is called once per frame
    void Update()
    {
        HandleSpearRotation();
        if (Input.GetKey(KeyCode.A))
        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                StopAllCoroutines();
                isRun = true;
            }
            
        }

        if (toggleOnce == false)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audiosource.Stop();
                audiosourcethrow.Play();
                GetComponent<Animator>().enabled = true;
                StopAllCoroutines();
                toggleOnce = true;
            }
            
        }
        
        
        if (isRun == true)
        {
            
            mash -= Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKeyDown(KeyCode.D) && !pressed)
                {
                    audiosource.Play();
                    playerAnimator.SetBool("IsRunJavelin", true);
                    //playerAnimator.SetBool("IsIdle", false);
                    rb.velocity = rb.velocity.normalized * speed;
                    speed += Time.deltaTime;
                    rb.AddForce(player.transform.right * speed, ForceMode2D.Impulse);
                    pressed = true;
                    mash = mashDelay;
                }
            }
            else if (!Input.GetKeyDown(KeyCode.A) || !Input.GetKeyDown(KeyCode.D))
            {
                
                pressed = false;
            }
            if (mash <= 0)
            {

                
                rb.velocity = Vector2.zero;
                //isRun = false;

                //rb.velocity = Vector2.zero;
            }
            if(!Input.anyKey)
            {
                isRun = false;
                
            }
            if (buttonCooler > 0 && buttonCount > 1)
            {
                
                isRun = true;
                playerAnimator.SetBool("IsRunJavelin", true);
                rb.velocity = rb.velocity.normalized * speed;
                speed += Time.deltaTime;
                rb.AddForce(player.transform.right * speed, ForceMode2D.Impulse);
            }
            else
            {

                buttonCooler = 0.5f;
                buttonCount += 1;
            }
        }
        
        if (isRun == false)
        {
            audiosource.Stop();
            speed = 1f;
            playerAnimator.SetBool("IsRunJavelin", false);
            //playerAnimator.SetBool("IsIdle",true);

        }

        if (buttonCooler > 0)
        {
            buttonCooler -= 1 * Time.deltaTime;
        }
        else
        {
            buttonCount = 0;
        }

    }
    private void FixedUpdate()
    {
        
    }

    public void HandleSpearRotation()
    {
        Vector3 euler = transform.eulerAngles;
        if (euler.z > 180) euler.z -= 360;
        euler.z = Mathf.Clamp(euler.z, -30, 50);
        transform.eulerAngles = euler;
    }
    IEnumerator UpdateDirection()
    {
        while (armOn)
        {
            if (!armIncreasing)
            {
                this.transform.rotation *= Quaternion.AngleAxis(1, Vector3.back * Time.deltaTime);
                currentArmValue -= armChangeSpeed;
                if (currentArmValue <= 0)
                {
                    armIncreasing = true;
                }
            }
            if (armIncreasing)
            {
                this.transform.rotation *= Quaternion.AngleAxis(1, Vector3.forward * Time.deltaTime);
                currentArmValue += armChangeSpeed;
                if (currentArmValue >= maxArmValue)
                {
                    armIncreasing = false;
                }
            }

            yield return new WaitForSeconds(0.02f);
        }
        yield return null;

    }
    
}
