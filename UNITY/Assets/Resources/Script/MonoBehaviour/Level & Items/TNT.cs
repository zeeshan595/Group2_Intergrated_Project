using UnityEngine;
using System.Collections;

public class TNT : MonoBehaviour
{
    public float level = 3.0f;
    public GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > level)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}