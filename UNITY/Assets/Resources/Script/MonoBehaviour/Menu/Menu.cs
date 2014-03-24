using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GUISkin skin;
    public Texture background;

    private Vector2 scrollView = Vector2.zero;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void OnGUI()
    {
        GUI.skin = skin;

        if (background != null)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        GUI.Window(1, new Rect((Screen.width / 2) - 132, (Screen.height / 2) - 150, 264, 300), windowFunc, "");

#if UNITY_WEBPLAYER
        if (GUI.Button(new Rect(125, 5, 49, 51), "FB", skin.customStyles[0]))
            Application.ExternalEval("window.open('https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl','_blank')");

        if (GUI.Button(new Rect(125, 60, 49, 51), "T", skin.customStyles[0]))
            Application.ExternalEval("window.open('https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl','_blank')");

        if (GUI.Button(new Rect(125, 120, 49, 51), "W", skin.customStyles[0]))
            Application.ExternalEval("window.open('http://impossiblesix.net','_blank')");


#else
        if (GUI.Button(new Rect(125, 5, 49, 51), "FB", skin.customStyles[0]))
            System.Diagnostics.Process.Start("https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl");

        if (GUI.Button(new Rect(125, 60, 49, 51), "T", skin.customStyles[0]))
            System.Diagnostics.Process.Start("http://impossiblesix.net");

        if (GUI.Button(new Rect(125, 120, 49, 51), "W", skin.customStyles[0]))
            System.Diagnostics.Process.Start("http://impossiblesix.net");
#endif
    }

    private void windowFunc(int id)
    {
        scrollView = GUILayout.BeginScrollView(scrollView);

        if (GUILayout.Button("Play"))
        {
            GetComponent<CharacterSelection>().enabled = true;
            this.enabled = false;
        }

        if (GUILayout.Button("Create"))
        {
            GetComponent<Create>().enabled = true;
            this.enabled = false;
        }

        if (GUILayout.Button("Credits"))
        {
            GetComponent<Credits>().enabled = true;
            this.enabled = false;
        }

        GUILayout.EndScrollView();
    }
}