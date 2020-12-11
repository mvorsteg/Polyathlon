using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Jetpack : Movement
{

    public GameObject jetpack;
    protected ParticleSystem[] jetpackExhaust;
    protected bool fireJetpack; // this tells us whether we will be firing the jetpack during this frame
    
    public float Direction { get => actualVelocity == Vector3.zero ? 0f : Mathf.Abs(Quaternion.LookRotation(actualVelocity, Vector3.up).eulerAngles.y - characterMesh.transform.rotation.eulerAngles.y); }

    private void Start()
    {
        jetpackExhaust = jetpack.transform.GetComponentsInChildren<ParticleSystem>();    
    }

    // Set up jetpack
    protected override void OnEnable() 
    {
        base.OnEnable();
        rb.mass = 1;
        rb.angularDrag = 0;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        maxSpeed = runSpeed;
        acceleration = runAcceleration;
        angularSpeed = 120f;

        SetJetpack(true);
    }

    // Put away jetpack
    protected override void OnDisable()
    {
        base.OnDisable();
        SetJetpack(false);
    }

    public virtual void SetJetpack(bool enabled)
    {
        if (jetpack != null)
            jetpack.SetActive(enabled);
        anim.SetBool("jetpack", enabled);
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

        // if not flying
        if (grounded)
        {
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
        }
        // if they are flying
        else if (!grounded)
        {
            // don't move player, just rotate
            if (forward != 0 || right != 0)
                characterMesh.rotation = Quaternion.Lerp(characterMesh.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotationSpeed);
            // if the player landed, enable another jump
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
    
    /*  causes the player to fire their jetpack */
    public override void Jump(bool hold)
    {
        base.Jump(hold);
        if (!hold)
        {
            // shut down the jetpack
            fireJetpack = false;
        }
        else
        {
            // Start firing the jetpack
            StartCoroutine(JetpackThrust());    
        }
        if (grounded)
        {
            grounded = false;
        }
        if (cameraController != null)
        {
            cameraController.SetXMinMax(-60f, 70f);
        }
    }

    
    private IEnumerator JetpackThrust()
    {
        // no need to restart the coroutine if we've already started it
        if (fireJetpack)
        {
            yield break;
        }
        fireJetpack = true;
        SetParticles(true);
        // Handle thrust
        while(fireJetpack && !racer.IsDead())
        {
            rb.AddForce(jetpack.transform.up * jetpackForce * Time.deltaTime);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, jetpackSpeed);
            grounded = false;
            anim.SetTrigger("jump");
            yield return null;
        }
        SetParticles(false);
    }

    public void SetParticles(bool fire)
    {
        // Handle particle systems for the exhaust
        if (fire)
        {
            foreach(ParticleSystem nozzle in jetpackExhaust)
            {
                //Debug.Log("Play " + nozzle.gameObject);
                nozzle.Play();
            }
        }
        else
        {
            foreach(ParticleSystem nozzle in jetpackExhaust)
            {
                //Debug.Log("Stop " + nozzle.gameObject);
                nozzle.Stop();
            }
        }
    }

    /*  grounds the player after a jump is complete */
    protected override void Land()
    {
        grounded = true;
        anim.SetTrigger("land");
        if (racer is NPC)
        {
            ((NPC)racer).Land();
        }
        else
        {
            cameraController.ResetXMinMax();
        }
    }
}
