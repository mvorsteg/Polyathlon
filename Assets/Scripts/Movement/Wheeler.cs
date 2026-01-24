using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Wheeler : Movement
{
    
 
    public float driveForce = 800f;

    public GameObject wheeler;


    private float forward;
    private float right;
    private Rigidbody wheelerRb;

    void Start()
    {
        

        Rigidbody leftWheel = transform.Find("Rideables/Wheeler/Wheeler Structure/Left Wheel").GetComponent<Rigidbody>();
        leftWheel.solverIterations = 12;
        Rigidbody rightWheel = transform.Find("Rideables/Wheeler/Wheeler Structure/Right Wheel").GetComponent<Rigidbody>();
        rightWheel.solverIterations = 12;
    }

    // enable wheeler
    protected override void OnEnable() 
    {
        base.OnEnable();

        SetWheeler(true);
    }

    // Put away wheeler
    protected override void OnDisable()
    {
        base.OnDisable();
        SetWheeler(false);
    }


    /*  moves the player rigidbody */
    /*
    public override void AddMovement(float forward, float right)
    {
        base.AddMovement(forward, right);
        // they're flipped here for some reason
        this.right = forward;
        this.forward = right;
    }*/

    public override void AddMovement(float inputForward, float inputRight)
    {
        base.AddMovement(inputForward, inputRight);

        if (cameraController == null)
        {
            forward = inputRight;  // W/S
            right   = inputForward; // A/D
            return;
        }

        // Fix flipped input
        float rawForward = inputRight; // W/S
        float rawRight   = inputForward; // A/D

        // ----- Vector from player to camera (XZ) -----
        Vector3 toCamera = cameraController.cameraTransform.position - wheeler.transform.position;
        toCamera = Vector3.ProjectOnPlane(toCamera, Vector3.up).normalized;

        // Forward = +1 when camera behind, -1 when in front
        forward = -Vector3.Dot(wheeler.transform.forward, toCamera) * rawForward;

        // Right = +1 when camera left, -1 when camera right
        right   = -Vector3.Dot(wheeler.transform.right, toCamera) * rawForward + rawRight;

        // Clamp
        forward = Mathf.Clamp(forward, -1f, 1f);
        right   = Mathf.Clamp(right, -1f, 1f);
        Debug.Log("toCamera: " + toCamera + " inputForward: " + inputForward +  " forward: " + forward + " inputRight: " + inputRight + " right: " + right);
    }




//Debug.Log("forward: " + forward + " right: " + right);
//Debug.Log("inputForward: " + inputForward + " forwardDot: " + forwardDot + " forward: " + forward + " inputRight: " + inputRight + " rightDot: " + rightDot + " right: " + right);


    float GetPitchAngle()
    {
        Vector3 forward = transform.forward;
        Vector3 flatForward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
        return Vector3.SignedAngle(flatForward, forward, transform.right);
    }

    /*
    Plan to fix camera-relative motion
    - Add rigidbody to main body of segway
    - Have it match the properties given to the current rigidbody in SetWheeler()
    - When we set wheeler, disable the regular rigidbody. When set wheeler false, re-enable regular rigidbody
    - Also move CharacterMesh to be a child of wheeler gameobject, and move it back when wheeler is false
    - Replace all the references to the regular rigidbody with wheeler rigidbody    
    */
    
    void FixedUpdate()
    {
        ApplyDriveForce();        // translation
        ApplyTurnForce();              // yaw

        // Clamp velocity
        
        maxSpeed = 10;
        
        Vector3 horizontalVel = Vector3.ProjectOnPlane(wheelerRb.linearVelocity, Vector3.up);

        if (horizontalVel.magnitude > maxSpeed)
        {
            Vector3 clampedHorizontal = horizontalVel.normalized * maxSpeed;
            wheelerRb.linearVelocity = clampedHorizontal + Vector3.up * wheelerRb.linearVelocity.y;
        }


        // clamp drift
        float maxDriftSpeed = 1;
        float driftSpeed = Vector3.Dot(wheelerRb.linearVelocity, wheeler.transform.right);

        driftSpeed = Mathf.Clamp(driftSpeed, -maxDriftSpeed, maxDriftSpeed);

        Vector3 lateral = wheelerRb.linearVelocity - wheeler.transform.right * Vector3.Dot(wheelerRb.linearVelocity, wheeler.transform.right);
        wheelerRb.linearVelocity = wheeler.transform.right * driftSpeed + lateral;


    }


    bool IsGrounded()
    {
        return Physics.Raycast(
            wheeler.transform.position,
            -wheeler.transform.up,
            out _,
            1.0f
        );
    }


    void ApplyDriveForce()
    {
        if (forward != 0)
        {
            if (!IsGrounded())
                return;

            Vector3 pitchAxis =
                Vector3.Cross(wheeler.transform.forward, Vector3.up).normalized;

            Vector3 driveDir =
                - Vector3.Cross(pitchAxis, Vector3.up).normalized;

            wheelerRb.AddForce(
                driveDir * forward * driveForce,
                ForceMode.Force
            );
        }
    }


    void ApplyTurnForce()
    {
        // Turn using an offset force so that we can keep the center of mass where it is without
        // causing the wheeler to rotate over the local Z axis
        if (right != 0)
        {
            if (!IsGrounded())
                return;
            
            Vector3 flatForward = Vector3.ProjectOnPlane(wheeler.transform.forward, Vector3.up).normalized;
            Vector3 force = -flatForward * 10;
            Vector3 yawRight = Vector3.Cross(Vector3.up, flatForward); // right, yaw-only
            Vector3 posRight = wheeler.transform.position + yawRight * 10f * right;
            Vector3 posLeft = wheeler.transform.position - yawRight * 10f * right; // left, yaw-only
            wheelerRb.AddForceAtPosition(force, posRight, ForceMode.Impulse);
            wheelerRb.AddForceAtPosition(-force, posLeft, ForceMode.Impulse);
        }

    }



    public override void ApplyJumpSplosion(Vector3 force)
    {
        Jump(true);
        Launch(force);
    }

    public virtual void SetWheeler(bool enabled)
    {
        if (enabled)
        {
            wheeler.SetActive(true);
            wheelerRb = wheeler.GetComponent<Rigidbody>();
            wheeler.GetComponent<ItemPickup>().AssignRacer(racer);
            wheelerRb.centerOfMass = new Vector3(0, -3f, -1f);
            rb.isKinematic = true;

            characterMesh.localPosition = new Vector3(0,0.56f,0);
            characterMesh.parent = wheeler.transform;
            StartCoroutine(cameraController.FollowTransform(wheeler.transform, 1f));
            GetComponent<CapsuleCollider>().enabled = false;
            wheelerRb.isKinematic = false;
            racer.overrideRb = wheelerRb;
        }
        else
        {
            transform.position = wheeler.transform.position;
            wheeler.transform.localPosition = Vector3.zero;
            characterMesh.parent = transform;
            characterMesh.localPosition = new Vector3(0,0,0);
            wheelerRb.isKinematic = false;
            
            // counterintuitively we start this particular coroutine again to stop it
            StartCoroutine(cameraController.FollowTransform(wheeler.transform, 1f));
            rb.isKinematic = false;
            racer.overrideRb = null;
            GetComponent<CapsuleCollider>().enabled = true;
            wheeler.SetActive(false);
        }
    }
}
