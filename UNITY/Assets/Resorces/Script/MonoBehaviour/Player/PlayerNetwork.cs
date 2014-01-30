using UnityEngine;
using System.Collections;

public class PlayerNetwork : MonoBehaviour
{
    public GameObject playerCamera;

    private void Start()
    {
        if (!networkView.isMine)
        {
            gameObject.GetComponent<Car>().enabled = false;
            Destroy(playerCamera);
        }
    }
}