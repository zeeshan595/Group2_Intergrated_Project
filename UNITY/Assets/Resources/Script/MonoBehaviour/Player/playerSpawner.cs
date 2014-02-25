using UnityEngine;
using System.Collections;

public class playerSpawner : MonoBehaviour
{
    public GameObject player;

    public void Start()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
            Instantiate(player, transform.position, transform.rotation);
        else
            Network.Instantiate(player, transform.position, transform.rotation, 0);
    }
}