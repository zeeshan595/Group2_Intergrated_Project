using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{

    #region Variables

    public Wheel[] wheels; //The Wheels of the Car.
    public Vector3 centerOfMass = Vector3.zero; //Center of the mass in vector3.
    public int health = 3; //Health of the car.
    public float torque = 50; //Torque applied to each wheel when pressed accelerate.
    public float topSpeed = 7; //Top Speed of the car.
    public float topSpeedReverse = 7; //Top speed of the car when its going backwards.
    public float steeringAngle = 30; //Minimum steering angle of the car.
    public float slowSteeringAngle = 30; //Maximum steering angle of the car.
    public float soundPitch = 1;
    public int numberOfGears = 3;
    public float minVolume = 0.1f;
    public float maxVolume = 0.4f;
    public bool antiRollBars = false;
    public float antiRollForce = 50; //Anit roll bar force amount.
    public float jumpForce = 50; //Jump force when jump is pressed.
    public bool canReset = true; //Can the car be reset.

    //
    public GameObject jumpEffect;

    //GameObject with light component to represent forward lighting of the car.
    public GameObject forwardLight;
    //GameObject with light component to represent backward lighting of the car.
    public GameObject backwardLight;

    //When Passed a checkpoint this will update
    [System.NonSerialized]
    public Vector3 resetPosition;
    [System.NonSerialized]
    public float timer = 0;

    private float[] gearRatioForSound;
    private float currentSpeed = 0;

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

        gearRatioForSound = new float[numberOfGears];
        for (int x = 0; x < gearRatioForSound.Length; x++)
            gearRatioForSound[x] = ((topSpeed + 5) / numberOfGears) * (x + 1);

        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void resetCar()
    {
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        transform.position = resetPosition;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        Object[] gameObjects = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (Object g in gameObjects)
        {
            if (((GameObject)g).GetComponent<editorObject>() && ((GameObject)g).GetComponent<editorObject>().backOnReset)
                ((GameObject)g).transform.position = ((GameObject)g).GetComponent<editorObject>().originalPosition;
        }
    }

    //Every frame this method is executed.
    private void Update()
    {
        bool isGrounded = false;

        //Update Timer
        timer += Time.deltaTime;

        //Anti-Roll Bar Setup
        float antiRollLeft = 0;
        float antiRollRight = 0;

        #region Car Reset

        if ((Input.GetKeyDown(Settings.buttons[4].key) || Input.GetKeyDown(KeyCode.Joystick1Button1)) && canReset)
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

                isGrounded = true;
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

        #region Engine Sound

        int i = 0;

        if (isGrounded)
            currentSpeed = Mathf.Abs(transform.TransformDirection(rigidbody.velocity).z);
        else if (Mathf.Abs(input.y) > 0)
        {
            currentSpeed = topSpeed;
        }
        else
        {
            currentSpeed = 0;
        }

        for (i = 0; i < gearRatioForSound.Length; i++)
        {
            if (gearRatioForSound[i] > currentSpeed)
                break;
        }

        float gearMinValue = 0;
        float gearMaxValue = 0;

        if (i == 0)
            gearMinValue = 0;
        else
            gearMinValue = gearRatioForSound[i - 1];

        gearMaxValue = gearRatioForSound[i];
        float enginePitch = ((currentSpeed - gearMinValue) / (gearMaxValue - gearMinValue));
        audio.volume = Mathf.Lerp(audio.volume, Mathf.Clamp(enginePitch, minVolume, maxVolume), Time.deltaTime * 2);
        audio.pitch = Mathf.Lerp(audio.pitch, enginePitch + soundPitch, Time.deltaTime * 5);

        #endregion

        #region Car Jump

        if ((Input.GetKeyDown(Settings.buttons[5].key) || Input.GetKeyDown(KeyCode.Joystick1Button0) || (Input.touchCount > 1 && (Input.touches[Input.touchCount - 1].phase == TouchPhase.Began || Input.touches[0].phase == TouchPhase.Began))) && isGrounded)
        {
            if (Settings.carType == Settings.CarType.ScienceFiction)
                rigidbody.AddForce(new Vector3(0.5f, 1, 0) * jumpForce * 5000);
            else
                rigidbody.AddForce(transform.up * jumpForce * 5000);

            if (jumpEffect != null)
                Instantiate(jumpEffect, transform.position, Quaternion.identity);
        }

        #endregion
    }

    private void OnGUI()
    {
        GUILayout.Box(TimerToString(timer));
    }

    #region helpers

    private string TimerToString(float seconds)
    {
        int min = 0;
        while (seconds > 60)
        {
            seconds -= 60;
            min++;
        }

        return min + ":" + Round(seconds, 2);
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    #endregion 
}