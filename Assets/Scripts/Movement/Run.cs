﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Run : Movement
{
    public float Direction { get => actualVelocity == Vector3.zero ? 0f : Mathf.Abs(Quaternion.LookRotation(actualVelocity, Vector3.up).eulerAngles.y - characterMesh.transform.rotation.eulerAngles.y); }

    protected override void OnEnable() 
    {
        base.OnEnable();
        rb.mass = 1;
        rb.angularDrag = 0;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        maxSpeed = runSpeed;
        acceleration = runAcceleration;
        angularSpeed = 120f;
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
    
        anim.SetFloat("speed", speed, dampTime, Time.deltaTime);
        //Debug.Log("velocity" + velocity);
    }
    
    /* causes the player to jump */
    public override void Jump(bool hold)
    {
        base.Jump(hold);
        if (grounded)
        {
            if (Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.1f, 0)))
            {
                rb.AddForce(Vector3.up * jumpForce);
                grounded = false;
                anim.SetTrigger("jump");
            }
        }
    }

    /*  grounds the player after a jump is complete */
    protected override void Land()
    {
        grounded = true;
        anim.SetTrigger("land");
    }
}