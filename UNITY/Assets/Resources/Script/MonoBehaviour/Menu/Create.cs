using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Create : MonoBehaviour
{
    private List<MySQL> levels = new List<MySQL>();
    private int selectedLevel = -1;

    private Vector2 scrollView = Vector2.zero;

    private void Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("q", "SELECT * FROM `levels` WHERE `Author` = '" + Settings.Username + "'");

        WWW w = new WWW("http://impossiblesix.net/InGame/returnQuery.php", form);

        StartCoroutine(getAuthorLevels(w));
    }

    private void OnGUI()
    {
        if (selectedLevel == -1)
            GUI.Window(3, new Rect(125, 5, Screen.width - 240, Screen.height - 10), windowFunc, "");
        else
        {
            GUI.Window(3, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400), levelEdit, "");
        }
    }

    private void levelEdit(int id)
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Delete", GUILayout.Width(190)))
        {
            WWWForm form = new WWWForm();
            form.AddField("q", "DELETE FROM `levels` WHERE `Author` = '" + Settings.Username + "' AND `id` = '" + int.Parse(levels[selectedLevel].data[levels[selectedLevel].Find("id")].data) + "'");
            WWW w = new WWW("http://impossiblesix.net/InGame/query.php", form);
            StartCoroutine(deleteLevel(w));

            selectedLevel = -1;
            return;
        }

        if (GUILayout.Button("Edit"))
        {
            LevelEditor.levelID = int.Parse(levels[selectedLevel].data[levels[selectedLevel].Find("id")].data);
            LevelEditor.editorData = levels[selectedLevel].data[levels[selectedLevel].Find("Level")].data;
            LevelEditor.levelName = levels[selectedLevel].data[levels[selectedLevel].Find("Name")].data;
            LevelEditor.levelDescription = levels[selectedLevel].data[levels[selectedLevel].Find("Description")].data;
            Application.LoadLevel("levelEditor");
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Back", GUILayout.Width(190)))
        {
            selectedLevel = -1;
            return;
        }

        if (GUILayout.Button("Play"))
        {
            loadLevel.levelData = levels[selectedLevel].data[levels[selectedLevel].Find("Level")].data;
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

        if (GUILayout.Button("Back"))
        {
            GetComponent<Menu>().enabled = true;
            this.enabled = false;
        }

        if (GUILayout.Button("+ Create New"))
        {
            LevelEditor.levelID = -1;
            Application.LoadLevel("levelEditor");
        }

        GUILayout.EndHorizontal();
        for (int x = 0; x < levels.Count; x++)
        {
            if (GUILayout.Button(levels[x].data[levels[x].Find("Name")].data))
            {
                selectedLevel = x;
            }
        }
    }

    private IEnumerator deleteLevel(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            WWWForm form = new WWWForm();
            form.AddField("q", "SELECT * FROM `levels` WHERE `Author` = '" + Settings.Username + "'");

            WWW www = new WWW("http://impossiblesix.net/InGame/returnQuery.php", form);

            StartCoroutine(getAuthorLevels(www));
        }
        else
            Debug.Log(w.error);
    }

    private IEnumerator getAuthorLevels(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
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
