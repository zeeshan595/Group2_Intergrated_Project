using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{
    private void Start()
    {
        if (gameObject.renderer)
        {
            Destroy(gameObject.renderer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.gameObject.rigidbody.velocity = Vector3.zero;
            other.gameObject.transform.parent.gameObject.rigidbody.angularVelocity = Vector3.zero;
            other.gameObject.transform.parent.gameObject.transform.position = other.gameObject.transform.parent.gameObject.GetComponent<Car>().resetPosition;
            other.gameObject.transform.parent.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}