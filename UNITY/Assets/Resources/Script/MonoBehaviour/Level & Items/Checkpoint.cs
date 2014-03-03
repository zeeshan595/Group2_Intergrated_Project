using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.transform.parent.GetComponent<Car>())
                other.transform.parent.GetComponent<Car>().resetPosition = transform.position;
        }
    }
}