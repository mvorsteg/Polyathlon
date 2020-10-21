using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo2 {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
     
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo2> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;

    private InputActions inputActions;

    private float steer = 0f;
    private float acc = 0f;

    // finds the corresponding visual wheel
    // correctly applies the transform

    private void Awake()
    {
        inputActions = new InputActions();

        inputActions.Driving.Enable();

        inputActions.Driving.Steer.performed += ctx => steer = ctx.ReadValue<float>();
        inputActions.Driving.Steer.canceled += ctx => steer = 0;

        inputActions.Driving.Accelerate.performed += ctx => acc = ctx.ReadValue<float>();
        inputActions.Driving.Accelerate.canceled += ctx => acc = 0;
    }


    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
     
    public void FixedUpdate()
    {
        float motor = maxMotorTorque * acc;
        float steering = maxSteeringAngle * steer;

        Debug.Log(steer);
     
        foreach (AxleInfo2 axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            //ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            //ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}