using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCarController : MonoBehaviour
{
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;
    private float engineRpm = 700;
    private float cylinderFireTimer = 1;
    private int gear = 1;

    public List<GameObject> waypoints;
    public int wayInt = 0;
    private GameObject currentWaypoint;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public float brakeForce = 25;
    public AudioSource engineExplosion;
    public Slider rpmSLider;
    public float idleEngineRpm = 700;
    public float maxEngineRpm = 11000;
    public float cylinderFireTime = 1;
    public float minCylFire = 0.03f;
    public float maxCylFire = 0.1f;
    public int rpmMult = 1000;
    public int gears = 1;

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
        //print(m_steeringAngle);
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
        rearPassengerW.motorTorque = m_verticalInput * motorForce;
        rearDriverW.motorTorque = m_verticalInput * motorForce;
        engineRpm += (m_verticalInput * motorForce)*0.1f;
        cylinderFireTime = (0.1f - Mathf.Abs(Mathf.Abs(m_verticalInput*0.5f)));
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

        if(engineRpm <= idleEngineRpm)
        {
            if (gear != 1)
            {
                gear--;
            }
        }
    }

    private void Brake()
    {
        if(m_verticalInput !> 0)
        {
            frontDriverW.brakeTorque = -m_verticalInput * brakeForce;
            frontPassengerW.brakeTorque = -m_verticalInput * brakeForce;
            rearPassengerW.brakeTorque = -m_verticalInput * brakeForce;
            rearDriverW.brakeTorque = -m_verticalInput * brakeForce;
        }
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
        if(cylinderFireTimer <= 0)
        {
            engineExplosion.PlayOneShot(engineExplosion.clip);

            cylinderFireTimer = cylinderFireTime;
        }
    }
    private void Update()
    {
        PlayEngineSound();
        //rpmSLider.value = engineRpm;
    }

    private void FixedUpdate()
    {
        GetInput();
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
            if(wayInt != (waypoints.Count-1))
            {
                wayInt++;
                currentWaypoint = waypoints[wayInt];
            }
        }
    }
}

