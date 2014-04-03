using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour
{
    public string nextLevel = "";
    private string timeFinished = "";
    private bool isFinished = false;

    private void Start()
    {
        GetComponent<BoxCollider>().size = new Vector3(0.05f, 1, 1);
        if (Application.loadedLevelName == "levelEditor")
            this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFinished = true;
            timeFinished = TimerToString(other.collider.transform.parent.GetComponent<Car>().timer);
            Camera.main.gameObject.GetComponent<CameraFollow>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnGUI()
    {
        if (isFinished)
        {
            GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 100, 50), timeFinished);
            if (nextLevel != "")
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Continue"))
                {
                    Application.LoadLevel(nextLevel);
                }
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 100, 50), "Back"))
            {
                Application.LoadLevel("menu");
            }
        }
    }

    #region helpers

    private string TimerToString(float seconds)
    {
        int min = 0;
        while (seconds > 60)
        {
            seconds -= 60;
            min++;
        }

        return min + ":" + Round(seconds, 2);
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    #endregion 
}