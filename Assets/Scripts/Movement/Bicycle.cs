using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bicycle : Movement
{
    public float amt = 1f;

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
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    [Range(0,1)]
    public float lean = 0.5f;
    public float smoothLean = 0.5f;

    private float rotationValue = 0f;
    public float rotSpeed = 10;

    private Quaternion startForkRot;
    private Vector3 upDirection = Vector3.up;

    public float speedModifier = 10f;

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        bike.SetActive(true);
        //rb.mass = 50;
        rb.angularDrag = 0;
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        rb.centerOfMass = centerOfMass.localPosition;

        maxSpeed = bikeSpeed;
        acceleration = 10f;
        angularSpeed = 65f;

        characterMesh.localPosition = new Vector3(0, 0.625f, -0.7f);
        characterMesh.localEulerAngles = new Vector3(56.975f, 0, 0);

        lPedal = pedals.transform.GetChild(0).gameObject;
        rPedal = pedals.transform.GetChild(1).gameObject;
        startForkRot = fork.transform.localRotation;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        bike.SetActive(false);
    }

    public override void AddMovement(float forward, float right)
    {
        base.AddMovement(forward, right);

        Vector3 translation = Vector3.zero;
        float rot = 0;

        if (right > 0)
            translation += right * transform.forward;
        rot += forward * rotSpeed;
        
        translation.y = 0;
        if (translation.magnitude > 0)
        {
            velocity = translation;
        }
        else
        {
            velocity = Vector3.zero;
        }

        // moved from update
        if (velocity.magnitude > 0)
        {
            rb.velocity = new Vector3(velocity.normalized.x * smoothSpeed, rb.velocity.y, velocity.normalized.z * smoothSpeed);
            smoothSpeed = Mathf.Lerp(smoothSpeed, maxSpeed * bonusSpeed, Time.deltaTime);    
        }
        else
        {
            smoothSpeed = Mathf.Lerp(smoothSpeed, 0, Time.deltaTime * 8);
        }

        if (rb.velocity.magnitude > 0.001 && forward != 0)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + (rot * Time.deltaTime), 0);
            if (rb.velocity.magnitude > 10f)
            if (forward > 0)
            {
                lean = Mathf.Lerp(lean, 45f, Time.deltaTime);
            }
            else
            {
                lean = Mathf.Lerp(lean, -45f, Time.deltaTime);   
            }
        }
        else
        {
            lean = Mathf.Lerp(lean, 0.5f, Time.deltaTime * 4);
        }
        //characterMesh.localEulerAngles = new Vector3(characterMesh.localEulerAngles.x, characterMesh.localEulerAngles.y, lean);
        //bike.transform.localEulerAngles = new Vector3(bike.transform.localEulerAngles.x, bike.transform.localEulerAngles.y, lean);
    
        // if the player landed, enable another jump
        if (!grounded)
        {
            RaycastHit hit;
            if (falling && Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.2f, 0), out hit))
            {
                falling = false;
                Land();
            }
        }
        // blend speed in animator to match pace of footsteps
        // normal movement (character moves independent of camera)
        
        speed = Mathf.SmoothStep(speed, actualVelocity.magnitude, Time.deltaTime * 20);


        //setRotationAndSpeed(forward, right);
        RotateMeshes();
        RotateFork();
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
        upDirection = Vector3.Normalize(Vector3.up + transform.right * maxSteeringAngle * lean * rotationValue* rb.velocity.magnitude / 100);
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