using UnityEngine;
using System.Collections;

public class blockMaterial : MonoBehaviour
{
    public Material[] materials;

    private void Start()
    {
        GetComponent<Renderer>().material = materials[(int)Settings.carType];
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
        Destroy(this);
    }

    public void UpdateTexture()
    {
        GetComponent<Renderer>().material = materials[(int)Settings.carType];
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
}