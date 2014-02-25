using UnityEngine;
using System;

[Serializable]
public class Wheel
{
    // Different types of wheels.
    public enum WheelType
    {
        Motor = 0,
        Turning = 1,
        Stationary = 2,
        MotorAndTurn = 3
    }

    // Different wheel positions (used for anti-rollbar system).
    public enum WheelPosition
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public GameObject gameObject; // Actuall wheel gameObject.
    public WheelType type; // The type of the wheel.
    public WheelPosition wheelPosition; // The position of the wheel.
    public float mass; // The mass of the wheel.
    public float radius; // The radius of the wheel.
    public float suspentionDistance; // How big the suspension of the wheel is.
    public float suspentionSpring; // How tight the spring of the suspension is.
    public float suspentionDamper; // Suspension Damper for the spring.
    public float meshSuspentionDistance = 0.6f; // The mesh distance from ground.
    public float meshYOffset = 0; // Mesh ofset incase the mesh is not aligned properly.
    [NonSerialized]
    public float wheelSpin = 0; // Wheel spin (used to rotate the wheel).
}