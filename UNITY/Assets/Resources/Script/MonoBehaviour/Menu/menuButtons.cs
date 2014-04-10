using UnityEngine;
using System.Collections;

public class menuButtons : MonoBehaviour
{
    public int id = 0;
    public bool blackMode = false;

    private bool mouseOver = false;

    private void Update()
    {
        if (mouseOver && Input.GetKeyUp(KeyCode.Mouse0))
        {
            buttonActive();
        }
    }

    private void OnMouseEnter()
    {
        if (!blackMode)
            GetComponent<TextMesh>().color = Color.black;
        else
            GetComponent<TextMesh>().color = Color.red;

        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (!blackMode)
            GetComponent<TextMesh>().color = Color.red;
        else
            GetComponent<TextMesh>().color = Color.black;

        mouseOver = false;
    }

    public void buttonActive()
    {
        if (!blackMode)
            GetComponent<TextMesh>().color = Color.red;
        else
            GetComponent<TextMesh>().color = Color.black;

        mouseOver = false;

        Camera.main.gameObject.GetComponent<Menu>().buttonActive(id);
    }
}