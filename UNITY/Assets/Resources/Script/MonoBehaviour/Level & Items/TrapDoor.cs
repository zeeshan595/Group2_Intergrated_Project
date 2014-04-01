using UnityEngine;
using System.Collections;

public class TrapDoor : MonoBehaviour
{
    public GameObject trap;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            trap.GetComponent<HingeJoint>().useSpring = false;
        }
    }
}