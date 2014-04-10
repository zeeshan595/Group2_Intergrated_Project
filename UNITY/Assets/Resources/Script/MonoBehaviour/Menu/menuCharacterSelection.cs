using UnityEngine;
using System.Collections;

public class menuCharacterSelection : MonoBehaviour
{
    public Settings.CarType type;
    public GameObject startMenu;

    private bool mouseOver = false;

    private void Start()
    {
        if (GetComponent<Renderer>())
            renderer.material.SetFloat("_Shininess", 0.1f);
    }

    private void Update()
    {
        if (mouseOver && Input.GetKeyUp(KeyCode.Mouse0))
        {
            mouseOver = false;
            if (GetComponent<Renderer>())
                renderer.material.SetFloat("_Shininess", 0.1f);
            Settings.carType = type;
            Camera.main.GetComponent<Menu>().buttonActive(6);
        }
    }

    private void OnMouseEnter()
    {
        if (GetComponent<Renderer>())
            renderer.material.SetFloat("_Shininess", 1.0f);
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (GetComponent<Renderer>())
            renderer.material.SetFloat("_Shininess", 0.1f);
        mouseOver = false;
    }
}