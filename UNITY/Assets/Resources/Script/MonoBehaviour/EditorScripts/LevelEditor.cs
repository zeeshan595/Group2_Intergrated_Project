using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LevelEditor : MonoBehaviour
{
    #region Variables

    public static string editorData = "";
    public static int levelID = -1;
    public static string levelName = "";
    public static string levelDescription = "";

    public GUISkin skin;
    public GameObject Light;
    public SpawnObject[] objects;
    public Material[] backgrounds;
    public GameObject spawner;

    private bool saving = false;
    private bool publishing = false;

    private int selectedBackground = 4;
    private bool isBackground = false;
    private bool isSaved = false;
    private bool chosingBackground = false;
    private bool showSettings = false;
    private bool showObjectMenu = true;
    private bool autoSnap = false;
    private List<GameObject> objectsSpawned = new List<GameObject>();
    private GameObject selected = null;
    private GameObject lastSelected = null;
    private bool isGrid = true;
    private Color[] selectedColor;
    private Vector2 objectScrollView = Vector2.zero;
    private Vector2 propertiesScrollView = Vector2.zero;
    private Vector2 historyScrollView = Vector2.zero;
    private Vector3 offset = Vector3.zero;

    private string propertiesPositionX = "";
    private string propertiesPositionY = "";
    private string propertiesPositionZ = "";
    private string propertiesRotation = "";

    private bool popupBlockChange = false;

    private string scale = "";
    private float scaleRatio = 1;

    private Material lineMaterial;

    private DropDown blockMaterial;
    private DropDown carType;

    public List<History> undo = new List<History>();
    private Vector3 selectedNormalPos = Vector3.zero;
    [System.NonSerialized]
    public int currentState = -1;

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

        List<string> drop = new List<string>();
        drop.Add("Action");
        drop.Add("Cyberpunk");
        drop.Add("Romance");
        drop.Add("Science Fiction");
        drop.Add("Fantasy");
        drop.Add("Adventure");

        blockMaterial = new DropDown(drop, 0);
        drop.Add("Log");
        carType = new DropDown(drop, 0);

        if (levelID != -1 && editorData == "")
        {
            isSaved = true;
            WWWForm form = new WWWForm();
            form.AddField("q", "SELECT * FROM `levels` WHERE `id` = '" + levelID + "' LIMIT 1");

            WWW w = new WWW("http://impossiblesix.net/InGame/returnQuery.php", form);

            StartCoroutine(loadLevel(w));
        }
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
        audio.volume = Mathf.Clamp(audio.volume + 0.001f, 0, Settings.MusicVolume);

        if (Time.timeScale != 0 || saving || publishing || ((Input.mousePosition.x < 210 || Input.mousePosition.x > Screen.width - 320) && selected == null))
            return;

        #region Camera movments

        if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Mouse1))
        {
            Vector2 movment = new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            if (!Input.GetKey(KeyCode.LeftControl))
                transform.Translate(movment * camera.orthographicSize * 0.05f);
            else
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + movment.x + movment.y, 1, 50);
        }

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * camera.orthographicSize * 0.03f);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.up * camera.orthographicSize * 0.03f);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(-Vector3.right * camera.orthographicSize * 0.03f);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * camera.orthographicSize * 0.03f);

        #endregion

        #region select and move

        if (Input.GetKey(KeyCode.Mouse0) && selected != null)
        {
            Vector3 movment = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movment.z = selected.transform.position.z;

            if (autoSnap && !Input.GetKey(KeyCode.LeftControl))
            {
                movment.x = Mathf.RoundToInt(movment.x);
                movment.y = Mathf.RoundToInt(movment.y);

                offset.x = Mathf.RoundToInt(offset.x);
                offset.y = Mathf.RoundToInt(offset.y);
            }
            else if (!autoSnap && Input.GetKey(KeyCode.LeftControl))
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.touches.Length < 2)
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

                scale = selected.transform.localScale.x.ToString();
                if (!selected.GetComponent<editorObject>().yZSizeSwap)
                    scaleRatio = selected.transform.localScale.y / selected.transform.localScale.x;
                else
                    scaleRatio = selected.transform.localScale.z / selected.transform.localScale.x;

                if (selected.GetComponent<blockMaterial>())
                {
                    blockMaterial.selectedItem = selected.GetComponent<blockMaterial>().selectedMaterial;
                }
                else if (selected.GetComponent<spawner>())
                {
                    carType.selectedItem = (int)Settings.carType;
                }

                MeshRenderer[] renderers = selected.GetComponentsInChildren<MeshRenderer>();
                selectedColor = new Color[renderers.Length];
                for (int x = 0; x < renderers.Length; x++)
                {
                    selectedColor[x] = renderers[x].material.color;
                    renderers[x].material.color = new Color(1, 1, 1, 0.3f);
                }

                selectedNormalPos = selected.transform.position;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && selected != null)
        {
            currentState++;
            if (currentState != undo.Count)
            {
                List<History> temp = new List<History>();
                foreach (History s in undo)
                    temp.Add(s);

                undo.Clear();
                for (int x = 0; x < currentState; x++)
                    undo.Add(temp[x]);
            }
            string levelData = "";
            for (int x = 0; x < objectsSpawned.Count; x++)
            {
                levelData += objectsSpawned[x].name + "{ \n";
                if (selected == objectsSpawned[x])
                    levelData += "  position=" + selectedNormalPos + ";\n";
                else
                    levelData += "  position=" + objectsSpawned[x].transform.position + ";\n";
                levelData += "  rotation=" + objectsSpawned[x].transform.eulerAngles + ";\n";
                levelData += "  scale=" + objectsSpawned[x].transform.localScale + ";\n";

                if (objectsSpawned[x].GetComponent<spawner>())
                {
                    levelData += "  car=" + (int)Settings.carType + ";\n";
                }
                else if (objectsSpawned[x].GetComponent<blockMaterial>())
                {
                    levelData += "  blockType=" + objectsSpawned[x].GetComponent<blockMaterial>().selectedMaterial + ";\n";
                }

                levelData += " }\n";
            }

            levelData += "Settings{ \n";
            levelData += "  Background=" + selectedBackground + ";\n";
            levelData += " }\n";

            undo.Add(new History(selected.name + " Position", levelData));

            if (selected.rigidbody)
                selected.rigidbody.isKinematic = false;

            MeshRenderer[] renderers = selected.GetComponentsInChildren<MeshRenderer>();
            for (int x = 0; x < renderers.Length; x++)
            {
                renderers[x].material.color = selectedColor[x];
            }

            selected = null;
        }

        #endregion

        #region extra functionality

        //DELETE

        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            updateUndo(lastSelected.name + " Deleted");
            if (lastSelected != null && lastSelected.tag != "Spawner")
            {
                objectsSpawned.Remove(lastSelected);
                Destroy(lastSelected);
            }
        }

        //FOCUS

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (lastSelected != null)
            {
                transform.position = new Vector3(lastSelected.transform.position.x, lastSelected.transform.position.y, transform.position.z);
                camera.orthographicSize = Mathf.Max(new float[2] { lastSelected.transform.localScale.x, lastSelected.transform.localScale.y });
            }
        }

        //ZOOM

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            gameObject.camera.orthographicSize = Mathf.Clamp(gameObject.camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel"), 1, 50);

        //TOUCH ZOOM

        if (Input.touches.Length == 2)
        {
            Touch t0 = Input.touches[0];
            Touch t1 = Input.touches[1];

            if (t0.position.x - t1.position.x > 0)
                gameObject.camera.orthographicSize = Mathf.Clamp((((t0.deltaPosition.x - t1.deltaPosition.x)) / -5) + gameObject.camera.orthographicSize, 1, 50);
            else
                gameObject.camera.orthographicSize = Mathf.Clamp((((t1.deltaPosition.x - t0.deltaPosition.x)) / -5) + gameObject.camera.orthographicSize, 1, 50);

            if (selected != null)
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
        }

        //Duplicate

        if (lastSelected != null && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
        {
            for (int x = 0; x < objects.Length; x++)
            {
                if (lastSelected.name == objects[x].name)
                {
                    updateUndo("Duplicated " + lastSelected.name);
                    GameObject spwn = (GameObject)Instantiate(objects[x].obj, new Vector3(lastSelected.transform.position.x, lastSelected.transform.position.y, objects[x].obj.transform.position.z), objects[x].obj.transform.rotation);
                    if (Time.timeScale == 0)
                    {
                        spwn.name = objects[x].obj.name;
                        MonoBehaviour[] behaviours = spwn.GetComponents<MonoBehaviour>();
                        foreach (MonoBehaviour m in behaviours)
                            m.enabled = false;

                        behaviours = spwn.GetComponentsInChildren<MonoBehaviour>();
                        foreach (MonoBehaviour m in behaviours)
                            m.enabled = false;

                        if (spwn.GetComponent<blockMaterial>())
                            spwn.GetComponent<blockMaterial>().UpdateTexture();

                        objectsSpawned.Add(spwn);
                    }
                    isSaved = false;
                }
            }
        }

        //UNDO + REDO

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z)) //Redo
        {
            currentState++;
            string curr = SaveLevel();
            loadLevel(undo[currentState].data);
            undo[currentState].data = curr;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z) && currentState != -1) //Undo
        {
            string curr = SaveLevel();
            loadLevel(undo[currentState].data);
            undo[currentState].data = curr;
            currentState--;
        }

        #endregion
    }

    private void OnGUI()
    {
        if (skin != null)
            GUI.skin = skin;

        if (popupBlockChange)
        {
            GUI.Window(4, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 75), popupWindow, "Change Blocks");
            return;
        }

        if (saving || publishing)
        {
            if (saving)
                GUI.Window(2, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400), savingWindow, "Save");
            else
                GUI.Window(3, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400), savingWindow, "Publish");

            return;
        }

        if (showObjectMenu)
        {
            //=======TopRight=======
            if (isSaved)
            {
                if (GUI.Button(new Rect(Screen.width - 105, 5, 100, 20), "Publish"))
                {
                    saving = false;
                    publishing = true;
                }
            }
            else
            {
                if (GUI.Button(new Rect(Screen.width - 105, 5, 100, 20), "Save"))
                {
                    saving = true;
                    publishing = false;
                }
            }

            if (GUI.Button(new Rect(Screen.width - 210, 5, 100, 20), "Settings"))
                showSettings = !showSettings;

            if (GUI.Button(new Rect(Screen.width - 315, 5, 100, 20), "Back"))
                Application.LoadLevel("menu");

            //=======TopLeft=======

            GUI.Window(0, new Rect(5, 30, 200, Screen.height - 35), objectWindow, "Object");
            GUI.Window(1, new Rect(Screen.width - 315, 30, 310, (Screen.height - 35) / 2 - 10), propertiesWindow, "Properties");
            GUI.Window(2, new Rect(Screen.width - 315, (Screen.height + 35) / 2, 310, (Screen.height - 35) / 2 - 10), historyWindow, "History");

            isGrid = GUI.Toggle(new Rect(60, 5, 50, 20), isGrid, "Grid");
            isBackground = GUI.Toggle(new Rect(105, 5, 100, 20), isBackground, "Background");
            autoSnap = GUI.Toggle(new Rect(195, 5, 100, 20), autoSnap, "Auto Snap");
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

                    if (g.name != "Finish (Static)")
                    {
                        behaviours = g.GetComponentsInChildren<MonoBehaviour>();
                        foreach (MonoBehaviour m in behaviours)
                            m.enabled = true;
                    }

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

    private void savingWindow(int id)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Title: ", GUILayout.Width(50));
        levelName = GUILayout.TextField(levelName);

        GUILayout.EndHorizontal();

        GUILayout.Label("Description: ", GUILayout.Width(200));
        levelDescription = GUILayout.TextArea(levelDescription, 1000, GUILayout.Height(290));


        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Cancel"))
        {
            saving = false;
            publishing = false;
        }

        if (id == 2)
        {
            if (GUILayout.Button("Save"))
            {
                if (levelName != "" && levelDescription != "")
                {
                    editorData = SaveLevel();
                    WWWForm form = new WWWForm();
                    form.AddField("Name", levelName);
                    form.AddField("Description", levelDescription);
                    form.AddField("Author", Settings.Username);
                    form.AddField("data", editorData);
                    form.AddField("id", levelID);
                    form.AddField("publish", '0');

                    WWW w = new WWW("http://impossiblesix.net/InGame/saveLevel", form);

                    StartCoroutine(saveLevel(w));
                }
            }
        }
        else
        {
            if (GUILayout.Button("Publish"))
            {
                if (levelName != "" && levelDescription != "")
                {
                    editorData = SaveLevel();
                    WWWForm form = new WWWForm();
                    form.AddField("Name", levelName);
                    form.AddField("Description", levelDescription);
                    form.AddField("Author", Settings.Username);
                    form.AddField("data", editorData);
                    form.AddField("id", levelID);
                    form.AddField("publish", '1');

                    WWW w = new WWW("http://impossiblesix.net/InGame/saveLevel", form);

                    StartCoroutine(saveLevel(w));
                }
            }
        }

        GUILayout.EndHorizontal();
    }

    private void objectWindow(int id)
    {
        objectScrollView = GUILayout.BeginScrollView(objectScrollView);
        for (int z = 0; z < System.Enum.GetNames(typeof(SpawnObject.ObjectTag)).Length; z++)
        {
            GUILayout.Label(((SpawnObject.ObjectTag)z).ToString());
            for (int x = 0; x < objects.Length; x++)
            {
                if (objects[x].tag == (SpawnObject.ObjectTag)z)
                {
                    if (GUILayout.Button(objects[x].name))
                    {
                        updateUndo(objects[x].name + " Created");
                        GameObject spwn = (GameObject)Instantiate(objects[x].obj, new Vector3(transform.position.x, transform.position.y, objects[x].obj.transform.position.z), objects[x].obj.transform.rotation);
                        if (Time.timeScale == 0)
                        {
                            spwn.name = objects[x].obj.name;
                            MonoBehaviour[] behaviours = spwn.GetComponents<MonoBehaviour>();
                            foreach (MonoBehaviour m in behaviours)
                                m.enabled = false;

                            behaviours = spwn.GetComponentsInChildren<MonoBehaviour>();
                            foreach (MonoBehaviour m in behaviours)
                                m.enabled = false;

                            if (spwn.GetComponent<blockMaterial>())
                            {
                                spwn.GetComponent<blockMaterial>().selectedMaterial = selectedBackground;
                                spwn.GetComponent<blockMaterial>().UpdateTexture();
                            }
                            objectsSpawned.Add(spwn);
                        }
                        isSaved = false;
                    }
                }
            }
        }
        GUILayout.EndScrollView();
    }

    #region properties

    private void propertiesWindow(int id)
    {
        propertiesScrollView = GUILayout.BeginScrollView(propertiesScrollView);

        if (showSettings)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Background:", GUILayout.Width(75));
            GUILayout.BeginVertical();
            if (chosingBackground)
            {
                for (int x = 0; x < backgrounds.Length; x++)
                {
                    if (GUILayout.Button(backgrounds[x].name))
                    {
                        updateUndo("Background Changed");
                        selectedBackground = x;
                        chosingBackground = false;
                        popupBlockChange = true;
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
        else if (lastSelected != null)
            DisplayProperties(lastSelected);

        GUILayout.EndScrollView();
    }

    private void DisplayProperties(GameObject obj)
    {
        GUILayout.Label("Name: " + obj.name);

        if (obj.GetComponent<editorObject>().position)
        {
            GUILayout.Label("Position: ");
            GUILayout.BeginHorizontal();

            propertiesPositionX = GUILayout.TextField(propertiesPositionX, GUILayout.Width(80));
            propertiesPositionY = GUILayout.TextField(propertiesPositionY, GUILayout.Width(80));

            GUILayout.EndHorizontal();
        }
        
        if (obj.GetComponent<editorObject>().zAxies)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Z-Axies", GUILayout.Width(50));//propertiesPositionZ = GUILayout.HorizontalSlider(float.Parse(propertiesPositionZ), -0.3f, 0.3f).ToString();
            propertiesPositionZ = GUILayout.TextField(propertiesPositionZ);

            GUILayout.EndHorizontal();
        }
        
        if (obj.GetComponent<editorObject>().rotation)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Rotation: ", GUILayout.Width(52));
            propertiesRotation = GUILayout.TextField(propertiesRotation);

            GUILayout.EndHorizontal();
        }

        if (obj.GetComponent<editorObject>().size)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale: " , GUILayout.Width(50));

            scale = GUILayout.TextField(scale);

            GUILayout.EndHorizontal();
        }
        
        if (obj.GetComponent<blockMaterial>())
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Material: ", GUILayout.Width(75));

            GUILayout.BeginVertical();
            obj.GetComponent<blockMaterial>().selectedMaterial = blockMaterial.Draw(this);
            obj.GetComponent<blockMaterial>().UpdateTexture();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
        else if (obj.GetComponent<spawner>())
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Car Type: ", GUILayout.Width(75));

            GUILayout.BeginVertical();
            Settings.carType = (Settings.CarType)carType.Draw(this);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(30);

        if (GUILayout.Button("Apply", GUILayout.Width(100)))
            saveProperties(obj);

        if (Event.current.type == EventType.keyDown && Event.current.character == '\n')
            saveProperties(obj);
    }

    private void saveProperties(GameObject obj)
    {
        isSaved = false;
        if (obj.GetComponent<editorObject>().size)
        {
            if (obj.GetComponent<editorObject>().ratioScale)
            {
                if (obj.GetComponent<editorObject>().yZSizeSwap)
                    obj.transform.localScale = new Vector3(float.Parse(scale), obj.transform.localScale.y, float.Parse(scale) * scaleRatio);
                else
                    obj.transform.localScale = new Vector3(float.Parse(scale), float.Parse(scale) * scaleRatio, obj.transform.localScale.z);
            }
            else
            {
                obj.transform.localScale = new Vector3(float.Parse(scale), obj.transform.localScale.y, obj.transform.localScale.z);
            }
        }

        if (obj.GetComponent<editorObject>().position)
            obj.transform.position = stringToVector3("( " + propertiesPositionX + ", " + propertiesPositionY + ", " + propertiesPositionZ + " )");

        if (obj.GetComponent<editorObject>().rotation)
            obj.transform.eulerAngles = new Vector3(0, 180, float.Parse(propertiesRotation));

        if (obj.GetComponent<blockMaterial>())
            obj.GetComponent<blockMaterial>().UpdateTexture();


        updateUndo(obj.name + " properties");
    }

    #endregion

    private void historyWindow(int id)
    {
        historyScrollView = GUILayout.BeginScrollView(historyScrollView);

        for (int x = 0; x < undo.Count; x++)
        {
            if (GUILayout.Button(undo[x].title))
            {
                isSaved = false;
                if (undo[undo.Count - 1].title != "Redo")
                {
                    updateUndo("Redo");
                    currentState = x;
                    loadLevel(undo[x].data);
                }
                else
                {
                    currentState = x;
                    loadLevel(undo[x].data);
                }
            }
        }

        GUILayout.EndScrollView();
    }

    private void popupWindow(int id)
    {
        GUILayout.Label("Change All Platform Textures?");

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("No", GUILayout.Width(75)))
        {
            popupBlockChange = false;
        }
        if (GUILayout.Button("Yes", GUILayout.Width(75)))
        {
            foreach (GameObject o in objectsSpawned)
            {
                if (o.GetComponent<blockMaterial>())
                {
                    o.GetComponent<blockMaterial>().selectedMaterial = selectedBackground;
                    o.GetComponent<blockMaterial>().UpdateTexture();
                }
            }
            popupBlockChange = false;
        }

        GUILayout.EndHorizontal();
    }

    #endregion

    #region MySQL Responces

    public IEnumerator saveLevel(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            if (w.text == "**ERROR**")
                Debug.Log("Unknown error when saving the level.");
            else
            {
                try
                {
                    levelID = int.Parse(w.text);
                    isSaved = true;
                }
                catch(System.Exception)
                {
                    Debug.Log("Unexpected server responce.");
                }
            }
        }
        else
        {
            Debug.Log(w.error);
        }
        saving = false;
        publishing = false;
    }

    public IEnumerator loadLevel(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            MySQL loaded = new MySQL(w.text);
            editorData = loaded.data[loaded.Find("Level")].data;
            loadLevel(editorData);
            if (loaded.data[loaded.Find("Published")].data == "1")
                isSaved = true;

            levelName = loaded.data[loaded.Find("Name")].data;
            levelDescription = loaded.data[loaded.Find("Description")].data;
        }
        else
        {
            Debug.Log(w.error);
        }
    }

    #endregion

    #region save & load

    public string SaveLevel()
    {
        string levelData = "";
        for (int x = 0; x < objectsSpawned.Count; x++)
        {
            levelData += objectsSpawned[x].name + "{ \n";
            levelData += "  position=" + objectsSpawned[x].transform.position + ";\n";
            levelData += "  rotation=" + objectsSpawned[x].transform.eulerAngles + ";\n";
            levelData += "  scale=" + objectsSpawned[x].transform.localScale + ";\n";

            if (objectsSpawned[x].GetComponent<spawner>())
            {
                levelData += "  car=" + (int)Settings.carType + ";\n";
            }
            else if (objectsSpawned[x].GetComponent<blockMaterial>())
            {
                levelData += "  blockType=" + objectsSpawned[x].GetComponent<blockMaterial>().selectedMaterial + ";\n";
            }

            levelData += " }\n";
        }

        levelData += "Settings{ \n";
        levelData += "  Background=" + selectedBackground + ";\n";
        levelData += " }\n";

        return levelData;
    }

    public void loadLevel(string levelData)
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
            else if (o.Groups[1].ToString() == "Spawner")
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
                        case "car":
                            Settings.carType = (Settings.CarType)int.Parse(value);
                            break;
                    }
                }
                continue;
            }

            for (int x = 0; x < objects.Length; x++)
            {
                if (o.Groups[1].ToString() == objects[x].obj.name)
                {
                    spwn = (GameObject)Instantiate(objects[x].obj, new Vector3(transform.position.x, transform.position.y, objects[x].obj.transform.position.z), objects[x].obj.transform.rotation);
                    spwn.name = objects[x].obj.name;
                    MonoBehaviour[] behaviours = spwn.GetComponents<MonoBehaviour>();
                    foreach (MonoBehaviour m in behaviours)
                        m.enabled = false;

                    behaviours = spwn.GetComponentsInChildren<MonoBehaviour>();
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
                            case "blockType":
                                if (spwn.GetComponent<blockMaterial>())
                                    spwn.GetComponent<blockMaterial>().selectedMaterial = int.Parse(value);
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

    public void updateUndo(string title)
    {
        isSaved = false;
        currentState++;
        if (currentState != undo.Count)
        {
            List<History> temp = new List<History>();
            foreach (History s in undo)
                temp.Add(s);

            undo.Clear();
            for (int x = 0; x < currentState; x++)
                undo.Add(temp[x]);
        }
        undo.Add(new History(title, SaveLevel()));
    }

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

#region Helper Classes

public class History
{
    public string title;
    public string data;

    public History(string title, string data)
    {
        this.title = title;
        this.data = data;
    }
}

[System.Serializable]
public class SpawnObject
{
    public enum ObjectTag
    {
        Uses = 0,
        Items = 1,
        Obstacles = 2,
        Detail = 3
    }

    public string name;
    public ObjectTag tag;
    public GameObject obj;

    public SpawnObject(string name, ObjectTag tag, GameObject obj)
    {
        this.name = name;
        this.tag = tag;
        this.obj = obj;
    }
}

#endregion