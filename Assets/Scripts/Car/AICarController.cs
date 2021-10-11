using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    public float m_steeringAngle;
    public float angleTowardsWay;
    private float engineRpm = 700;
    private float cylinderFireTimer = 1;
    private int gear = 1;
    public int wayInt = 0;
    public GameObject currentWaypoint;
    public float brakeAccel = 0;

    public List<GameObject> waypoints;
    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public float sensitivity = 0.1f;
    public AudioSource engineExplosion;
    public float idleEngineRpm = 700;
    public float maxEngineRpm = 11000;
    public float cylinderFireTime = 1;
    public float minCylFire = 0.03f;
    public float maxCylFire = 0.1f;
    public int rpmMult = 1000;
    public int gears = 1;
    public int angle = 0;

    private void Steer()
    {
        angleTowardsWay = Vector3.SignedAngle((currentWaypoint.transform.position - transform.position), transform.forward, Vector3.up) + angle;
        m_steeringAngle = -Mathf.Lerp(m_steeringAngle, angleTowardsWay, 0.001f);

        if(brakeAccel < 0)
        {
            m_steeringAngle = -m_steeringAngle;
        }
        //m_steeringAngle *= 2;
        m_steeringAngle = Mathf.Clamp(m_steeringAngle, -maxSteerAngle, maxSteerAngle);
        //m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;

        if(brakeAccel == 1)
        {
            if (m_steeringAngle > (10 + angle) || m_steeringAngle < (angle - 10))
            {
                brakeAccel -= sensitivity;
            }
            else
            {
                brakeAccel += sensitivity;
            }
        }
    }

    private void Accelerate()
    {
        brakeAccel = Mathf.Clamp(brakeAccel, -1, 1);
        frontDriverW.motorTorque = motorForce * brakeAccel;
        frontPassengerW.motorTorque = motorForce * brakeAccel;
        rearPassengerW.motorTorque = motorForce * brakeAccel;
        rearDriverW.motorTorque = motorForce * brakeAccel;
        cylinderFireTime = (0.1f - Mathf.Abs(Mathf.Abs(brakeAccel * 0.5f)));
        cylinderFireTime = Mathf.Clamp(cylinderFireTime, minCylFire, maxCylFire);

        if (engineRpm > maxEngineRpm)
        {
            if (gears > gear)
            {
                gear++;
                engineRpm = idleEngineRpm;
            }
            else
            {
                engineRpm = maxEngineRpm;
            }
        }

        if (engineRpm <= idleEngineRpm)
        {
            if (gear != 1)
            {
                gear--;
            }
        }
    }

    private void Brake()
    {

    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider Wcollider, Transform Wtransform)
    {
        Vector3 pos = Wtransform.position;
        Quaternion quat = Wtransform.rotation;

        Wcollider.GetWorldPose(out pos, out quat);

        Wtransform.position = pos;
        Wtransform.rotation = quat;
    }

    private void PlayEngineSound()
    {
        cylinderFireTimer -= Time.deltaTime;
        if (cylinderFireTimer <= 0)
        {
            engineExplosion.PlayOneShot(engineExplosion.clip);

            cylinderFireTimer = cylinderFireTime;
        }
    }
    private void Update()
    {
        PlayEngineSound();
    }

    private void FixedUpdate()
    {
        Steer();
        //Brake();
        Accelerate();
        UpdateWheelPoses();
    }
    private void Start()
    {
        currentWaypoint = waypoints[wayInt];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == ("way" + wayInt))
        {
            if (wayInt != (waypoints.Count - 1))
            {
                wayInt++;
                currentWaypoint = waypoints[wayInt];
            }
        }
    }
}
