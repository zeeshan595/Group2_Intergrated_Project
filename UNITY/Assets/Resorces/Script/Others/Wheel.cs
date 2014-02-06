using UnityEngine;
using System;

[Serializable]
public class Wheel
{
    public GameObject gameObject;
    public bool turn;
    public bool motor;
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