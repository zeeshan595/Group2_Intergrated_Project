using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.gameObject.transform.position = other.gameObject.transform.parent.gameObject.GetComponent<Car>().resetPosition;
            other.gameObject.transform.parent.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}