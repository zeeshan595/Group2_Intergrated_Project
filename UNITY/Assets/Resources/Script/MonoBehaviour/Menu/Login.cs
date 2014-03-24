using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour
{
    public int effectWidth = 100;
    public GUISkin skin;
    public Texture blackBlock;
    public Texture whiteBlock;

    private string username = "";
    private string password = "";
    private string message = "Please login.";
    private bool isLoading = false;
    private bool capsLock = false;

    private void Start()
    {
        if (Settings.Username != "")
        {
            GetComponent<Menu>().enabled = true;
            this.enabled = false;
        }
    }

    private void OnGUI()
    {
        GUI.skin = skin;

        GUI.Window(0, new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 180), loginWindow, "");

        if (capsLock)
            GUI.Box(new Rect(5, Screen.height - 55, 350, 24), "Password is case senstive, Caps lock is on.");

#if UNITY_WEBPLAYER
        if (GUI.Button(new Rect(125, 5, 49, 51), "FB", skin.customStyles[0]))
            Application.ExternalEval("window.open('https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl','_blank')");

        if (GUI.Button(new Rect(125, 60, 49, 51), "T", skin.customStyles[0]))
            Application.ExternalEval("window.open('https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl','_blank')");

        if (GUI.Button(new Rect(125, 120, 49, 51), "W", skin.customStyles[0]))
            Application.ExternalEval("window.open('http://impossiblesix.net','_blank')");

        
#else
        if (GUI.Button(new Rect(125, 5, 49, 51), "FB", skin.customStyles[0]))
            System.Diagnostics.Process.Start("https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl");

        if (GUI.Button(new Rect(125, 60, 49, 51), "T", skin.customStyles[0]))
            System.Diagnostics.Process.Start("http://impossiblesix.net");

        if (GUI.Button(new Rect(125, 120, 49, 51), "W", skin.customStyles[0]))
            System.Diagnostics.Process.Start("http://impossiblesix.net");
#endif
    }

    private void loginWindow(int id)
    {
        GUILayout.Label(message);
        if (!capsLock)
        {
            if (Event.current.keyCode == KeyCode.CapsLock && Event.current.type == EventType.keyDown)
                capsLock = true;
        }
        else
            if (Event.current.keyCode == KeyCode.CapsLock && Event.current.type == EventType.keyDown)
                capsLock = false;

        if (!isLoading)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                WWWForm form = new WWWForm();
                form.AddField("user", username);
                form.AddField("pass", password);

                isLoading = true;
                message = "Please wait...";

                WWW w = new WWW("http://impossiblesix.net/inGame/login", form);
                StartCoroutine(login(w));
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label("Username:", GUILayout.Width(75));
            username = GUILayout.TextField(username);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Password:", GUILayout.Width(75));
            password = GUILayout.PasswordField(password, '*');

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Login"))
            {
                WWWForm form = new WWWForm();
                form.AddField("user", username);
                form.AddField("pass", password);

                isLoading = true;
                message = "Please wait...";

                WWW w = new WWW("http://impossiblesix.net/inGame/login", form);
                StartCoroutine(login(w));
            }

            //if (GUILayout.Button("Create an account", GUIStyle.none))
            //    System.Diagnostics.Process.Start("http://impossiblesix.net/register");
        }
    }

    private IEnumerator login(WWW w)
    {
        yield return w;
        isLoading = false;
        if (w.error == null)
        {
            if (w.text == "**LOGIN_SUCCESS**")
            {
                Settings.Username = username;
                message = "";
                GetComponent<Menu>().enabled = true;
                this.enabled = false;
            }
            else
            {
                message = "Wrong user or pass";
            }
        }
        else
        {
            message = "Couldn't connect to the server.";
        }
    }
}
