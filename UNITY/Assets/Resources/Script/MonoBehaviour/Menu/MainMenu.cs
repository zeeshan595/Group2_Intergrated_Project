using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    #region variables

    public GUISkin guiSkin;
    private string ip = "";

    #endregion

    private void OnGUI()
    {
        GUI.skin = guiSkin;
        
        if (GUILayout.Button("Create Server"))
        {
            Network.InitializeSecurity();
            Network.InitializeServer(32, 25565, !Network.HavePublicAddress());
        }
        GUILayout.Label("IP Address:");
        ip = GUILayout.TextField(ip);
        if (GUILayout.Button("Join"))
        {
            Network.Connect(ip, 25565);
        }
    }

    #region Network Methods

    private void OnServerInitialized()
    {
        Application.LoadLevel("game");
    }

    private void OnConnectedToServer()
    {
        Application.LoadLevelAdditive("game");
    }

    #endregion
}