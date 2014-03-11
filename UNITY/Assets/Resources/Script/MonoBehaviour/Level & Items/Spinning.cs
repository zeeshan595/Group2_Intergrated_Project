using UnityEngine;
using System.Collections;

public class Spinning : MonoBehaviour
{
    public bool x = false, y = false, z = false;
    public float speed = 5.0f;

    private void Update()
    {
        Vector3 rotater = Vector3.zero;
        if (x)
            rotater.x++;
        if (y)
            rotater.y++;
        if (z)
            rotater.z++;
        transform.Rotate(rotater * speed * Time.deltaTime);
    }
}