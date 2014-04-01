using UnityEngine;
using System.Collections;

public class TNTTrigger : MonoBehaviour
{
    public GameObject TNT;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent.transform.rigidbody.AddForce(new Vector3(1, 0.3f, 0) * 1000);
            Instantiate(TNT.GetComponent<TNT>().explosion, TNT.transform.position, Quaternion.identity);
            Destroy(TNT);
            Destroy(gameObject);
        }
    }
}