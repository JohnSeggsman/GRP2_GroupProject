using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Throwable : MonoBehaviour
{

    public float power;
    float angle;
    float speed = 5;
    float acceleration = 5f;
    float deceleration = 10f;

    public Vector2 minPower, maxPower;

    public TrajectoryLine tl;

    public Rigidbody2D rb;

    Camera cam;
    Vector2 force;
    Vector2 worldPosition;
    Vector2 direction;
    Vector3 rotation1;

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
        StartCoroutine(UpdateDirection());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            StopAllCoroutines();
            rb.AddForce(player.transform.right * speed, ForceMode2D.Force);
        }
        if(Input.GetKey(KeyCode.A))
        {
            if(Input.GetKeyUp(KeyCode.D))
            {

            }
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
