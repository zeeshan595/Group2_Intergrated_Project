using UnityEngine;
using System.Collections;

public class menuScroll : MonoBehaviour
{
    public bool FX = false;

    private bool isActive = false;
    private bool mouseOver = false;

    private void Update()
    {
        if (mouseOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            isActive = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isActive = false;
        }

        if (isActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                float pos = hit.point.x - transform.GetChild(0).transform.position.x;
                transform.GetChild(0).transform.localPosition += new Vector3(Mathf.Clamp(pos / 2, -0.5f, 0.5f), 0, 0);
                if (FX)
                    Settings.FXVolume = transform.GetChild(0).transform.localPosition.x + 0.5f;
                else
                    Settings.MusicVolume = transform.GetChild(0).transform.localPosition.x + 0.5f;
            }
        }
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}