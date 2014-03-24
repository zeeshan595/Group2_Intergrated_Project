using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour
{
    private void OnGUI()
    {
        GUI.Window(1, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 125, 300, 250), windowFunc, "");
    }

    private void windowFunc(int id)
    {
        GUILayout.Label("Alex McCauley - Illustration, Concept Art");
        GUILayout.Label("Andrew Smith - Programmer");
        GUILayout.Label("Beatrice - Animator, Artist");
        GUILayout.Label("Kati Salminen - Artist");
        GUILayout.Label("Thomas Agnew - Computer Games (Design)");
        GUILayout.Label("Zeeshan Abid - Programmer");
        if (GUILayout.Button("Back"))
        {
            GetComponent<Menu>().enabled = true;
            this.enabled = false;
        }
    }
}