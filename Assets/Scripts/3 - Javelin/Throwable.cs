using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Throwable : MonoBehaviour
{
    public float mashDelay = 0.5f;

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
    public Animator animbomb;

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
        rb = player.GetComponent<Rigidbody2D>();
        currentArmValue = maxArmValue;
        armIncreasing = false;
        armOn = true;
        mash = mashDelay;
        StartCoroutine(UpdateDirection());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            StopAllCoroutines();
        }
        /*
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(Input.GetKeyUp(KeyCode.D))
            {
                isRun = true;
                //rb.AddForce(player.transform.right * speed, ForceMode2D.Force);
            }
        }
        */
        /*
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            if(buttonCooler > 0 && buttonCount == 2)
            {
                isRun = true;
            }
            else
            {
                buttonCooler = 0.5f;
                buttonCount += 1;
            }
        }

        if (buttonCooler > 0)
        {
            buttonCooler -= 1 * Time.deltaTime;
        }
        else
        {
            buttonCount = 0;
        }
        
        if (!Input.anyKey)
        {
            isRun = false;
        }
        */
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            isRun = true;
        }

        HandleGunRotation();
        

        if (toggleOnce == false)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Animator>().enabled = true;
                StopAllCoroutines();
                toggleOnce = true;
            }
            
        }
        if (isRun == true)
        {
            mash -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) && !pressed)
            {
                rb.velocity = rb.velocity.normalized * speed;
                speed += Time.deltaTime;
                rb.AddForce(player.transform.right * speed, ForceMode2D.Impulse);
                pressed = true;
                mash = mashDelay;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                pressed = false;
            }
            if (mash <= 0)
            {
                isRun = false;
                //rb.velocity = Vector2.zero;
            }
        }
        else if (isRun == false)
        {
            speed = 0.5f;
            //rb.velocity = Vector2.zero;
        }
    }
    private void FixedUpdate()
    {
        
    }

    public void HandleGunRotation()
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
