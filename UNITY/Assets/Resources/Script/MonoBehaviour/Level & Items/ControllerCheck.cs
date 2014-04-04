using UnityEngine;
using System.Collections;

public class ControllerCheck : MonoBehaviour
{
    public GameObject[] ToDisable;
    public GameObject[] ToEnable;

    public void LateUpdate()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            foreach (GameObject g in ToDisable)
                g.SetActive(false);

            foreach (GameObject g in ToEnable)
                g.SetActive(true);
        }
        else
        {
            foreach (GameObject g in ToDisable)
                g.SetActive(true);

            foreach (GameObject g in ToEnable)
                g.SetActive(false);
        }
    }
}