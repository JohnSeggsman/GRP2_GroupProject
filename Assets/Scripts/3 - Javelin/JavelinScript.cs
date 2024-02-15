using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelinScript : MonoBehaviour
{
    public Vector3 lastPosition;
    public float distanceTraveled;
    Rigidbody2D rb;
    public Throwable throwable;
    bool toggleOnce = false;
    public Transform head;
    public GameObject flag;
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
        HandleSpearRotation();
        if (rb.velocity.y < -0.00001f)
        {

        }
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
                
                //Fail
            }
            else
            {

            }

            if(rb.simulated == false && distanceTraveled == 0)
            {
                Debug.Log("Fail");
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
            }
        }
        
    }

    public void HandleSpearRotation()
    {
        Vector3 euler = transform.eulerAngles;
        if (euler.z > 180) euler.z -= 360;
        euler.z = Mathf.Clamp(euler.z, -70, 90);
        transform.eulerAngles = euler;
    }
}
