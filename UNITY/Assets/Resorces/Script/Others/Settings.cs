using UnityEngine;

public class Settings
{
    public static Button[] buttons = new Button[5]{
        new Button("Accelerate", KeyCode.UpArrow),
        new Button("Brake/Reverse", KeyCode.DownArrow),
        new Button("Turn Right", KeyCode.RightArrow),
        new Button("Turn Left", KeyCode.LeftArrow),
        new Button("Reset Car", KeyCode.R),
    };

    public static int GetAxies(KeyCode pos, KeyCode neg)
    {
        if (Input.GetKey(pos) && Input.GetKey(neg))
            return 0;
        else if (Input.GetKey(pos))
            return 1;
        else if (Input.GetKey(neg))
            return -1;
        else
            return 0;
    }
}

public class Button
{
    public string name;
    public KeyCode key;

    public Button(string name, KeyCode key)
    {
        this.name = name;
        this.key = key;
    }
}