using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public enum Type
    {
        totem = 0,
        tree = 1
    }

    public Type type;
    public Texture[] frames;
    public GameObject glowLight;
    public GameObject lantern;

    private int currentFrame = 0;
    private bool reverse = false;
    private bool activate = false;
    private float sinWave = 0;

    private void Start()
    {
        if (type == Type.totem)
            InvokeRepeating("UpdateFrame", 0.1f, 0.05f);
        else if (type == Type.tree)
        {
            glowLight.SetActive(false);
        }
    }

    private void UpdateFrame()
    {
        if (activate)
        {
            if (type == Type.totem)
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
    }

    private void Update()
    {
        if (type == Type.totem)
            transform.parent.renderer.material.mainTexture = frames[currentFrame];
        else if (type == Type.tree)
        {
            if (activate)
            {
                glowLight.SetActive(true);
                lantern.transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(sinWave) * 10);
            }
        }

        if (!activate)
            sinWave += Time.deltaTime;
        else
            sinWave += Time.deltaTime * 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activate = true;
            sinWave = 0;
            if (other.transform.parent.GetComponent<Car>())
                other.transform.parent.GetComponent<Car>().resetPosition = transform.position;
            else if (other.GetComponent<TheLog>())
                other.GetComponent<TheLog>().resetPosition = transform.position;
        }
    }
}