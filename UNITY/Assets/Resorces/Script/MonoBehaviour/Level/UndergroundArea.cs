using UnityEngine;
using System.Collections;

public class UndergroundArea : MonoBehaviour
{
    public Light[] lights;

    private void OnTriggerEnter()
    {
        for (int x = 0; x < lights.Length; x++ )
        {
            lights[x].GetComponent<Light>().enabled = false;
        }
    }

    private void OnTriggerExit()
    {
        for (int x = 0; x < lights.Length; x++)
        {
            lights[x].GetComponent<Light>().enabled = true;
        }
    }
}