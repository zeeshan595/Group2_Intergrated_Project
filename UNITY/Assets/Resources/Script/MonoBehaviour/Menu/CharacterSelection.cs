using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{
    public Texture2D action;
    public Texture2D fantasy;
    public Texture2D adventure;
    public Texture2D scifi;
    public Texture2D romance;
    public Texture2D cyberpuk;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 138, Screen.height / 2 - 210, 128, 200), action, GUIStyle.none))
        {
            Settings.carType = Settings.CarType.Action;
            Application.LoadLevel("tutorial");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 138, Screen.height / 2, 128, 200), fantasy, GUIStyle.none))
        {
            Settings.carType = Settings.CarType.Fantasy;
            Application.LoadLevel("tutorial");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 138, Screen.height / 2 + 210, 128, 200), adventure, GUIStyle.none))
        {
            Settings.carType = Settings.CarType.Adventure;
            Application.LoadLevel("tutorial");
        }

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 - 210, 128, 200), scifi, GUIStyle.none))
        {
            Settings.carType = Settings.CarType.ScienceFiction;
            Application.LoadLevel("tutorial");
        }

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 128, 200), romance, GUIStyle.none))
        {
            Settings.carType = Settings.CarType.Romance;
            Application.LoadLevel("tutorial");
        }

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 210, 128, 200), cyberpuk, GUIStyle.none))
        {
            Settings.carType = Settings.CarType.Cyberpunk;
            Application.LoadLevel("tutorial");
        }
    }
}