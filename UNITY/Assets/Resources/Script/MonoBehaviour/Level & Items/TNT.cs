using UnityEngine;
using System.Collections;

public class TNT : MonoBehaviour
{
    public float level = 3.0f;
    public GameObject explosion;

    private float previousMagnitude = 0;

    private IEnumerator Start()
    {
        previousMagnitude = rigidbody.velocity.magnitude;
        yield return new WaitForEndOfFrame();
        StartCoroutine(Start());
    }

    private void Update()
    {
        if (Mathf.Abs(rigidbody.velocity.magnitude - previousMagnitude) > level)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}