using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float cameraSpeed;
    public float height;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(0, 0, -height), Time.deltaTime * cameraSpeed);
        GetComponent<Camera>().orthographicSize = Mathf.Log((target.rigidbody.velocity.magnitude * 50) + 25);
    }
}