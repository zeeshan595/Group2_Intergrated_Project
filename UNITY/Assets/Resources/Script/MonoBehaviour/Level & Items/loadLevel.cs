using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class loadLevel : MonoBehaviour
{
    public static string levelData = "";
    public GameObject[] objects;
    public Material[] backgrounds;
    public GameObject spawner;

    private int selectedBackground = 0;

    private List<GameObject> objectsSpawned = new List<GameObject>();

    private void Start()
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