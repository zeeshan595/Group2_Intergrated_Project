using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour
{
    private string timeFinished = "";
    private bool isFinished = false;

    private void Start()
    {
        GetComponent<BoxCollider>().size = new Vector3(0.05f, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFinished = true;
            timeFinished = other.collider.transform.parent.GetComponent<Car>().timerObj.GetComponent<TextMesh>().text;
            Camera.main.gameObject.GetComponent<CameraFollow>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnGUI()
    {
        if (isFinished)
        {
            GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 100, 50), timeFinished);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Back"))
            {
                Application.LoadLevel("menu");
            }
        }
    }
}