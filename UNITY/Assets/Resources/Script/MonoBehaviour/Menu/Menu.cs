using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("Action"))
        {
            Settings.carType = Settings.CarType.Action;
            Application.LoadLevel("Tutorial");
        }

        if (GUILayout.Button("Romance"))
        {
            Settings.carType = Settings.CarType.Romance;
            Application.LoadLevel("Tutorial");
        }

        if (GUILayout.Button("Fantasy"))
        {
            Settings.carType = Settings.CarType.Fantasy;
            Application.LoadLevel("Tutorial");
        }
    }
}
