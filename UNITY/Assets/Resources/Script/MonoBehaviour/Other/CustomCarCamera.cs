using UnityEngine;
using System.Collections;

public class CustomCarCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel"), 1, 10);
        if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Mouse1))
        {
            Vector2 movment = new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            transform.Translate(movment * camera.orthographicSize * 0.05f);
        }
    }
}