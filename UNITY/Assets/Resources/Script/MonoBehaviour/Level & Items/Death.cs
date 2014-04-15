using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{
    public bool destroyRender = true;

    private void Start()
    {
        if (gameObject.renderer && destroyRender)
        {
            Destroy(gameObject.renderer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.gameObject.GetComponent<Car>().resetCar();
        }
    }
}