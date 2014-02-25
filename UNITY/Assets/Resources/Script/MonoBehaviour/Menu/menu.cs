using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour
{
    public GameObject action;
    public GameObject cyberpunk;

    private void OnGUI()
    {
        if (GUILayout.Button("Action Car"))
        {
            Instantiate(action, new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gameObject);
        }

        if (GUILayout.Button("Cyberpunk Car"))
        {
            Instantiate(cyberpunk, new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gameObject);
        }
        GUILayout.Label("________________\n\n");
        for (int x = 0; x < Settings.buttons.Length; x++)
        {
            GUILayout.Label(Settings.buttons[x].name + ": " + Settings.buttons[x].key);
        }
    }
}