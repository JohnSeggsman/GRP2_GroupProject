using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothCameraScript : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Transform player;
    public Throwable throwable;
    public Text distanceText;
    public JavelinScript javelinscript;
    public Image leftKey;
    public Image rightKey;
    public Image spaceKey;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        distanceText.text = javelinscript.distanceTraveled.ToString("F2") + "M";
        if(throwable.toggleOnce == true)
        {
            //target = throwable.weaponInst.transform;
            smoothTime = 0f;
            Vector3 targetPosition = target.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            smoothTime = 0.01f;
            Vector3 targetPosition = target.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        

    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            leftKey.GetComponent<Image>().color = new Color32(126, 126, 126, 255);
        }
        else if (!Input.GetKeyDown(KeyCode.A))
        {
            leftKey.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rightKey.GetComponent<Image>().color = new Color32(126, 126, 126, 255);
        }
        else if (!Input.GetKeyDown(KeyCode.D))
        {
            rightKey.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            spaceKey.GetComponent<Image>().color = new Color32(126, 126, 126, 255);
        }
        else if (!Input.GetKeyDown(KeyCode.Space))
        {
            spaceKey.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}
