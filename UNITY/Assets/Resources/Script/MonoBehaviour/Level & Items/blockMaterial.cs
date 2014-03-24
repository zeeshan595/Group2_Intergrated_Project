using UnityEngine;
using System.Collections;

public class blockMaterial : MonoBehaviour
{
    public Material[] materials;
    public int selectedMaterial = 0;

    private void Start()
    {
        UpdateTexture();
    }

    public void UpdateTexture()
    {
        GetComponent<Renderer>().material = materials[selectedMaterial];
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
        if ((Settings.CarType)selectedMaterial == Settings.CarType.Fantasy)
        {
            transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            transform.GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
        }
        else if (transform.childCount > 0)
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }
}