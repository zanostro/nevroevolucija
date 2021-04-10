using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float brake = 0;

    public Global global;
    public float motor;
    public float steering;


    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    public void Start()
    {
        axleInfos[0].leftWheel.ConfigureVehicleSubsteps(5, 12, 15);
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        brake = 1;
    }


    public void FixedUpdate()
    {
        float d = 0.0f;
        if (global.isActive) d = 1.0f;
             
        motor = maxMotorTorque * ((global.gas *2) -1) *d * brake;          //gas
        steering = maxSteeringAngle * ((global.stearing *2) - 1) *d * brake;     //right left


        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; 
    public bool steering; 
}