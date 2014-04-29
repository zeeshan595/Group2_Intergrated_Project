using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
    public string levelName = "";

    private bool mouseOver = false;

    private void Update()
    {
        if (mouseOver && Input.GetKeyDown(KeyCode.Mouse0))
            Application.LoadLevel(levelName);
    }

    private void OnMouseEnter()
    {
        mouseOver = true;

        if (GetComponent<TextMesh>())
            GetComponent<TextMesh>().color = Color.black;
        else
            transform.GetChild(0).GetComponent<TextMesh>().color = Color.black;
    }

    private void OnMouseExit()
    {
        mouseOver = false;

        if (GetComponent<TextMesh>())
            GetComponent<TextMesh>().color = Color.red;
        else
            transform.GetChild(0).GetComponent<TextMesh>().color = Color.red;
    }
}