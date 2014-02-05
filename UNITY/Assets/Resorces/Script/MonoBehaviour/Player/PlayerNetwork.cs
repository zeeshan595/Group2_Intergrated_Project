using UnityEngine;
using System.Collections;

public class PlayerNetwork : MonoBehaviour
{
    public GameObject playerCamera;

    private void Start()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            Destroy(gameObject.GetComponent<NetworkView>());
        }
        else if (!networkView.isMine)
        {
            gameObject.GetComponent<Car>().enabled = false;
            Destroy(playerCamera);
        }
    }
}