using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LevelEditor : MonoBehaviour
{
    #region Variables

    public GUISkin skin;
    public GameObject[] objects;


    private bool showObjectMenu = false;
    private List<GameObject> objectsSpawned = new List<GameObject>();
    private GameObject selected = null;
    private GameObject lastSelected = null;
    private bool isGrid = false;
    private bool gettingCode = false;
    private string editorData = "";
    private Color[] selectedColor;
    private Vector2 scrollView = Vector2.zero;
    private Vector3 offset = Vector3.zero;

    private string propertiesPositionX = "";
    private string propertiesPositionY = "";
    private string propertiesPositionZ = "";
    private string propertiesRotation = "";

    #endregion

    #region Unity Functions

    private void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (gettingCode)
            return;

        if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Mouse1))
        {
            Vector2 movment = new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            transform.Translate(movment * 0.3f);
        }

        if (Input.GetKey(KeyCode.Mouse0) && selected != null)
        {
            Vector3 movment = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movment.z = selected.transform.position.z;
            selected.transform.position = movment - offset;
            propertiesPositionX = movment.x.ToString();
            propertiesPositionY = movment.y.ToString();
            propertiesPositionZ = movment.z.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                selected = hit.collider.gameObject;
                while (selected.transform.parent != null)
                    selected = selected.transform.parent.gameObject;

                if (selected.rigidbody)
                    selected.rigidbody.isKinematic = true;

                lastSelected = selected;
                offset = new Vector3(hit.point.x, hit.point.y, selected.transform.position.z) - selected.transform.position;
                propertiesRotation = selected.transform.eulerAngles.z.ToString();

                MeshRenderer[] renderers = selected.GetComponentsInChildren<MeshRenderer>();
                selectedColor = new Color[renderers.Length];
                for (int x = 0; x < renderers.Length; x++)
                {
                    selectedColor[x] = renderers[x].material.color;
                    renderers[x].material.color = new Color(1, 1, 1, 0.3f);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && selected != null)
        {
            if (selected.rigidbody)
                selected.rigidbody.isKinematic = false;

            MeshRenderer[] renderers = selected.GetComponentsInChildren<MeshRenderer>();
            for (int x = 0; x < renderers.Length; x++)
            {
                renderers[x].material.color = selectedColor[x];
            }

            selected = null;
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (selected != null)
            {
                objectsSpawned.Remove(selected);
                Destroy(selected);
            }
            else if (lastSelected != null)
            {
                objectsSpawned.Remove(lastSelected);
                Destroy(lastSelected);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (lastSelected != null)
            {
                transform.position = new Vector3(lastSelected.transform.position.x, lastSelected.transform.position.y, transform.position.z);
                float maxSize = Mathf.Max(new float[3] { lastSelected.transform.localScale.x, lastSelected.transform.localScale.y, lastSelected.transform.localScale.z });
                camera.orthographicSize = 3 / maxSize;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            gameObject.camera.orthographicSize = Mathf.Clamp(gameObject.camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel"), 1, 50);
    }

    private void OnGUI()
    {
        if (skin != null)
            GUI.skin = skin;

        if (!gettingCode)
        {

            if (showObjectMenu)
            {
                if (GUI.Button(new Rect(5, 5, 50, 20), "Hide"))
                    showObjectMenu = false;

                GUI.Window(0, new Rect(5, 30, 200, Screen.height - 35), objectWindow, "Object");
                GUI.Window(1, new Rect(Screen.width - 205, 5, 200, Screen.height - 10), propertiesWindow, "Properties");
            }
            else
            {
                if (GUI.Button(new Rect(5, 5, 50, 20), "Show"))
                {
                    showObjectMenu = true;
                }
            }

            if (GUI.Button(new Rect(60, 5, 80, 20), "Grid: " + isGrid.ToString()))
            {
                isGrid = !isGrid;
            }

            if (GUI.Button(new Rect(145, 5, 100, 20), "Edit Code"))
            {
                editorData = SaveLevel();
                gettingCode = true;
            }
            if (Time.timeScale == 0)
            {
                if (GUI.Button(new Rect(250, 5, 100, 20), "Play"))
                {
                    GetComponent<Camera>().enabled = false;
                    GetComponent<AudioListener>().enabled = false;

                    foreach (GameObject g in objectsSpawned)
                    {
                        MonoBehaviour[] behaviours = g.GetComponents<MonoBehaviour>();
                        foreach (MonoBehaviour m in behaviours)
                            m.enabled = true;
                    }
                    editorData = SaveLevel();
                    Time.timeScale = 1;
                }
            }
            else
            {
                if (GUI.Button(new Rect(250, 5, 100, 20), "Stop"))
                {
                    GetComponent<Camera>().enabled = true;
                    GetComponent<AudioListener>().enabled = true;

                    Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
                    foreach(Object o in objects)
                    {
                        if ((GameObject)o != gameObject)
                            Destroy((GameObject)o);
                    }

                    Time.timeScale = 0;
                    loadLevel(editorData);
                }
            }
        }
        else
        {
            editorData = GUI.TextArea(new Rect(5, 5, Screen.width - 10, Screen.height - 30), editorData);
            if (GUI.Button(new Rect(Screen.width - 55, Screen.height - 25, 50, 20), "Close"))
            {
                loadLevel(editorData);
                gettingCode = false;
            }
        }
    }

    #endregion

    #region windows

    private void objectWindow(int id)
    {
        scrollView = GUILayout.BeginScrollView(scrollView);
        for (int x = 0; x < objects.Length; x++)
        {
            if (GUILayout.Button(objects[x].name))
            {
                GameObject spwn = (GameObject)Instantiate(objects[x], new Vector3(transform.position.x, transform.position.y, objects[x].transform.position.z), objects[x].transform.rotation);
                if (Time.timeScale == 0)
                {
                    spwn.name = objects[x].name;
                    MonoBehaviour[] behaviours = spwn.GetComponents<MonoBehaviour>();
                    foreach (MonoBehaviour m in behaviours)
                        m.enabled = false;

                    objectsSpawned.Add(spwn);
                }
                lastSelected = spwn;
            }
        }
        GUILayout.EndScrollView();
    }

    private void propertiesWindow(int id)
    {
        if (selected != null)
        {
            DisplayProperties(selected);
        }
        else if (lastSelected != null)
        {
            DisplayProperties(lastSelected);
        }
    }

    private void DisplayProperties(GameObject obj)
    {
        GUILayout.Label("Name: " + obj.name);

        GUILayout.Label("Position: ");
        GUILayout.BeginHorizontal();

        propertiesPositionX = GUILayout.TextField(propertiesPositionX, GUILayout.Width(60));
        propertiesPositionY = GUILayout.TextField(propertiesPositionY, GUILayout.Width(60));
        propertiesPositionZ = GUILayout.TextField(propertiesPositionZ, GUILayout.Width(60));

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        GUILayout.Label("Rotation: ", GUILayout.Width(52));
        propertiesRotation = GUILayout.TextField(propertiesRotation);

        GUILayout.EndHorizontal();


        if (Event.current.type == EventType.keyDown && Event.current.character == '\n')
        {
            saveProperties(obj);
        }
    }

    private void saveProperties(GameObject obj)
    {
        obj.transform.position = stringToVector3("( " + propertiesPositionX + ", " + propertiesPositionY + ", " + propertiesPositionZ + " )");
        obj.transform.eulerAngles = new Vector3(0, 0, float.Parse(propertiesRotation));
    }

    #endregion

    #region save & load

    private string SaveLevel()
    {
        string levelData = "";
        for (int x = 0; x < objectsSpawned.Count; x++)
        {
            levelData += objectsSpawned[x].name + "{ \n";
            levelData += "  position=" + objectsSpawned[x].transform.position + ";\n";
            levelData += "  rotation=" + objectsSpawned[x].transform.eulerAngles + ";\n";
            levelData += " }\n";
        }

        return levelData;
    }

    private void loadLevel(string levelData)
    {
        foreach (GameObject g in objectsSpawned)
            Destroy(g);

        objectsSpawned.Clear();
        MatchCollection objectsMatched = Regex.Matches(levelData, @"([a-zA-Z0-9\(\)\ ]+){\s*(\s*[a-zA-Z]+\s*=\s*[a-zA-Z0-9\(\.\,\ \)\-]+;\s*)+\s*}");
        foreach (Match o in objectsMatched)
        {
            GameObject spwn;
            for (int x = 0; x < objects.Length; x++)
            {
                if (o.Groups[1].ToString() == objects[x].name)
                {
                    spwn = (GameObject)Instantiate(objects[x], new Vector3(transform.position.x, transform.position.y, objects[x].transform.position.z), objects[x].transform.rotation);
                    spwn.name = objects[x].name;
                    MonoBehaviour[] behaviours = spwn.GetComponents<MonoBehaviour>();
                    foreach (MonoBehaviour m in behaviours)
                        m.enabled = false;
                    objectsSpawned.Add(spwn);

                    MatchCollection attributes = Regex.Matches(o.ToString(), @"([a-zA-Z]+)\s*=\s*([a-zA-Z0-9\(\.\,\ \)\-]+);");
                    foreach (Match a in attributes)
                    {
                        string name = a.Groups[1].ToString();
                        string value = a.Groups[2].ToString();

                        switch (name)
                        {
                            case "position":
                                spwn.transform.position = stringToVector3(value);
                                break;
                            case "rotation":
                                spwn.transform.eulerAngles = stringToVector3(value);
                                break;
                        }
                    }
                    break;
                }
            }
        }
    }

    #endregion

    #region Helpers

    private Vector3 stringToVector3(string vector)
    {
        Vector3 value = Vector3.zero;

        MatchCollection matches = Regex.Matches(vector, @"([0-9\.\-]+)+");

        value.x = float.Parse(matches[0].ToString());
        value.y = float.Parse(matches[1].ToString());
        value.z = float.Parse(matches[2].ToString());

        return value;
    }

    #endregion
}