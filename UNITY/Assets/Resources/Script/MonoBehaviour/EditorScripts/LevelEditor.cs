using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LevelEditor : MonoBehaviour
{
    #region Variables

    public GUISkin skin;
    public GameObject Light;
    public GameObject[] objects;
    public Material[] backgrounds;
    public GameObject spawner;

    private int selectedBackground = 0;
    private bool isBackground = false;
    private bool isSaved = false;
    private bool chosingBackground = false;
    private bool showSettings = false;
    private bool showObjectMenu = true;
    private List<GameObject> objectsSpawned = new List<GameObject>();
    private GameObject selected = null;
    private GameObject lastSelected = null;
    private bool isGrid = true;
    private string editorData = "";
    private Color[] selectedColor;
    private Vector2 scrollView = Vector2.zero;
    private Vector3 offset = Vector3.zero;

    private string propertiesPositionX = "";
    private string propertiesPositionY = "";
    private string propertiesPositionZ = "";
    private string propertiesRotation = "";

    private string scaleX = "";
    private string scaleY = "";

    private Material lineMaterial;

    #endregion

    #region Unity Functions

    private void Start()
    {
        Time.timeScale = 0;

        if (!lineMaterial)
        {
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                "SubShader { Pass { " +
                "    Blend SrcAlpha OneMinusSrcAlpha " +
                "    ZWrite Off Cull Off Fog { Mode Off } " +
                "    BindChannels {" +
                "      Bind \"vertex\", vertex Bind \"color\", color }" +
                "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }


        GameObject spwn = (GameObject)Instantiate(spawner, Vector3.zero, Quaternion.identity);
        spwn.name = spawner.name;
        MonoBehaviour[] behaviours = spwn.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour m in behaviours)
            m.enabled = false;

        objectsSpawned.Add(spwn);
    }

    private void OnPostRender()
    {
        if (!isGrid)
            return;

        lineMaterial.SetPass(0);

        GL.Color(new Color(0.3f, 0.3f, 0.3f, 0.7f));

        float offset = 3;

        for (int x = -Mathf.RoundToInt(camera.orthographicSize * offset); x <= Mathf.RoundToInt(camera.orthographicSize * offset); x++)
        {
            GL.Begin(GL.LINES);

            GL.Vertex3(x + Mathf.RoundToInt(transform.position.x) + 0.5f, -Mathf.RoundToInt(camera.orthographicSize * offset) + Mathf.RoundToInt(transform.position.y), 1);
            GL.Vertex3(x + Mathf.RoundToInt(transform.position.x) + 0.5f, Mathf.RoundToInt(camera.orthographicSize * offset) + Mathf.RoundToInt(transform.position.y), 1);

            GL.End();
        }

        for (int c = -Mathf.RoundToInt(camera.orthographicSize * offset); c <= Mathf.RoundToInt(camera.orthographicSize * offset); c++)
        {
            GL.Begin(GL.LINES);

            GL.Vertex3(-Mathf.RoundToInt(camera.orthographicSize * offset) + Mathf.RoundToInt(transform.position.x), c + Mathf.RoundToInt(transform.position.y) + 0.5f, 1);
            GL.Vertex3(Mathf.RoundToInt(camera.orthographicSize * offset) + Mathf.RoundToInt(transform.position.x), c + Mathf.RoundToInt(transform.position.y) + 0.5f, 1);

            GL.End();
        }
    }

    private void Update()
    {
        if (Time.timeScale != 0)
            return;

        if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Mouse1))
        {
            Vector2 movment = new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            if (!Input.GetKey(KeyCode.LeftControl))
                transform.Translate(movment * camera.orthographicSize * 0.05f);
            else
            {
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + movment.x + movment.y, 1, 50);
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && selected != null)
        {
            Vector3 movment = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movment.z = selected.transform.position.z;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                movment.x = Mathf.RoundToInt(movment.x);
                movment.y = Mathf.RoundToInt(movment.y);

                offset.x = Mathf.RoundToInt(offset.x);
                offset.y = Mathf.RoundToInt(offset.y);
            }
            selected.transform.position = movment - offset;
            propertiesPositionX = selected.transform.position.x.ToString();
            propertiesPositionY = selected.transform.position.y.ToString();
            propertiesPositionZ = selected.transform.position.z.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                isSaved = false;
                showSettings = false;

                selected = hit.collider.gameObject;
                while (selected.transform.parent != null)
                    selected = selected.transform.parent.gameObject;

                if (selected.rigidbody)
                    selected.rigidbody.isKinematic = true;

                lastSelected = selected;
                offset = new Vector3(hit.point.x, hit.point.y, selected.transform.position.z) - selected.transform.position;
                propertiesRotation = selected.transform.eulerAngles.z.ToString();

                scaleX = selected.transform.localScale.x.ToString();
                scaleY = selected.transform.localScale.y.ToString();

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
            if (selected != null && selected.tag != "Spawner")
            {
                objectsSpawned.Remove(selected);
                Destroy(selected);
            }
            else if (lastSelected != null && lastSelected.tag != "Spawner")
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
                camera.orthographicSize = Mathf.Max(new float[2] { lastSelected.transform.localScale.x, lastSelected.transform.localScale.y });
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            gameObject.camera.orthographicSize = Mathf.Clamp(gameObject.camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel"), 1, 50);
    }

    private void OnGUI()
    {
        if (skin != null)
            GUI.skin = skin;

        if (showObjectMenu)
        {
            //=======TopRight=======
            if (isSaved)
            {
                if (GUI.Button(new Rect(Screen.width - 105, 5, 100, 20), "Publish"))
                {

                }
            }
            else
            {
                if (GUI.Button(new Rect(Screen.width - 105, 5, 100, 20), "Save"))
                {

                    isSaved = true;
                }
            }

            if (GUI.Button(new Rect(Screen.width - 210, 5, 100, 20), "Settings"))
            {
                showSettings = !showSettings;
            }

            //=======TopLeft=======

            GUI.Window(0, new Rect(5, 30, 200, Screen.height - 35), objectWindow, "Object");
            GUI.Window(1, new Rect(Screen.width - 205, 30, 200, Screen.height - 35), propertiesWindow, "Properties");
            if (GUI.Button(new Rect(60, 5, 80, 20), "Grid: " + isGrid.ToString()))
                isGrid = !isGrid;

            isBackground = GUI.Toggle(new Rect(145, 5, 100, 20), isBackground, "Background");
        }

        if (Time.timeScale == 0)
        {
            if (!isBackground)
                RenderSettings.skybox = null;
            else
                RenderSettings.skybox = backgrounds[selectedBackground];

            if (GUI.Button(new Rect(5, 5, 50, 20), "Play"))
            {
                showObjectMenu = false;
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
            RenderSettings.skybox = backgrounds[selectedBackground];
            if (GUI.Button(new Rect(5, 5, 50, 20), "Stop"))
            {
                showObjectMenu = true;
                GetComponent<Camera>().enabled = true;
                GetComponent<AudioListener>().enabled = true;

                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
                foreach (Object o in objects)
                {
                    if ((GameObject)o != gameObject && (GameObject)o != Light)
                        Destroy((GameObject)o);
                }
                Time.timeScale = 0;
                loadLevel(editorData);
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

                    if (spwn.GetComponent<blockMaterial>())
                        spwn.GetComponent<blockMaterial>().UpdateTexture();

                    objectsSpawned.Add(spwn);
                }
                isSaved = false;
            }
        }
        GUILayout.EndScrollView();
    }

    private void propertiesWindow(int id)
    {
        if (showSettings)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Car Type:", GUILayout.Width(75));
            GUILayout.BeginVertical();
            if (chosingBackground)
            {
                for (int x = 0; x < backgrounds.Length; x++)
                {
                    if (GUILayout.Button(backgrounds[x].name))
                    {
                        selectedBackground = x;
                        Settings.carType = (Settings.CarType)selectedBackground;
                        foreach (GameObject o in objectsSpawned)
                        {
                            if (o.GetComponent<blockMaterial>())
                                o.GetComponent<blockMaterial>().UpdateTexture();
                        }
                        chosingBackground = false;
                    }
                }
            }
            else
            {
                if (GUILayout.Button(backgrounds[selectedBackground].name))
                {
                    chosingBackground = true;
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        else if (selected != null)
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

        propertiesPositionX = GUILayout.TextField(propertiesPositionX, GUILayout.Width(90));
        propertiesPositionY = GUILayout.TextField(propertiesPositionY, GUILayout.Width(90));

        GUILayout.EndHorizontal();

        GUILayout.Label("Z-Axies");

        try
        {
            propertiesPositionZ = GUILayout.HorizontalScrollbar(float.Parse(propertiesPositionZ), 0.1f, -0.3f, 0.3f).ToString();
        }
        catch(System.Exception){        }

        GUILayout.BeginHorizontal();

        GUILayout.Label("Rotation: ", GUILayout.Width(52));
        propertiesRotation = GUILayout.TextField(propertiesRotation);

        GUILayout.EndHorizontal();

        GUILayout.Label("Scale: ");
        GUILayout.BeginHorizontal();

        scaleX = GUILayout.TextField(scaleX, GUILayout.Width(90));
        scaleY = GUILayout.TextField(scaleY, GUILayout.Width(90));

        GUILayout.EndHorizontal();

        GUILayout.Space(50);

        if (GUILayout.Button("Apply", GUILayout.Width(100)))
        {
            saveProperties(obj);
        }

        if (Event.current.type == EventType.keyDown && Event.current.character == '\n')
        {
            saveProperties(obj);
        }
    }

    private void saveProperties(GameObject obj)
    {
        isSaved = false;

        obj.transform.localScale = new Vector3(float.Parse(scaleX), float.Parse(scaleY), obj.transform.localScale.z);
        obj.transform.position = stringToVector3("( " + propertiesPositionX + ", " + propertiesPositionY + ", " + propertiesPositionZ + " )");
        obj.transform.eulerAngles = new Vector3(0, 0, float.Parse(propertiesRotation));

        if (obj.GetComponent<blockMaterial>())
            obj.GetComponent<blockMaterial>().UpdateTexture();
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
            levelData += "  scale=" + objectsSpawned[x].transform.localScale + ";\n";
            levelData += " }\n";
        }

        levelData += "Settings{ \n";
        levelData += "  Background=" + selectedBackground + ";\n";
        levelData += " }\n";

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
            if (o.Groups[1].ToString() == "Settings")
            {
                MatchCollection attributes = Regex.Matches(o.ToString(), @"([a-zA-Z]+)\s*=\s*([a-zA-Z0-9\(\.\,\ \)\-]+);");
                foreach (Match a in attributes)
                {
                    string name = a.Groups[1].ToString();
                    string value = a.Groups[2].ToString();

                    switch (name)
                    {
                        case "Background":
                            selectedBackground = int.Parse(value);
                            if (isBackground)
                                RenderSettings.skybox = backgrounds[selectedBackground];
                            break;
                    }
                }
                continue;
            }
            else if (o.Groups[1].ToString() == "Spawner (Static)")
            {
                spwn = (GameObject)Instantiate(spawner, Vector3.zero, Quaternion.identity);
                spwn.name = spawner.name;
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
                        case "scale":
                            spwn.transform.localScale = stringToVector3(value);
                            break;
                    }
                }
                continue;
            }

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
                            case "scale":
                                spwn.transform.localScale = stringToVector3(value);
                                break;
                        }
                    }
                    if (spwn.GetComponent<blockMaterial>())
                        spwn.GetComponent<blockMaterial>().UpdateTexture();
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

