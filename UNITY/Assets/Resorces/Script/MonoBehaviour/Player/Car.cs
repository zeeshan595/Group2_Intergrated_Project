using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{

    #region Variables;

    public Wheel[] wheels;
    public Vector3 centerOfMass = Vector3.zero;
    public float Torque = 50;
    public float topSpeed = 7;
    public float topSpeedTurning = 7;
    public float steeringAngle = 30;
    public float slowSteeringAngle = 30;
    public float antiRollForce = 50;

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
        //Anti-Roll Forces
        float antiRollLeft = 0;
        float antiRollRight = 0;

        //Reset Car
        if (Input.GetKeyDown(Settings.buttons[4].key))
        {
            transform.position = transform.position + new Vector3(0, 2, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }

        //Get User Input
        Vector2 input = new Vector2(Settings.GetAxies(Settings.buttons[2].key, Settings.buttons[3].key), Settings.GetAxies(Settings.buttons[0].key, Settings.buttons[1].key));

        for (int x = 0; x < wheels.Length; x++)
        {
            WheelCollider collider = wheels[x].gameObject.GetComponent<WheelCollider>();

            #region turnning

            if (wheels[x].type == Wheel.WheelType.Turning || wheels[x].type == Wheel.WheelType.MotorAndTurn)
            {
                collider.steerAngle = Mathf.Lerp(collider.steerAngle, Mathf.Lerp(slowSteeringAngle, steeringAngle, Mathf.Log(rigidbody.velocity.magnitude) * 1.1f) * input.x, Time.deltaTime * 15);
            }

            #endregion

            #region car motor

            if (wheels[x].type == Wheel.WheelType.Motor || wheels[x].type == Wheel.WheelType.MotorAndTurn)
            {
                if (rigidbody.velocity.magnitude < topSpeed)
                {
                    if (transform.InverseTransformDirection(rigidbody.velocity).z > 0)
                        collider.motorTorque = Torque * input.y - (input.x * 5);
                    else if (rigidbody.velocity.magnitude < 10)
                        collider.motorTorque = Torque * input.y - (input.x * 5);
                    else
                        collider.motorTorque = 0;
                }
                else
                    collider.motorTorque = 0;
            }

            #endregion

            #region wheels mesh

            Vector3 meshPosition = wheels[x].gameObject.transform.position + (wheels[x].gameObject.transform.TransformDirection(-Vector3.up) * (wheels[x].meshSuspentionDistance - (wheels[x].radius / 2)));
            RaycastHit hit;
            if (Physics.Raycast(wheels[x].gameObject.transform.position, -transform.up, out hit, wheels[x].meshSuspentionDistance))
            {
                meshPosition = wheels[x].gameObject.transform.position + (wheels[x].gameObject.transform.TransformDirection(-Vector3.up) * (hit.distance - (wheels[x].radius / 2)));

                if (wheels[x].type == Wheel.WheelType.Turning || wheels[x].type == Wheel.WheelType.Stationary || input.y == 0)
                    wheels[x].wheelSpin += transform.InverseTransformDirection(rigidbody.velocity).z * Mathf.PI;
                else
                    wheels[x].wheelSpin += transform.InverseTransformDirection(rigidbody.velocity).z * Mathf.PI;
            }
            else if (wheels[x].type == Wheel.WheelType.Motor || wheels[x].type == Wheel.WheelType.MotorAndTurn)
                wheels[x].wheelSpin += transform.InverseTransformDirection(rigidbody.velocity).z * Mathf.PI;

            meshPosition += new Vector3(0, wheels[x].meshYOffset, 0);

            int totalChildren = wheels[x].gameObject.transform.childCount;
            for (int c = 0; c < totalChildren; c++)
            {
                wheels[x].gameObject.transform.GetChild(c).transform.position = meshPosition;
                wheels[x].gameObject.transform.GetChild(c).transform.localRotation = Quaternion.Euler(new Vector3(wheels[x].wheelSpin, collider.steerAngle, 90));
            }

            #endregion

            #region Anti Roll Bar Calculations

            if (wheels[x].wheelPosition != Wheel.WheelPosition.Middle)
            {
                float wheelTravel;
                WheelHit wheelHit;
                bool ground = collider.GetGroundHit(out wheelHit);

                if (ground)
                    wheelTravel = (-collider.transform.InverseTransformPoint(wheelHit.point).y - collider.radius) / collider.suspensionDistance;
                else
                    wheelTravel = 1.0f;

                float wheelRollForce = wheelTravel * antiRollForce * rigidbody.velocity.magnitude;

                if (wheels[x].wheelPosition == Wheel.WheelPosition.Left)
                {
                    antiRollLeft += wheelRollForce;
                }
                else if (wheels[x].wheelPosition == Wheel.WheelPosition.Right)
                {
                    antiRollRight += wheelRollForce;
                }
            }

            #endregion
        }

        #region Apply Anti Roll Bar Forces

        rigidbody.AddForceAtPosition(-Vector3.up * (antiRollLeft - antiRollRight), -transform.right * (transform.localScale.x / 2));
        rigidbody.AddForceAtPosition(-Vector3.up * (antiRollRight - antiRollLeft), transform.right * (transform.localScale.x / 2));

        #endregion
    }
}