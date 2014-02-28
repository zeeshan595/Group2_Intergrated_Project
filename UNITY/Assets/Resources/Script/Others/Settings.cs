using UnityEngine;

public class Settings
{
    //This class contains only static variables and methods for the other scripts to access.
    public static Button[] buttons = new Button[]{
        new Button("Accelerate", KeyCode.D),
        new Button("Brake/Reverse", KeyCode.A),
        new Button("Rotate Clockwise", KeyCode.W),
        new Button("Rotate Anti-clockwise", KeyCode.S),
        new Button("Reset Car", KeyCode.R),
        new Button("Jump", KeyCode.Space)
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