class DropDown
{
    public bool isClicked = false;
    public List<string> items = new List<string>();
    public int selectedItem = 0;

    public DropDown(List<string> items, int defaultItem)
    {
        this.items = items;
        this.selectedItem = defaultItem;
    }

    public int Draw()
    {
        if (isClicked)
        {
            for (int x = 0; x < items.Count; x++)
            {
                if (GUILayout.Button(items[x]))
                {
                    selectedItem = x;
                    isClicked = false;
                }
            }
        }
        else
        {
            if (GUILayout.Button(items[selectedItem]))
            {
                isClicked = true;
            }
        }
        return selectedItem;
    }

    public int Draw(float width)
    {
        if (isClicked)
        {
            for (int x = 0; x < items.Count; x++)
            {
                if (GUILayout.Button(items[x], GUILayout.Width(width)))
                {
                    selectedItem = x;
                    isClicked = false;
                }
            }
        }
        else
        {
            if (GUILayout.Button(items[selectedItem], GUILayout.Width(width)))
            {
                isClicked = true;
            }
        }
        return selectedItem;
    }

    public int Draw(float width, float height)
    {
        if (isClicked)
        {
            for (int x = 0; x < items.Count; x++)
            {
                if (GUILayout.Button(items[x], GUILayout.Width(width), GUILayout.Height(height)))
                {
                    selectedItem = x;
                    isClicked = false;
                }
            }
        }
        else
        {
            if (GUILayout.Button(items[selectedItem], GUILayout.Width(width), GUILayout.Height(height)))
            {
                isClicked = true;
            }
        }
        return selectedItem;
    }
}