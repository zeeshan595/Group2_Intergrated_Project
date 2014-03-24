using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    #region Variables

    public GameObject target;
    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;

    #endregion

    private void Update()
    {
        distance = Mathf.Lerp(distance, Mathf.Clamp(Mathf.Log(target.rigidbody.velocity.magnitude) * 2.3f, 3, 15), Time.deltaTime * 3);
    }

    private void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(Mathf.Clamp(-target.transform.TransformDirection(target.rigidbody.velocity).z, -4, 4), height, -distance), Time.deltaTime * 2);
    }
}