using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bicycle : Movement
{

    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider wheel;
        public bool motor; // is this wheel attached to motor?
        public bool steering; // does this wheel apply steer angle?
    }


    [Header ("GameObjects")]

    public GameObject bike;

    public GameObject rearWheel;
    public GameObject frontWheel;
    public GameObject pedals;
    private GameObject lPedal;
    private GameObject rPedal;
    public GameObject fork;
    public Transform centerOfMass;

    [Header("Values")]
    public float oneRotationSpeed = 2.7f;
    public float crankMultiplier = 2f;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    [Range(0,1)]
    public float relativeLeanAmount = 0f;
    public Transform leftWheels;
    public Transform rightWheels;

    private float rotationValue = 0f;
    public float rotSpeed = 10;

    private Vector3[] wheelPositions;
    private Quaternion startForkRot;
    private Vector3 upDirection = Vector3.up;


    public float speedModifier = 10f;

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        bike.SetActive(true);
        rb.mass = 100;
        rb.angularDrag = 1;
        rb.constraints = RigidbodyConstraints.None;
        rb.centerOfMass = centerOfMass.localPosition;

        characterMesh.localPosition = new Vector3(0, 0.625f, -0.7f);
        characterMesh.localEulerAngles = new Vector3(56.975f, 0, 0);

        lPedal = pedals.transform.GetChild(0).gameObject;
        rPedal = pedals.transform.GetChild(1).gameObject;
        startForkRot = fork.transform.localRotation;
        wheelPositions = new Vector3[axleInfos.Count];
        for (int i = 0; i < axleInfos.Count; i++)
        {
            wheelPositions[i] = axleInfos[i].wheel.center;
            
        }
    }

    public override void AddMovement(float forward, float right)
    {
        setRotationAndSpeed(forward, right);
        RotateMeshes();
        RotateFork();
    }

    public void FixedUpdate()
    {
        ApplyWheelForce();
        //RotateStraight();
    }

    private void RotateMeshes()
    {
        RotateObject(pedals, 1);
        RotateObject(lPedal, -1);
        RotateObject(rPedal, -1);
        RotateObject(rearWheel, crankMultiplier);
        RotateObject(frontWheel, crankMultiplier);
    }

    void RotateFork()
    {
        fork.transform.localRotation = startForkRot;
        fork.transform.RotateAround(fork.transform.position, fork.transform.up, maxSteeringAngle * rotationValue);
    }

    void Lean()
    {
        upDirection = Vector3.Normalize(Vector3.up + transform.right * maxSteeringAngle * relativeLeanAmount * rotationValue* rb.velocity.magnitude / 100);
    }
    

    void ApplyWheelForce()
    {
        float motor = maxMotorTorque;
        float steering = maxSteeringAngle * rotationValue;

        leftWheels.localPosition = - Vector3.up * relativeLeanAmount * rotationValue * rb.velocity.magnitude;
        rightWheels.localPosition = Vector3.up * relativeLeanAmount * rotationValue * rb.velocity.magnitude;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            
            if (axleInfo.steering)
            {
                axleInfo.wheel.steerAngle = steering;
            }
            if (axleInfo.motor && rb.velocity.magnitude < maxSpeed)
            {
                axleInfo.wheel.motorTorque = motor;
            }
            else if(axleInfo.motor)
            {
                axleInfo.wheel.motorTorque = 0;
            }
        }
    }

    public void setRotationAndSpeed(float x, float y)
    {
        rotationValue = x;
        maxSpeed = y * bikeSpeed;
    }
    
    //rotates the meshes
    private void RotateObject(GameObject obj, float multiplier)
    {
        obj.transform.Rotate(Time.deltaTime * rb.velocity.magnitude * (360f / oneRotationSpeed) * multiplier, 0, 0);
        //obj.transform.Rotate(Time.deltaTime * rotSpeed * (360f / oneRotationSpeed) * multiplier, 0, 0);
    }

    public override void Jump(bool hold)
    {
        base.Jump(hold);
    }
}