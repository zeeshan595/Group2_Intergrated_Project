using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{
    public float Starter = 0;
    public float movmentAmount = 1.3f;
    public float movmentSpeed = 2;

    private Vector2 movingSin = Vector2.zero;
    private Vector3 normalPos;

    private void Start()
    {
        normalPos = transform.position;
        movingSin.x = Starter * 0.5f;
        movingSin.y = Starter;
    }

    private void Update()
    {
        transform.position = normalPos + new Vector3(Mathf.Sin(movingSin.x) * 0.1f * movmentAmount, Mathf.Sin(movingSin.y) * 0.1f * movmentAmount, 0);

        movingSin.x += Time.deltaTime * 0.5f * movmentSpeed;
        movingSin.y += Time.deltaTime * movmentSpeed;
    }
}