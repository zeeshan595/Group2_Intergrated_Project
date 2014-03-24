using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BoxCollider>().size = new Vector3(0.05f, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.transform.parent.GetComponent<Car>())
                other.transform.parent.GetComponent<Car>().resetPosition = transform.position;
        }
    }
}