using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{

    #region Variables

    public Wheel[] wheels; //The Wheels of the Car.
    public Vector3 centerOfMass = Vector3.zero; //Center of the mass in vector3.
    public float Torque = 50; //Torque applied to each wheel when pressed accelerate.
    public float topSpeed = 7; //Top Speed of the car.
    public float topSpeedTurning = 7; //Top speed of the car when its turnning.
    public float steeringAngle = 30; //Minimum steering angle of the car.
    public float slowSteeringAngle = 30; //Maximum steering angle of the car.
    public float antiRollForce = 50; //Anit roll bar force amount.
    public float jumpForce = 50; //Jump force when jump is pressed.
    public bool canReset = true; //Can the car be reset.
    public bool canJump = true; //Can the car Jump.

    //GameObject with light component to represent forward lighting of the car.
    public GameObject forwardLight;
    //GameObject with light component to represent backward lighting of the car.
    public GameObject backwardLight;

    #endregion

    //When the scene starts this is executed.
    private void Start()
    {
        rigidbody.centerOfMass = centerOfMass;

        //Setup all the wheels
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

    //Every frame this method is executed.
    private void Update()
    {
        //Anti-Roll Bar Setup
        float antiRollLeft = 0;
        float antiRollRight = 0;

        #region Car Reset

        if (Input.GetKeyDown(Settings.buttons[4].key) && canReset)
        {
            transform.position = transform.position + new Vector3(0, 2, 0);
            transform.rotation = Quaternion.Euler(0, 90, 0);
            canReset = false;
            Invoke("enableReset", 3);
        }

        #endregion

        #region Car Jump

        if (Input.GetKeyDown(Settings.buttons[5].key) && canJump)
        {
            rigidbody.AddForce(transform.up * jumpForce * 5000);
            canJump = false;
            Invoke("enableJump", 2);
        }

        #endregion

        //Get User Input
        Vector2 input = new Vector2(Settings.GetAxies(Settings.buttons[2].key, Settings.buttons[3].key), Settings.GetAxies(Settings.buttons[0].key, Settings.buttons[1].key));

        #region Lights

        //-0.3f because sometimes when the car stops or just rolls back it changes the light.
        if (transform.TransformDirection(rigidbody.velocity).z < -0.3f)
        {
            backwardLight.GetComponent<Light>().enabled = false;
            forwardLight.GetComponent<Light>().enabled = true;
        }
        else
        {
            backwardLight.GetComponent<Light>().enabled = true;
            forwardLight.GetComponent<Light>().enabled = false;
        }

        #endregion

        for (int x = 0; x < wheels.Length; x++)
        {
            WheelCollider collider = wheels[x].gameObject.GetComponent<WheelCollider>();

            #region turnning

            /*
             * Scraped since game is 2D

            if (wheels[x].type == Wheel.WheelType.Turning || wheels[x].type == Wheel.WheelType.MotorAndTurn)
                collider.steerAngle = Mathf.Lerp(collider.steerAngle, Mathf.Lerp(slowSteeringAngle, steeringAngle, Mathf.Log(rigidbody.velocity.magnitude) * 1.1f) * input.x, Time.deltaTime * 15);
            */

            #endregion

            #region car motor

            if (wheels[x].type == Wheel.WheelType.Motor || wheels[x].type == Wheel.WheelType.MotorAndTurn)
            {
                if (rigidbody.velocity.magnitude < topSpeed)
                {
                    if (transform.InverseTransformDirection(rigidbody.velocity).z > 0)
                        collider.motorTorque = Torque * input.y;
                    else if (rigidbody.velocity.magnitude < Mathf.Clamp(topSpeed, 0, 10))
                        collider.motorTorque = Torque * input.y;
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
                wheels[x].wheelSpin += -input.y * Mathf.PI * 50;

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

        #region Rotational Forces

        rigidbody.AddForceAtPosition(-Vector3.up * input.x * 500, Vector3.right);
        rigidbody.AddForceAtPosition(Vector3.up * input.x * 500, -Vector3.right);

        #endregion
    }

    /*
     * When the player clicks reset button a timer starts to make sure the player does 
     * not click it to soon and has to wait after the timer is finished this method 
     * changes the boolean back to true so the player can click reset button again.
    */
    private void enableReset()
    {
        canReset = true;
    }

    /*
     * When the player clicks jump button a timer starts to make sure the player does 
     * not click it to soon and has to wait after the timer is finished this method 
     * changes the boolean back to true so the player can click jump button again.
    */
    private void enableJump()
    {
        canJump = true;
    }
}