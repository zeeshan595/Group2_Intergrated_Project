using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{

    #region Variables

    public Wheel[] wheels; //The Wheels of the Car.
    public GameObject[] attachments; //Attachments attached to the car.
    public Vector3 centerOfMass = Vector3.zero; //Center of the mass in vector3.
    public int health = 3; //Health of the car.
    public Texture[] healthTexture;
    public float torque = 50; //Torque applied to each wheel when pressed accelerate.
    public float topSpeed = 7; //Top Speed of the car.
    public float topSpeedReverse = 7; //Top speed of the car when its going backwards.
    public float steeringAngle = 30; //Minimum steering angle of the car.
    public float slowSteeringAngle = 30; //Maximum steering angle of the car.
    public bool antiRollBars = false;
    public float antiRollForce = 50; //Anit roll bar force amount.
    public float jumpForce = 50; //Jump force when jump is pressed.
    public bool canReset = true; //Can the car be reset.
    public bool canJump = true; //Can the car Jump.

    //GameObject with light component to represent forward lighting of the car.
    public GameObject forwardLight;
    //GameObject with light component to represent backward lighting of the car.
    public GameObject backwardLight;

    //When Passed a checkpoint this will update
    [System.NonSerialized]
    public Vector3 resetPosition;

    private float timer = 0;

    #endregion

    //When the scene starts this is executed.
    private void Start()
    {
        resetPosition = transform.position;
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
        //Update Timer
        timer += Time.deltaTime;

        //Anti-Roll Bar Setup
        float antiRollLeft = 0;
        float antiRollRight = 0;

        #region Car Reset

        if (Input.GetKeyDown(Settings.buttons[4].key) && canReset)
        {
            transform.position = resetPosition;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        #endregion

        #region Car Jump

        if ((Input.GetKeyDown(Settings.buttons[5].key) || (Input.touchCount > 1 && (Input.touches[Input.touchCount - 1].phase == TouchPhase.Began || Input.touches[0].phase == TouchPhase.Began))) && canJump)
        {
            rigidbody.AddForce(transform.up * jumpForce * 5000);
        }
        canJump = false;

        #endregion

        #region Get Input
        Vector2 input = Vector2.zero;

#if UNITY_ANDROID
        input.x = Input.acceleration.x * 3;

        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 2)
                input.y = -1;
            else
                input.y = 1;
        }
#else
        //3 * -(((Screen.width / 2) - Input.mousePosition.x) / (Screen.width / 2))
        input = new Vector2(Settings.GetAxies(Settings.buttons[2].key, Settings.buttons[3].key), Settings.GetAxies(Settings.buttons[0].key, Settings.buttons[1].key));
#endif
        #endregion

        #region Lights

        //-0.3f because sometimes when the car stops or just rolls back it changes the light.
        if (transform.TransformDirection(rigidbody.velocity).z < 0.3f)
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

        #region anti gravity Easter Egg

        if (Input.GetKeyDown(KeyCode.G))
        {
            rigidbody.useGravity = !rigidbody.useGravity;
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
                        collider.motorTorque = torque * input.y;
                    else if (rigidbody.velocity.magnitude < topSpeedReverse)
                        collider.motorTorque = torque * input.y;
                    else
                        collider.motorTorque = 0;
                }
                else
                    collider.motorTorque = 0;
            }

            #endregion

            #region wheels mesh

            float meshHeight = -wheels[x].suspentionDistance;
            RaycastHit hit;
            if (Physics.Raycast(wheels[x].gameObject.transform.position, -transform.up, out hit, wheels[x].meshSuspentionDistance))
            {
                meshHeight = -hit.distance + wheels[x].radius;

                if (wheels[x].type == Wheel.WheelType.Turning || wheels[x].type == Wheel.WheelType.Stationary || input.y == 0)
                    wheels[x].wheelSpin += transform.InverseTransformDirection(rigidbody.velocity).z * Mathf.PI;
                else
                    wheels[x].wheelSpin += transform.InverseTransformDirection(rigidbody.velocity).z * Mathf.PI;

                if (!canJump)
                    canJump = true;
            }
            else if (wheels[x].type == Wheel.WheelType.Motor || wheels[x].type == Wheel.WheelType.MotorAndTurn)
                wheels[x].wheelSpin += -input.y * Mathf.PI * 50;

            int totalChildren = wheels[x].gameObject.transform.childCount;
            for (int c = 0; c < totalChildren; c++)
            {
                meshHeight = Mathf.Clamp(meshHeight + wheels[x].meshYOffset, -wheels[x].suspentionDistance, 0);
                wheels[x].gameObject.transform.GetChild(c).transform.position = wheels[x].gameObject.transform.position + wheels[x].gameObject.transform.TransformDirection(new Vector3(0, meshHeight, 0));
                wheels[x].gameObject.transform.GetChild(c).transform.localRotation = Quaternion.Euler(new Vector3(wheels[x].wheelSpin, collider.steerAngle, 90));
            }

            #endregion

            #region Anti Roll Bar Calculations

            if (antiRollBars)
            {
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
            }

            #endregion
        }

        #region Apply Anti Roll Bar Forces

        if (antiRollBars)
        {
            rigidbody.AddForceAtPosition(-Vector3.up * (antiRollLeft - antiRollRight), -transform.right * (transform.localScale.x / 2));
            rigidbody.AddForceAtPosition(-Vector3.up * (antiRollRight - antiRollLeft), transform.right * (transform.localScale.x / 2));
        }

        #endregion
        
        #region Rotational Forces

        rigidbody.AddForceAtPosition(-Vector3.up * input.x * 500, Vector3.right);
        rigidbody.AddForceAtPosition(Vector3.up * input.x * 500, -Vector3.right);

        #endregion
    }

    #region Health

    public int rediuceHealth(int amount)
    {
        health -= amount;
        return health;
    }

    public int increaseHealth(int amount)
    {
        health += amount;
        return health;
    }

    #endregion

#if UNITY_ANDROID
    //RESET BUTTON FOR ANDROID

    private void OnGUI()
    {
        if (GUI.Button(new Rect(5, 5, 100, 100), "Reset"))
        {
            transform.position = resetPosition;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
#else
    private void OnGUI()
    {
        /*
        //GUILayout.Box(timer.ToString());
        if (health == 3)
            GUI.DrawTexture(new Rect(5, Screen.height - 505, 500, 500), healthTexture[0]);
        else if (health == 2)
            GUI.DrawTexture(new Rect(5, Screen.height - 505, 500, 500), healthTexture[1]);
        else if (health == 1)
            GUI.DrawTexture(new Rect(5, Screen.height - 505, 500, 500), healthTexture[2]);
        else
            GUI.DrawTexture(new Rect(5, Screen.height - 505, 500, 500), healthTexture[3]);
         */
    }
#endif
}