using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraScript : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Throwable throwable;
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
        
        if(throwable.toggleOnce == true)
        {
            //target = throwable.weaponInst.transform;
            Vector3 targetPosition = target.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        

    }
}
