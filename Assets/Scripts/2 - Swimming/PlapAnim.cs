using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlapAnim : MonoBehaviour
{
    [SerializeField] private GameObject[] PLAP;
    public int Plapping;

    private SwimmingScript SS;

    private void Start()
    {
        Plapping = 0;
        SS = GameObject.Find("StickestMan").GetComponent<SwimmingScript>();
        PLAP[0].SetActive(false);
        PLAP[1].SetActive(false);
        PLAP[2].SetActive(false);
        PLAP[3].SetActive(false);
        PLAP[4].SetActive(false);
    }
    private void Update()
    {
        switch (Plapping)
        {
            case 0:
                PLAP[0].SetActive(false);
                PLAP[1].SetActive(false);
                PLAP[2].SetActive(false);
                PLAP[3].SetActive(false);
                PLAP[4].SetActive(false);
                break;
            case 1:
                PLAP[0].SetActive(true);
                break;
            case 2:
                PLAP[1].SetActive(true);
                break;
            case 3:
                PLAP[2].SetActive(true);
                break;
            case 4:
                PLAP[3].SetActive(true);
                break;
            case 5:
                PLAP[4].SetActive(true);
                break;
        }
    }

    public void onPlap()
    {
        if (SS.meterCount >= 96)
        {
            if (Plapping < 7)
            {
                Plapping++;
            }
            if (Plapping == 6)
            {
                Plapping = 0;
            }
        }
    }
}
