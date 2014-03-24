using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{
    private void OnGUI()
    {
        GUI.Window(4, new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200), windowFunc, "");
    }

    private void windowFunc(int id)
    {
        if (GUILayout.Button("back"))
        {
            GetComponent<Menu>().enabled = true;
            this.enabled = false;
        }
        if (GUILayout.Button("Action"))
        {
            Settings.carType = Settings.CarType.Action;
            Application.LoadLevel("tutorial");
        }

        if (GUILayout.Button("Cyberpunk"))
        {
            Settings.carType = Settings.CarType.Cyberpunk;
            Application.LoadLevel("tutorial");
        }

        if (GUILayout.Button("Romance"))
        {
            Settings.carType = Settings.CarType.Romance;
            Application.LoadLevel("tutorial");
        }

        if (GUILayout.Button("Science Fiction"))
        {
            Settings.carType = Settings.CarType.ScienceFiction;
            Application.LoadLevel("tutorial");
        }

        if (GUILayout.Button("Fantasy"))
        {
            Settings.carType = Settings.CarType.Fantasy;
            Application.LoadLevel("tutorial");
        }

        if (GUILayout.Button("Adventure"))
        {
            Settings.carType = Settings.CarType.Adventure;
            Application.LoadLevel("tutorial");
        }
    }
}