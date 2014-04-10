using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject menu;

    private Vector3 carVelocity = Vector3.zero;
    private Vector3 carAngularVelocity = Vector3.zero;

    public static bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            changeState();
        }
    }

    public void changeState()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            carVelocity = player.rigidbody.velocity;
            carAngularVelocity = player.rigidbody.angularVelocity;

            menu.SetActive(true);

            if (player.GetComponent<Car>())
                player.GetComponent<Car>().enabled = false;
            else if (player.GetComponent<TheLog>())
                player.GetComponent<TheLog>().enabled = false;

            player.rigidbody.isKinematic = true;
        }
        else
        {
            menu.SetActive(false);
            if (player.GetComponent<Car>())
                player.GetComponent<Car>().enabled = true;
            else if (player.GetComponent<TheLog>())
                player.GetComponent<TheLog>().enabled = true;
            player.rigidbody.isKinematic = false;

            player.rigidbody.velocity = carVelocity;
            player.rigidbody.angularVelocity = carAngularVelocity;
        }
    }
}