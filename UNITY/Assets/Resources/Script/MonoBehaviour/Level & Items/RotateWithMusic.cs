using UnityEngine;
using System.Collections;

public class RotateWithMusic : MonoBehaviour
{
    public GameObject musicObj;

    private float randomTimer = 1;

    private void Start()
    {
        randomTimer = Random.Range(-3.0f, 3.0f);
    }

    private void LateUpdate()
    {
        float[] samples = new float[1024];
        musicObj.audio.GetOutputData(samples, 0);
        float value = Mathf.Clamp(Mathf.Abs(samples[1020] * 6), 1.1f, 3.0f);
        transform.Rotate(new Vector3(0, value * randomTimer, 0));
    }
}