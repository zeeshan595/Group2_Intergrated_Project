using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public enum Type
    {
        totem = 0,
        tree = 1,
        cyberpunk = 2,
        romance = 3
    }

    public Type type;
    public Texture[] frames;
    public GameObject glowLight;
    public GameObject lantern;
    public Texture replaceImage;
    public Texture[] BeginningAnimation;

    private int currentFrame = 0;
    private bool reverse = false;
    private bool activate = false;
    private float sinWave = 0;
    private bool romanceCycleComplete = false;

    private void Start()
    {
        if (type == Type.totem || type == Type.romance)
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
            else if (type == Type.romance)
            {
                if (!romanceCycleComplete)
                {
                    currentFrame++;
                    if (BeginningAnimation.Length == currentFrame)
                    {
                        currentFrame = 0;
                        romanceCycleComplete = true;
                    }
                }
                else
                {
                    if ((frames.Length - 1) == currentFrame)
                        currentFrame = 0;

                    currentFrame++;
                }
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
        else if (type == Type.romance)
        {
            if (romanceCycleComplete)
                transform.parent.renderer.material.mainTexture = frames[currentFrame];
            else
                transform.parent.renderer.material.mainTexture = BeginningAnimation[currentFrame];
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

            if (type == Type.cyberpunk)
            {
                transform.parent.renderer.material.mainTexture = replaceImage;
                glowLight.SetActive(true);
            }
        }
    }
}