using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{
    #region Variables;

    public Wheel[] wheels;
    public Vector3 centerOfMass = Vector3.zero;
    public float Torque = 50;
    public float topSpeed = 7;
    public float steeringAngle = 30;
    public float slowSteeringAngle = 30;
    public bool turnning = false;

    #endregion

    private void Start()
    {
        rigidbody.centerOfMass = centerOfMass;

        for (int x = 0; x < wheels.Length; x++)
        {
            WheelCollider w;

            if (!wheels[x].gameObject.GetComponent<WheelCollider>())
                w = wheels[x].gameObject.AddComponent<WheelCollider>();
            else
                w = wheels[x].gameObject.GetComponent<WheelCollider>();


            w.mass = wheels[x].mass;
            w.radius = wheels[x].radius;
            w.suspensionDistance = wheels[x].suspentionDistance;

            JointSpring spring = new JointSpring();
            spring.spring = wheels[x].suspentionSpring;
            spring.damper = wheels[x].suspentionDamper;

            w.suspensionSpring = spring;
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(Settings.buttons[4].key))
        {
            transform.position = transform.position + new Vector3(0, 2, 0);
            transform.rotation = Quaternion.identity;
        }

        Vector2 input = new Vector2(Settings.GetAxies(Settings.buttons[2].key, Settings.buttons[3].key), Settings.GetAxies(Settings.buttons[0].key, Settings.buttons[1].key));

        for (int x = 0; x < wheels.Length; x++)
        {
            WheelCollider collider = wheels[x].gameObject.GetComponent<WheelCollider>();

            #region car motor

            if (wheels[x].motor)
            {
                if (rigidbody.velocity.magnitude < topSpeed)
                    collider.motorTorque = Torque * input.y - (input.x * 5);
                else
                    collider.motorTorque = 0;
            }

            #endregion

            #region turnning

            if (wheels[x].turn)
            {
                if (turnning)
                    collider.steerAngle = Mathf.Lerp(slowSteeringAngle, steeringAngle, rigidbody.velocity.magnitude / 4) * input.x;
                else
                    rigidbody.AddForceAtPosition(Vector3.up * 25 * input.x, transform.forward);
            }

            #endregion

            #region wheels mesh

            Vector3 meshPosition = wheels[x].gameObject.transform.position + (wheels[x].gameObject.transform.TransformDirection(-Vector3.up) * (wheels[x].meshSuspentionDistance - (wheels[x].radius / 2)));
            RaycastHit hit;
            if (Physics.Raycast(wheels[x].gameObject.transform.position, -transform.up, out hit, wheels[x].meshSuspentionDistance))
                meshPosition = wheels[x].gameObject.transform.position + (wheels[x].gameObject.transform.TransformDirection(-Vector3.up) * (hit.distance - (wheels[x].radius / 2)));

            int totalChildren = wheels[x].gameObject.transform.childCount;
            for (int c = 0; c < totalChildren; c++)
            {
                wheels[x].gameObject.transform.GetChild(c).transform.position = meshPosition;
                
            }

            #endregion
        }
    }
}