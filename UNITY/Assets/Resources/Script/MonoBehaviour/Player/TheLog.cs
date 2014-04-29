using UnityEngine;
using System.Collections;

public class TheLog : MonoBehaviour
{
    [System.NonSerialized]
    public Vector3 resetPosition;

    private void Start()
    {
        resetPosition = transform.position;
    }

    public void resetCar()
    {
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        transform.position = resetPosition;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ResetObject");
        foreach (GameObject g in gameObjects)
        {
            g.SendMessage("resetCar", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Update()
    {
        #region Car Reset

        if ((Input.GetKeyDown(Settings.buttons[4].key) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
        {
            resetCar();
        }

        #endregion

        #region Get Input

        Vector2 input = Vector2.zero;

        input = new Vector2(Settings.GetAxies(Settings.buttons[2].key, Settings.buttons[3].key), Settings.GetAxies(Settings.buttons[0].key, Settings.buttons[1].key));
        if (input == Vector2.zero)
            input += new Vector2(Input.GetAxis("Horizontal") * 2, Input.GetAxis("Vertical"));

        #endregion

        rigidbody.AddForce(Vector3.right * input.y * 1000);

        #region Car Jump

        if ((Input.GetKeyDown(Settings.buttons[5].key) || Input.GetKeyDown(KeyCode.Joystick1Button0) || (Input.touchCount > 1 && (Input.touches[Input.touchCount - 1].phase == TouchPhase.Began || Input.touches[0].phase == TouchPhase.Began))))
        {
            rigidbody.AddForce(Vector3.up * 20000);
        }

        #endregion
    }
}