using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Swim : Movement
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
        rb.angularDrag = 0.5f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        maxSpeed = runSpeed;
        acceleration = swimAcceleration;
    }

    /*  moves the player rigidbody */
    public override void AddMovement(float forward, float right)
    {
        base.AddMovement(forward, right);
        Vector3 translation = Vector3.zero;
        // for npcs
        if (cameraController == null)
        {
            translation += right * transform.forward;
            translation += forward * transform.right;    
        }
        // for players
        else
        {
            translation += right * cameraController.transform.forward;
            translation += forward * cameraController.transform.right;
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
                characterMesh.rotation = Quaternion.Lerp(characterMesh.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotationSpeed);
            }
            else
            {
                smoothSpeed = Mathf.Lerp(smoothSpeed, 0, Time.deltaTime*8);
            }
            
            // blend speed in animator to match pace of footsteps
            // normal movement (character moves independent of camera)
            speed = Mathf.SmoothStep(speed, actualVelocity.magnitude, Time.deltaTime * 20);
            anim.SetFloat("speed", speed, dampTime, Time.deltaTime);
    }

    
    /*  causes the player to jump */
    public override void Jump(bool hold)
    {
        base.Jump(hold);
    }

    /*  grounds the player after a jump is complete */
    protected override void Land()
    {
        grounded = true;
        anim.SetTrigger("land");
    }
}
