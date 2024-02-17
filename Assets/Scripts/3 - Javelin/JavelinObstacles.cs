using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelinObstacles : MonoBehaviour
{
    private float xSpeed = -4f;
    public Rigidbody2D rb;
    bool isMoving;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving == false)
        {
            transform.Translate(new Vector3(xSpeed * Time.deltaTime, 0, 0f));
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Back"))
        {
            Destroy(this.gameObject);
        }
        if(collision.gameObject.CompareTag("Javelin"))
        {

            isMoving = true;
            animator.enabled = false;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
