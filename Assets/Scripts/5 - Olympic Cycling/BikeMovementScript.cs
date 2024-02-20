using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BikeMovementScript : MonoBehaviour
{
    public SmoothCameraBMX smoothcameraBMX;

    [SerializeField]private Rigidbody2D frontWheel;
    [SerializeField] private Rigidbody2D backWheel;
    [SerializeField] private Rigidbody2D carRb;

    [SerializeField] private float speed = 150f;
    [SerializeField] private float rotationSpeed = 300f;
    [SerializeField] Animator animator;
    

    public GameObject person;
    private float _moveInputX;
    private float _moveInputY;

    [Range(0.1f, 5f)] private float staminaDrainSpeed = 1f;
    public float maxStaminaAmount = 2f;
    public float currentStaminaAmount;

    public bool usedAllStamina = false;
    public GameObject tiredImage;
    public GameObject losePanel;
    public GameObject winPanel;
    public AudioSource bikeSound;
    public AudioSource loseSound;
    public AudioSource winSound;
    public AudioSource BGM;
    public GameObject BGMObject;

    public bool winGame;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = person.GetComponent<Animator>();
        currentStaminaAmount = maxStaminaAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            animator.speed = 0;
        }
        else if(usedAllStamina == true && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)))
        {
            animator.speed = 0;
        }

        if(Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            carRb.MoveRotation(carRb.rotation + 200 * Time.deltaTime);
        }
        else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            carRb.MoveRotation(carRb.rotation - 300 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            animator.speed = 0;
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) && usedAllStamina == false)
        {
            animator.SetBool("IsMoving", true);
            animator.speed = 1;
            speed = 100f;
            frontWheel.AddTorque(-speed * Time.fixedDeltaTime);
            backWheel.AddTorque(-speed * Time.fixedDeltaTime);
        }
        
        carRb.AddTorque(_moveInputY * rotationSpeed * Time.fixedDeltaTime);
        if (Input.GetKey(KeyCode.S) && currentStaminaAmount >= 0 && usedAllStamina == false)
        {
            animator.SetBool("IsMoving", true);
            //animator.SetBool("IsMovingReverse", false);
            frontWheel.AddTorque(-speed * Time.fixedDeltaTime);
            backWheel.AddTorque(-speed * Time.fixedDeltaTime);
            currentStaminaAmount -= Time.deltaTime * staminaDrainSpeed;
            animator.speed = 2;
            speed = 300f;
        }
        else if (currentStaminaAmount <= 0)
        {
            usedAllStamina = true;
            speed = 100f;
        }

        if (usedAllStamina == true && currentStaminaAmount <= 1f)
        {
            
            smoothcameraBMX.staminaImage.color = new Color32(255, 0, 0, 255);
        }
        else if(usedAllStamina == true && currentStaminaAmount > 1f)
        {
            usedAllStamina = false;
            smoothcameraBMX.staminaImage.color = new Color32(255, 161, 0, 255);
        }

        if(usedAllStamina == true && Input.GetKey(KeyCode.S))
        {
            tiredImage.SetActive(true);
        }
        else if(usedAllStamina == true && !Input.GetKey(KeyCode.S))
        {
            tiredImage.SetActive(false);
        }

        if (!Input.GetKey(KeyCode.S) && currentStaminaAmount < maxStaminaAmount)
        {
            currentStaminaAmount += Time.deltaTime * 0.5f;
        }

        if(winGame == true)
        {
            StartCoroutine(nameof(WinScene));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                animator.speed = 0;
                frontWheel.velocity = Vector3.zero;
                backWheel.velocity = Vector3.zero;
                
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(nameof(LoseScene));
        
    }

    public IEnumerator LoseScene()
    {
        BGM.gameObject.SetActive(false);
        loseSound.Play();
        losePanel.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(1f);
        

    }
    public IEnumerator WinScene()
    {
        BGM.gameObject.SetActive(false);
        winSound.Play();
        winPanel.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(3f);
        
        
    }
}
