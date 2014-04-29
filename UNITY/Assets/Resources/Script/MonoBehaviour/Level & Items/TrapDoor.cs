using UnityEngine;
using System.Collections;

public class TrapDoor : MonoBehaviour
{
    public GameObject trap;
    public bool destroy = false;
    public bool oposite = false;

    private void Start()
    {
        renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!destroy)
            {
                if (oposite)
                    trap.GetComponent<HingeJoint>().useSpring = true;
                else
                    trap.GetComponent<HingeJoint>().useSpring = false;
            }
            else
                Destroy(trap);
        }
    }
}