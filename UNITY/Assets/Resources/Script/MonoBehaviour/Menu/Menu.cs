using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GUISkin skin;
    public GameObject[] menus;

    private void Start()
    {
        Time.timeScale = 1;
        if (Settings.Username != "")
        {
            menus[0].SetActive(false);
            menus[1].SetActive(true);
        }
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        /*
#if UNITY_WEBPLAYER
        if (GUI.Button(new Rect(125, 5, 49, 51), "FB", skin.customStyles[0]))
            Application.ExternalEval("window.open('https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl','_blank')");

        if (GUI.Button(new Rect(125, 60, 49, 51), "T", skin.customStyles[0]))
            Application.ExternalEval("window.open('https://twitter.com/impossible_6','_blank')");

        if (GUI.Button(new Rect(125, 120, 49, 51), "W", skin.customStyles[0]))
            Application.ExternalEval("window.open('http://impossiblesix.net','_blank')");


#else
        if (GUI.Button(new Rect(125, 5, 49, 51), "FB", skin.customStyles[0]))
            System.Diagnostics.Process.Start("https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl");

        if (GUI.Button(new Rect(125, 60, 49, 51), "T", skin.customStyles[0]))
            System.Diagnostics.Process.Start("https://twitter.com/impossible_6");

        if (GUI.Button(new Rect(125, 120, 49, 51), "W", skin.customStyles[0]))
            System.Diagnostics.Process.Start("http://impossiblesix.net");
#endif
         */
    }

    public void buttonActive(int id)
    {
        switch(id)
        {
            case -1:
                Application.LoadLevel("tutorial");
                break;
            case 0://Quit
                Application.Quit();
                break;
            case 1://Play
                menus[0].SetActive(false);
                menus[1].SetActive(false);
                menus[2].SetActive(true);
                menus[3].SetActive(false);
                break;
            case 2://Comunity

                break;
            case 3://Create
                //menus[1].SetActive(false);
                //GetComponent<Create>().enabled = true;
                break;
            case 4://Credits

                break;
            case 5://Login
                WWWForm form = new WWWForm();
                form.AddField("user", menus[0].GetComponent<menuObjects>().objects[1].obj.GetComponent<menuTextField>().text);
                form.AddField("pass", menus[0].GetComponent<menuObjects>().objects[2].obj.GetComponent<menuTextField>().text);

                WWW w = new WWW("http://impossiblesix.net/inGame/login", form);
                StartCoroutine(login(w));
                break;
            case 6://Character Chosen
                menus[2].SetActive(false);
                menus[3].SetActive(true);
                break;
            case 7://Back to main menu
                menus[0].SetActive(false);
                menus[1].SetActive(true);
                menus[2].SetActive(false);
                menus[3].SetActive(false);
                break;
        }
    }

    private IEnumerator login(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            if (w.text == "**LOGIN_SUCCESS**")
            {
                Settings.Username = menus[0].GetComponent<menuObjects>().objects[1].obj.GetComponent<menuTextField>().text;
                menus[0].SetActive(false);
                menus[1].SetActive(true);
            }
            else
            {
                menus[0].GetComponent<menuObjects>().objects[0].obj.GetComponent<TextMesh>().text = "Wrong user or pass";
            }
        }
        else
        {
            menus[0].GetComponent<menuObjects>().objects[0].obj.GetComponent<TextMesh>().text = "Couldn't connect to the server.";
        }
    }
}