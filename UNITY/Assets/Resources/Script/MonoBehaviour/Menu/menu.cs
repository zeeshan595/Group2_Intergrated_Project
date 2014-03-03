using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour
{
    public GameObject action;
    public GameObject romance;
    public GameObject cyberpunk;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(5, 5, 200, 200), "Action Car"))
        {
            Instantiate(action, new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gameObject);
        }

        if (GUI.Button(new Rect(210, 5, 200, 200), "Romance Car"))
        {
            Instantiate(romance, new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gameObject);
        }

        if (GUI.Button(new Rect(5, 210, 200, 200), "Cyberpunk Car"))
        {
            Instantiate(cyberpunk, new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gameObject);
        }
        for (int x = 0; x < Settings.buttons.Length; x++)
        {
            GUI.Label(new Rect(5, 420 + (x * 15), 200, 200), Settings.buttons[x].name + ": " + Settings.buttons[x].key);
        }
    }
}