using UnityEngine;
using System.Collections;

public class BolderTrigger : MonoBehaviour
{
    public GameObject bolder;

    private Vector3 defaultPos;

    private void Start()
    {
        defaultPos = bolder.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bolder.rigidbody.isKinematic = false;
            bolder.rigidbody.AddForce(-Vector3.up * 2000);
        }
    }

    public void resetCar()
    {
        bolder.transform.position = defaultPos;
        bolder.rigidbody.isKinematic = true;
    }
}