using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelinScript : MonoBehaviour
{
    Rigidbody2D rb;
    public Throwable throwable;
    bool toggleOnce = false;
    public Transform head;
    //PowerBarScript powerbarscript;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.GetComponent<Rigidbody2D>().isKinematic = true;
        

    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y < -0.001f)
        {
            rb.AddForceAtPosition(30 * Time.deltaTime * -transform.up, head.position);
        }
    }
    public void SetStraightVelocity()
    {
        rb.GetComponent<Rigidbody2D>().isKinematic = false;
        rb.gravityScale = 1;
        transform.parent = null;
        rb.velocity = 1000 * Time.deltaTime * transform.right;
        
    }
    private void FixedUpdate()
    {
        if (throwable.toggleOnce == true && toggleOnce == false)
        {
            SetStraightVelocity();
            toggleOnce = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            rb.simulated = false;
        }
    }
}
