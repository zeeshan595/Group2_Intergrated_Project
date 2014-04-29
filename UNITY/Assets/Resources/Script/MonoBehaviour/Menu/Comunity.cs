using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Comunity : MonoBehaviour
{
    public GUISkin skin;

    private List<MySQL> levels = new List<MySQL>();
    private int selectedLevel = -1;

    private Vector2 scrollView = Vector2.zero;
    private string searchBox = "";

    private void Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("q", "SELECT * FROM `levels` WHERE `Published` = '1' ORDER BY `Time` DESC LIMIT 50");

        WWW w = new WWW("http://impossiblesix.net/InGame/returnQuery.php", form);

        StartCoroutine(getLevels(w));
    }

    private void OnGUI()
    {
        GUI.skin = skin;

        if (selectedLevel == -1)
            GUI.Window(3, new Rect(125, 5, Screen.width - 240, Screen.height - 10), windowFunc, "");
        else
        {
            GUI.Window(3, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400), levelEdit, "");
        }

        if (Event.current.type == EventType.keyDown && Event.current.character == '\n')
        {
            WWWForm form = new WWWForm();
            form.AddField("q", "SELECT * FROM `levels` WHERE `Published` = '1' AND `Name` LIKE '%" + searchBox + "%' ORDER BY `Time` DESC LIMIT 50");

            WWW w = new WWW("http://impossiblesix.net/InGame/returnQuery.php", form);

            StartCoroutine(getLevels(w));
        }
    }

    private void levelEdit(int id)
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Back", GUILayout.Width(190)))
        {
            selectedLevel = -1;
            return;
        }

        if (GUILayout.Button("Play"))
        {
            LevelLoader.levelData = levels[selectedLevel].data[levels[selectedLevel].Find("Level")].data;
            Application.LoadLevel("game");
        }

        GUILayout.EndHorizontal();

        scrollView = GUILayout.BeginScrollView(scrollView);

        GUILayout.Label(levels[selectedLevel].data[levels[selectedLevel].Find("Name")].data);
        GUILayout.Label(levels[selectedLevel].data[levels[selectedLevel].Find("Description")].data);

        GUILayout.EndScrollView();
    }

    private void windowFunc(int id)
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Back", GUILayout.Width(50)))
        {
            GetComponent<Menu>().buttonActive(7);
            this.enabled = false;
        }

        if (GUILayout.Button("Featured Levels", GUILayout.Width(110)))
        {
            WWWForm form = new WWWForm();
            form.AddField("q", "SELECT * FROM `levels` WHERE `Published` = '1' ORDER BY `Time` DESC LIMIT 50");

            WWW w = new WWW("http://impossiblesix.net/InGame/returnQuery.php", form);

            StartCoroutine(getLevels(w));
        }

        searchBox = GUILayout.TextField(searchBox);

        if (GUILayout.Button("Search", GUILayout.Width(75)))
        {
            WWWForm form = new WWWForm();
            form.AddField("q", "SELECT * FROM `levels` WHERE `Published` = '1' AND `Name` LIKE '%" + searchBox + "%' ORDER BY `Time` DESC LIMIT 50");

            WWW w = new WWW("http://impossiblesix.net/InGame/returnQuery.php", form);

            StartCoroutine(getLevels(w));
        }

        GUILayout.EndHorizontal();

        scrollView = GUILayout.BeginScrollView(scrollView);
        
        for (int x = 0; x < levels.Count; x++)
        {
            if (GUILayout.Button(levels[x].data[levels[x].Find("Name")].data))
            {
                selectedLevel = x;
            }
        }

        GUILayout.EndScrollView();
    }

    private IEnumerator getLevels(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            levels.Clear();
            levels = new List<MySQL>();
            MatchCollection matches = Regex.Matches(w.text, @"Array[\w\s\[\]\=\>\(\)\{\}\.\,\-\;\!\@\$\%\^\&\*]+\#");
            foreach (Match m in matches)
                levels.Add(new MySQL(m.ToString()));
        }
        else
        {
            Debug.Log(w.error);
        }
    }
}
