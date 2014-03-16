using UnityEngine;
using System.Collections;

public class blockMaterial : MonoBehaviour
{
    public Material[] materials;
    public int selected = 0;

    private void Start()
    {
        GetComponent<Renderer>().material = materials[selected];
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
        Destroy(this);
    }

    public void UpdateTexture()
    {
        GetComponent<Renderer>().material = materials[selected];
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
}