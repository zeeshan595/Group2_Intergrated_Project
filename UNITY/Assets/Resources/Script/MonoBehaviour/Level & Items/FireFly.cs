using UnityEngine;
using System.Collections;

public class FireFly : MonoBehaviour
{
    private float height = 0;
    private float randomSpeed = 0;

    private void Start()
    {
        if (GetComponent<BoxCollider>())
            Destroy(GetComponent<BoxCollider>());

        randomSpeed = Random.Range(0.0f, 1.0f);
        height = Random.Range(0, 360);
    }

    private void Update()
    {
        transform.position += new Vector3(0, Mathf.Sin(height) * randomSpeed, 0) * Time.deltaTime;

        height += Time.deltaTime;
    }
}