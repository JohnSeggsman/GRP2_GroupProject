using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarScript : MonoBehaviour
{
    public Image PowerBarMask;
    public float barChangeSpeed = 1;
    float maxPowerBarValue = 30;
    public float currentPowerBarValue;
    bool powerIsIncreasing;
    public bool powerBarOn;
    // Start is called before the first frame update
    void Start()
    {
        currentPowerBarValue = maxPowerBarValue;
        powerIsIncreasing = false;
        powerBarOn = true;
        StartCoroutine(UpdatePowerBar());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator UpdatePowerBar()
    {
        while(powerBarOn)
        {
            if(!powerIsIncreasing)
            {
                currentPowerBarValue -= barChangeSpeed;
                if(currentPowerBarValue <= 0)
                {
                    powerIsIncreasing = true;
                }
            }
            if(powerIsIncreasing)
            {
                currentPowerBarValue += barChangeSpeed;
                if(currentPowerBarValue >= maxPowerBarValue)
                {
                    powerIsIncreasing = false;
                }
            }
            //currentPowerBarValue -= barChangeSpeed;

            float fill = currentPowerBarValue / maxPowerBarValue;
            PowerBarMask.fillAmount = fill;
            yield return new WaitForSeconds(0.02f);
            if(Input.touchCount >0)
            {
                powerBarOn = false;
            }
        }
        yield return null;
        
    }
}
