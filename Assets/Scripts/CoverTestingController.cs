using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverTestingController : MonoBehaviour
{
    public Transform component;
    public Transform entry01;
    public Transform entry07;
    public Transform exit06;
    public Transform exit12;
    public GameObject configL;
    public GameObject configR;
    public GameObject configC;

    public void ChangeConfig(int config) {
        switch (config) {
            case 1:
                configL.SetActive(true);
                configR.SetActive(false);
                configC.SetActive(false);
                break;
            case 2:
                configL.SetActive(false);
                configR.SetActive(true);
                configC.SetActive(false);
                break;
            case 3:
                configL.SetActive(false);
                configR.SetActive(false);
                configC.SetActive(true);
                break;
            default:
                configL.SetActive(false);
                configR.SetActive(false);
                configC.SetActive(false);
                break;
        }
    }
}
