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
    // How much we 
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

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

        /*
        // Calculate the current rotation angles
        var wantedRotationAngle = target.transform.eulerAngles.y;
        var wantedHeight = target.transform.position.y + height;

        if (target.transform.InverseTransformDirection(target.rigidbody.velocity).z < -0.1f)
            wantedRotationAngle += 180;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.transform.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        transform.LookAt(target.transform.position);
         */

        transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(distance, height, 0), Time.deltaTime * 2);
        GetComponent<Camera>().orthographicSize = distance;
    }
}