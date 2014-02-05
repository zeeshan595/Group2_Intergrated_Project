using UnityEngine;
using System.Collections;

public class PlayerNetwork : MonoBehaviour
{
    public GameObject playerCamera;

    private void Start()
    {
        if (!networkView.isMine && Network.peerType != NetworkPeerType.Disconnected)
        {
            gameObject.GetComponent<Car>().enabled = false;
            Destroy(playerCamera);
        }
    }
}