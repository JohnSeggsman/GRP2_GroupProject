using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Throwable : MonoBehaviour
{
    public PowerBarScript powerbarscript;
    

    public float power;
    float angle;
    float speed = 5;
    float currentRotation;

    public Vector2 minPower, maxPower;

    public TrajectoryLine tl;

    Camera cam;
    Vector2 force;
    Vector2 worldPosition;
    Vector2 direction;
    Vector3 rotation1;

    public GameObject bomb;
    public GameObject placeHolder;
    public GameObject bombInst;
    public Transform bombSpawnPoint;
    public bool toggleOnce = false;
    int tries = 3;

    public Animator animator;
    public Animator animbomb;
    private void Awake()
    {
        powerbarscript = GameObject.Find("PowerBarCanvas").GetComponent<PowerBarScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {

        HandleGunRotation();
        if (Input.GetKey(KeyCode.Q))
        {
            transform.rotation *= Quaternion.AngleAxis(1, Vector3.forward * Time.deltaTime);
            
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.rotation *= Quaternion.AngleAxis(1, Vector3.back * Time.deltaTime);
            
        }

        if (toggleOnce == false && tries >= 0)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleGunShooting();
                tries -= 1;
                powerbarscript.powerBarOn = false;
                placeHolder.SetActive(false);
                //angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                GetComponent<Animator>().enabled = true;
                //force = new Vector2(Mathf.Clamp(arm.transform.position.x+angle, minPower.x, maxPower.x), Mathf.Clamp(arm.transform.position.y+angle, minPower.y, maxPower.y));
                //rb.AddForce(force * powerbarscript.currentPowerBarValue, ForceMode2D.Impulse);
                
                toggleOnce = true;
            }
            
        }
        if(tries <= 0)
        {
            //Lose condition restart scene
        }
        
    }
    
    public void HandleGunRotation()
    {
        Vector3 euler = transform.eulerAngles;
        if (euler.z > 180) euler.z = euler.z - 360;
        euler.z = Mathf.Clamp(euler.z, -50, 50);
        transform.eulerAngles = euler;
    }
    
    public void HandleGunShooting()
    {
        if (transform.childCount <= 2)
        {
            bombInst = Instantiate(bomb, bombSpawnPoint.position, transform.rotation);
        }
        
    }
}
