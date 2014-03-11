using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour
{
    public GameObject action;
    public GameObject cyberpunk;
    public GameObject romance;
    public GameObject scienceFunction;
    public GameObject fantasy;
    public GameObject adventure;
    public GameObject girlyGirl;

    private void Start()
    {
        switch(Settings.carType)
        {
            case Settings.CarType.Adventure:
                Instantiate(adventure, transform.position, Quaternion.identity);
                break;
            case Settings.CarType.Cyberpunk:
                Instantiate(cyberpunk, transform.position, Quaternion.identity);
                break;
            case Settings.CarType.Fantasy:
                Instantiate(fantasy, transform.position, Quaternion.identity);
                break;
            case Settings.CarType.GirlyGirl:
                Instantiate(girlyGirl, transform.position, Quaternion.identity);
                break;
            case Settings.CarType.Romance:
                Instantiate(romance, transform.position, Quaternion.identity);
                break;
            case Settings.CarType.ScienceFunction:
                Instantiate(scienceFunction, transform.position, Quaternion.identity);
                break;
            default: // Action car (default)
                Instantiate(action, transform.position, Quaternion.identity);
                break;
        }
    }
}