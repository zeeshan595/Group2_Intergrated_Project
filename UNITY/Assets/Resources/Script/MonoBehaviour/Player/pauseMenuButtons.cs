using UnityEngine;
using System.Collections;

public class pauseMenuButtons : MonoBehaviour
{
    public enum PauseButtonType
    {
        normal = 0,
        quit = 1,
        restart = 2
    }

    public GameObject pauseMenu;
    public PauseButtonType type;

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
        GetComponent<TextMesh>().color = Color.black;

        mouseOver = true;
    }

    private void OnMouseExit()
    {
        GetComponent<TextMesh>().color = Color.red;

        mouseOver = false;
    }

    public void buttonActive()
    {
        GetComponent<TextMesh>().color = Color.red;
        mouseOver = false;

        pauseMenu.GetComponent<PauseMenu>().changeState();

        if (type == PauseButtonType.quit)
            Application.LoadLevel("menu");
        else if (type == PauseButtonType.restart)
            Application.LoadLevel(Application.loadedLevel);
    }
}