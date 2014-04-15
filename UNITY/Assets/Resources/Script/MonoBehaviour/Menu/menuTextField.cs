using UnityEngine;
using System.Collections;

public class menuTextField : MonoBehaviour
{
    public int maxLength = 12;
    public bool isPassword = false;
    public menuButtons buttonOnEnter;
    public GameObject nextObject;

    private bool mouseOver = false;
    [System.NonSerialized]
    private bool editingMode = true;
    [System.NonSerialized]
    public string text = "";

    private Color normalColor;

    private void Start()
    {
        normalColor = renderer.material.color;
    }

    private void Update()
    {
        if (editingMode)
        {
            if (buttonOnEnter != null && (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)))
            {
                buttonOnEnter.GetComponent<menuButtons>().buttonActive();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return))
            {
                renderer.material.color = normalColor;
                mouseOver = false;
                editingMode = false;
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && nextObject != null)
            {
                nextObject.GetComponent<menuTextField>().editingMode = true;
                nextObject.renderer.material.color = normalColor * 1.25f;

                renderer.material.color = normalColor;
                mouseOver = false;
                editingMode = false;
            }
            else if (!Input.GetKey(KeyCode.Backspace) && Input.inputString != null && Input.inputString != "" && transform.GetChild(0).GetComponent<TextMesh>().text.Length < maxLength)
            {
                if (isPassword)
                    transform.GetChild(0).GetComponent<TextMesh>().text += "*";
                else
                    transform.GetChild(0).GetComponent<TextMesh>().text += Input.inputString;

                text += Input.inputString;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace) && transform.GetChild(0).GetComponent<TextMesh>().text.Length > 0)
            {
                transform.GetChild(0).GetComponent<TextMesh>().text = transform.GetChild(0).GetComponent<TextMesh>().text.Substring(0, transform.GetChild(0).GetComponent<TextMesh>().text.Length - 1);
                text = text.Substring(0, text.Length - 1);
            }
        }

        if (mouseOver && Input.GetKeyUp(KeyCode.Mouse0))
        {
            editingMode = true;
        }
    }

    private void OnMouseEnter()
    {
        renderer.material.color = normalColor * 1.25f;
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (!editingMode)
        {
            renderer.material.color = normalColor;
            mouseOver = false;
        }
    }

    public void updateText(string t)
    {
        transform.GetChild(0).GetComponent<TextMesh>().text = t;
        text = t;
    }
}