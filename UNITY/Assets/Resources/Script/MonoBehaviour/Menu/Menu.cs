using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject[] menus;

    private void Start()
    {
        Time.timeScale = 1;
        if (Settings.Username != "")
        {
            changeMenu(1);
        }
        else
        {
            changeMenu(0);
        }
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
            case 2://Credits
                menus[0].SetActive(false);
                menus[1].SetActive(false);
                menus[2].SetActive(false);
                menus[3].SetActive(false);
                menus[4].SetActive(true);
                break;
            case 3://Create
                menus[1].SetActive(false);
                GetComponent<Create>().enabled = true;
                break;
            case 4://Comunity
                
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
                changeMenu(1);
                break;
            case 8://go to options
                changeMenu(5);
                break;
            case 9://Register Button
#if UNITY_WEBPLAYER
                Application.ExternalEval("window.open('http://impossiblesix.net/register','_blank')");
#else
                System.Diagnostics.Process.Start("http://impossiblesix.net/register");
#endif
                break;
        }
    }

    private void changeMenu(int id)
    {
        for (int x = 0; x < menus.Length; x++)
        {
            if (x == id)
                menus[x].SetActive(true);
            else
                menus[x].SetActive(false);
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