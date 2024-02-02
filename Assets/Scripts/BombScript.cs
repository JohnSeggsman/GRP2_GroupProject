using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    float bulletSpeed = 15f;
    Rigidbody2D rb;
    Animator animator;
    PowerBarScript powerbarscript;
    private void Awake()
    {
        powerbarscript = GameObject.Find("PowerBarCanvas").GetComponent<PowerBarScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        SetStraightVelocity();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetStraightVelocity()
    {
        rb.velocity = transform.right * powerbarscript.currentPowerBarValue;
    }
    private void FixedUpdate()
    {
        if(rb.velocity.magnitude <= 0.3)
        {
            animator.GetComponent<Animator>().enabled = true;
        }
    }
}
