using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public float height = 5;
    public float speed = 2.0f;

    private bool playerOn = false;
    private float normalHeight = 0;
    private int wheelsInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            wheelsInside++;
            if (wheelsInside > 1)
            {
                StartCoroutine(changeStatus(0.3f, true));
                other.transform.parent.gameObject.transform.parent = gameObject.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            wheelsInside--;
            if (wheelsInside == 0 && playerOn)
            {
                StartCoroutine(changeStatus(1, false));
                other.transform.parent.gameObject.transform.parent = null;
            }
        }
    }

    private void Start()
    {
        normalHeight = transform.position.y;
        height += normalHeight;
    }

    private void Update()
    {
        if (PauseMenu.isPaused)
            return;

        if (playerOn)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, height, transform.position.z), (Time.deltaTime / Mathf.Abs(height - normalHeight)) * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, normalHeight, transform.position.z), (Time.deltaTime / Mathf.Abs(height - normalHeight)) * speed);
        }
    }

    private IEnumerator changeStatus(float t, bool b)
    {
        yield return new WaitForSeconds(t);
        playerOn = b;
    }
}