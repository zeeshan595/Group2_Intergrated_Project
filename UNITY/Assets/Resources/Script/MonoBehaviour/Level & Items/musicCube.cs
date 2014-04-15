using UnityEngine;
using System.Collections;

public class musicCube : MonoBehaviour
{
    public GameObject musicObj;

    private GameObject[] cubes;

    private void Start()
    {
        cubes = new GameObject[transform.childCount];
        for (int x = 0; x < cubes.Length; x++)
        {
            cubes[x] = transform.GetChild(x).gameObject;
        }
    }

    private void LateUpdate()
    {
        float[] samples = new float[1024];
        musicObj.audio.GetOutputData(samples, 0);
        float value = Mathf.Clamp(Mathf.Abs(samples[1020] * 6), 1.1f, 3.0f);

        foreach (GameObject g in cubes)
            g.transform.localScale = Vector3.Lerp(g.transform.localScale, new Vector3(g.transform.localScale.x, value, g.transform.localScale.z), Time.deltaTime * 5);
    }
}