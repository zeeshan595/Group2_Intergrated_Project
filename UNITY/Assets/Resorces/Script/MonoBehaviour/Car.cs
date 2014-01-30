using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{
    public Wheel[] wheels;
    public Vector3 centerOfMass = Vector3.zero;
    public float Torque = 50;
    public float topSpeed = 7;
    public float steeringAngle = 30;
    public float slowSteeringAngle = 30;

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
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        for (int x = 0; x < wheels.Length; x++)
        {
            WheelCollider collider = wheels[x].gameObject.GetComponent<WheelCollider>();

            if (wheels[x].motor)
            {
                if (rigidbody.velocity.magnitude < topSpeed)
                    collider.motorTorque = Torque * input.y - (input.x * 5);
                else
                    collider.motorTorque = 0;
            }

            if (wheels[x].turn)
            {
                collider.steerAngle = Mathf.Lerp(slowSteeringAngle, steeringAngle, rigidbody.velocity.magnitude) * input.x;
            }
        }
    }
}