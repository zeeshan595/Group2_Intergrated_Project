using UnityEngine;
using System;

[Serializable]
public class Wheel
{
    public enum WheelType
    {
        Motor = 0,
        Turning = 1,
        Stationary = 2,
        MotorAndTurn = 3
    }

    public enum WheelPosition
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public GameObject gameObject;
    public WheelType type;
    public WheelPosition wheelPosition;
    public float mass;
    public float radius;
    public float suspentionDistance;
    public float suspentionSpring;
    public float suspentionDamper;
    public float meshSuspentionDistance = 0.6f;
    public float meshYOffset = 0;
    [NonSerialized]
    public float wheelSpin = 0;
}