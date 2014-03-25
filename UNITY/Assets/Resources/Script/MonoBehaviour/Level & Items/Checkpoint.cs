using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public Texture[] frames;

    private int currentFrame = 0;
    private bool reverse = false;
    private bool activate = false;

    private void Start()
    {
        InvokeRepeating("UpdateFrame", 0.1f, 0.05f);
    }

    private void UpdateFrame()
    {
        if (activate)
        {
            if ((frames.Length - 1) == currentFrame)
                reverse = true;
            else if (currentFrame == 0)
                reverse = false;

            if (!reverse)
                currentFrame++;
            else
                currentFrame--;
        }
    }

    private void Update()
    {
        transform.parent.renderer.material.mainTexture = frames[currentFrame];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activate = true;
            if (other.transform.parent.GetComponent<Car>())
                other.transform.parent.GetComponent<Car>().resetPosition = transform.position;
        }
    }
}