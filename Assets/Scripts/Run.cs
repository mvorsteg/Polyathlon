using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Run : Movement
{

    // used to smooth out speed transition an animation
    protected float speed = 0;
    protected float smoothSpeed = 0;

    private float bonusSpeed = 1f;

    private const float dampTime = 0.05f; // reduce jittering in animator by providing dampening

    public float Direction { get => actualVelocity == Vector3.zero ? 0f : Mathf.Abs(Quaternion.LookRotation(actualVelocity, Vector3.up).eulerAngles.y - characterMesh.transform.rotation.eulerAngles.y); }
    public float BonusSpeed { get => bonusSpeed; set => bonusSpeed = value; }

    protected override void OnEnable() 
    {
        base.OnEnable();
        rb.mass = 1;
        rb.angularDrag = 0;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        maxSpeed = runSpeed;
        acceleration = runAcceleration;
    }

    /*  moves the player rigidbody */
    public override void AddMovement(float forward, float right)
    {
    
        base.AddMovement(forward, right);
        
        Vector3 translation = Vector3.zero;
        if (!orientRotation)
        {
            translation += right * characterMesh.transform.forward;;
            translation += forward * characterMesh.transform.right;
        }
        else
        {
            if (cameraController == null)
            {
                translation += right * transform.forward;
                translation += forward * transform.right;    
            }
            else
            {
                translation += right * cameraController.transform.forward;
                translation += forward * cameraController.transform.right;
            }
        }
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
            // rotate the character mesh if enabled
            if (orientRotation)
            {
                characterMesh.rotation = Quaternion.Lerp(characterMesh.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            smoothSpeed = Mathf.Lerp(smoothSpeed, 0, Time.deltaTime*8);
        }
    
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
        if (orientRotation)
        {
            speed = Mathf.SmoothStep(speed, actualVelocity.magnitude, Time.deltaTime * 20);
        }
        // movement when aiming (straft left/right/forward/backward)
        else
        {
            float speedY = Vector3.Dot(actualVelocity, anim.transform.right);
            anim.SetFloat("speedY", speedY, dampTime, Time.deltaTime);
            speed = Vector3.Dot(actualVelocity, anim.transform.forward);
        }
        anim.SetFloat("speed", speed, dampTime, Time.deltaTime);
        //Debug.Log("velocity" + velocity);
    }
    
    /* causes the player to jump */
    public override void Jump()
    {
        if (orientRotation && grounded)
        {
            if (Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.1f, 0)))
            {
                rb.AddForce(Vector3.up * jumpForce);
                grounded = false;
                anim.SetTrigger("jump");
            }
        
        }
    }

    /* fires the players jetpack
       this is called every frame */
    public override void Jetpack(bool fire)
    {
        base.Jetpack(fire);
        if (fire) {
            rb.AddForce(jetpackTransform.up * jetpackForce * Time.deltaTime);
           // rb.velocity = Vector3.ClampMagnitude(rb.velocity, jetpackSpeed);
            grounded = false;
            anim.SetTrigger("jump");
        }
    }

    /*  grounds the player after a jump is complete */
    protected override void Land()
    {
        grounded = true;
        anim.SetTrigger("land");
    }
}
