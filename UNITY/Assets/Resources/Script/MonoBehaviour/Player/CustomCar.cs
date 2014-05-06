using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomCar : MonoBehaviour
{
    public bool editor = true;
    public Material[] tiresTexture;
    public float suspension = 5000;
    public float torque = 33;
    public float mass = 1500;

    private Vector2 scrollView = Vector2.zero;
    private DropDown tiresDropDown;

    private void Start()
    {
        List<string> tires = new List<string>();
        foreach (Material t in tiresTexture)
            tires.Add(t.name);

        tiresDropDown = new DropDown(tires, 0);
        tiresDropDown.changedValue += tiresDropDown_changedValue;
    }

    private void tiresDropDown_changedValue(int i)
    {
        foreach (Wheel w in GetComponent<Car>().wheels)
        {
            foreach(Transform t in w.gameObject.transform)
            {
                t.renderer.material = tiresTexture[i];
            }
        }
    }

    private void OnGUI()
    {
        if (editor)
        {
            GUI.Window(0, new Rect(5, 5, 200, Screen.height - 10), windowFunc, "Car Settings");
        }
    }

    private void windowFunc(int id)
    {
        scrollView = GUILayout.BeginScrollView(scrollView);

        GUILayout.Label("Physics");

        GUILayout.Label("Suspension");
        suspension = GUILayout.HorizontalScrollbar(suspension, 500, 2500, 10000);

        GUILayout.Label("Torque");
        torque = GUILayout.HorizontalScrollbar(torque, 1, 15, 60);

        GUILayout.Label("Mass");
        mass = GUILayout.HorizontalScrollbar(mass, 50, 700, 2000);

        GUILayout.Space(30);
        GUILayout.Label("Display");

        tiresDropDown.Draw();



        GUILayout.EndScrollView();

        if (GUILayout.Button("Apply"))
        {
            foreach (Wheel w in GetComponent<Car>().wheels)
            {
                JointSpring j = w.gameObject.GetComponent<WheelCollider>().suspensionSpring;
                j.spring = suspension;
                w.gameObject.GetComponent<WheelCollider>().suspensionSpring = j;
            }

            rigidbody.mass = mass;
            GetComponent<Car>().torque = torque;
            GetComponent<Car>().resetCar();
        }
    }
}