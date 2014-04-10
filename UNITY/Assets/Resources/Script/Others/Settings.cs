using UnityEngine;

public class Settings
{
    public static string Username = "";

    public enum CarType
    {
        Action = 0,
        Cyberpunk = 1,
        Romance = 2,
        ScienceFiction = 3,
        Fantasy = 4,
        Adventure = 5,
        Log = 6
    }

    public static Button[] buttons = new Button[]{
        new Button("Accelerate", KeyCode.D),
        new Button("Brake/Reverse", KeyCode.A),
        new Button("Rotate Clockwise", KeyCode.RightArrow),
        new Button("Rotate Anti-clockwise", KeyCode.LeftArrow),
        new Button("Reset Car", KeyCode.R),
        new Button("Jump", KeyCode.Space)
    };

    public static CarType carType = CarType.Log; // Action car (default)

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