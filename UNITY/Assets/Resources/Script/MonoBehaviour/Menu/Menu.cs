using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Menu : MonoBehaviour
{
    public GUISkin skin;
    public Texture background;

    private Vector2 scrollView = Vector2.zero;
    private MySQL levels;

    private void Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("q", "SELECT * FROM `levels` WHERE `Author` = '" + Settings.Username + "'");

        WWW w = new WWW("http://impossiblesix.net/InGame/returnQuery.php", form);

        StartCoroutine(getAuthorLevels(w));
    }

    private void OnGUI()
    {
        GUI.skin = skin;

        if (background != null)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        GUI.Window(1, new Rect((Screen.width / 2) - 132, (Screen.height / 2) - 150, 264, 300), windowFunc, "");

        if (GUI.Button(new Rect(125, 5, 49, 51), "FB", skin.customStyles[0]))
            System.Diagnostics.Process.Start("https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl");

        if (GUI.Button(new Rect(125, 60, 49, 51), "T", skin.customStyles[0]))
            System.Diagnostics.Process.Start("http://impossiblesix.net");

        if (GUI.Button(new Rect(125, 120, 49, 51), "W", skin.customStyles[0]))
            System.Diagnostics.Process.Start("http://impossiblesix.net");
    }

    private void windowFunc(int id)
    {
        scrollView = GUILayout.BeginScrollView(scrollView);

        if (GUILayout.Button("Play"))
        {

        }

        if (GUILayout.Button("Comunity Levels"))
        {
            Application.LoadLevel("comunityLevels");
        }

        if (GUILayout.Button("Create"))
        {
            
        }

        if (GUILayout.Button("Options"))
        {

        }

        GUILayout.EndScrollView();

        /*
        for (int x = 0; x < levels.data.Count; x++)
        {
            if (GUILayout.Button(levels.data[x].name))
            {
                //LevelEditor.levelID = levels.data
            }
        }
        */
    }

    private IEnumerator getAuthorLevels(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            MatchCollection matches = Regex.Matches(w.text, MySQL.regularExp);
            
        }
        else
        {
            Debug.Log(w.error);
        }
    }
}