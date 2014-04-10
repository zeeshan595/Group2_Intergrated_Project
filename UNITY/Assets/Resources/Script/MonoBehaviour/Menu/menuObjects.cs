using UnityEngine;
using System.Collections;

public class menuObjects : MonoBehaviour
{
    public menuObject[] objects;
}

[System.Serializable]
public class menuObject
{
    public enum objectType
    {
        label = 0,
        button = 1,
        textField = 2
    }

    public string name;
    public objectType type;
    public GameObject obj;
}