using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Glider : Movement
{

    public GameObject glider;
    public Transform characterHips;
    public float liftFactor = 10f;
    public float rollSpeed = 10f;
    public float pitchSpeed = 10f;
    public float dragFactor = 10f;
    private Vector3 lastVelocity;
    
    private AudioSource audioSource;
    private bool gliding = false; 
    private bool landable = false; // lets us know if we're allowed to call Land()
    private bool countingDownLandable = false; // prevents PreventFalseLanding() coroutine from being started if its running


    // Set up glider
    protected override void OnEnable() 
    {
        base.OnEnable();
        rb.mass = 1;
        rb.angularDrag = 0;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        maxSpeed = runSpeed;
        acceleration = runAcceleration;
        angularSpeed = 120f;

        SetGlider(true);
    }

    // Put away glider
    protected override void OnDisable()
    {
        base.OnDisable();
        SetGlider(false);
    }

    public virtual void SetGlider(bool enabled)
    {
        if (glider != null)
            glider.SetActive(enabled);
        anim.SetBool("glide", enabled);
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
                rb.velocity = new Vector3(velocity.normalized.x * smoothSpeed, racer is NPC ? 0 : rb.velocity.y, velocity.normalized.z * smoothSpeed);
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
            // don't move player, just roll and pitch
            if (forward != 0 || right != 0)
            {
                characterMesh.RotateAround(characterHips.position, characterHips.right, pitchSpeed * right * Time.deltaTime);
                characterMesh.RotateAround(characterHips.position, characterHips.up, rollSpeed * - forward * Time.deltaTime);
            }
            RaycastHit hit;
            // allow exception to NPC from landable rule because their navmesh gets messed up otherwise
            if ((landable) && Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.2f, 0), out hit))
            {
                characterMesh.position = transform.position;
                characterMesh.localEulerAngles = new Vector3(0, characterMesh.localEulerAngles.y, 0);
                rb.drag = 0;
                Land();
                Debug.Log(gameObject.name + "has landed");
            }
        }
        // blend speed in animator to match pace of footsteps
        // normal movement (character moves independent of camera)
        
        speed = Mathf.SmoothStep(speed, actualVelocity.magnitude, Time.deltaTime * 20);
    
        anim.SetFloat("speed", speed, dampTime, Time.deltaTime);
        
    }
    
    /*  causes the player to toggle their glider */
    public override void Jump(bool hold)
    {
        base.Jump(hold);
        if (grounded)
        {
            if (racer is NPC || Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.2f, 0)))
            {
                BeginGliding();
                StartCoroutine(JumpAtNextFrame());
                StartCoroutine(PreventFalseLanding(racer is NPC ? 0.5f : 0.2f));
            }
        }
    }

    private void BeginGliding()
    {
        anim.ResetTrigger("land");
        lastVelocity = new Vector3(0,0,0);
        grounded = false;
        gliding = true;
        rb.drag = 0.01f;
        anim.SetTrigger("jump");
    }

    // Need to wait for next frame here because otherwise the NPC may attempt to jump
    // before the navmesh agent is disabled, which will not work.
    private IEnumerator JumpAtNextFrame()
    {
        yield return null;
        rb.AddForce(Vector3.up * jumpForce + characterMesh.transform.forward * jumpForce);
    }

    // Handle the gliding in the physics loop
    void FixedUpdate()
    {
        if (gliding)
        {
            // lift = local forward speed * cos(angle off of the xy plane) * lift factor * cos(roll angle)
            Vector3 lift = Vector3.Project(rb.velocity, characterMesh.forward) * Mathf.Cos(Vector3.Angle(characterMesh.forward, Vector3.forward)) * liftFactor * Mathf.Cos(Vector3.Angle(characterMesh.right, Vector3.right));
            Vector3 verticalDragAmount = - Vector3.Project(rb.velocity, characterMesh.up) * dragFactor;
            if (verticalDragAmount.y < 0)
            {
                verticalDragAmount *= 2;
            }

            // apply lift and vertical drag
            rb.AddForce(Time.fixedDeltaTime * lift.magnitude * characterMesh.up + verticalDragAmount * Time.fixedDeltaTime);
            
        }
        else if (!(racer is NPC) && !Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.5f, 0)))
        {
            BeginGliding();
            landable = true;
        }
    }

    // Without this, when the racer first takes off, the raycast in AddMovement that
    // determines if they're on the ground returns true, causing the player to do the
    // landing animation while in the air.
    // This coroutine prevents that by setting a bool (landable) after a short time of being airborn
    // that allows the racer to land
    private IEnumerator PreventFalseLanding(float waitTime)
    {
        yield return new WaitForEndOfFrame();

        // prevent other countdowns from being started
        if (countingDownLandable)
        {
            yield break;
        }
        countingDownLandable = true;
        yield return new WaitForSeconds(waitTime);
        landable = true;
        countingDownLandable = false;
    }

    /*  grounds the player after a jump is complete */
    public override void Land()
    {
        grounded = true;
        landable = false;
        gliding = false;
        rb.drag = 0;
        anim.SetTrigger("land");
        if (racer is NPC)
        {
            ((NPC)racer).Land();
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        else
        {
            cameraController.ResetXMinMax();
        }
    }
}
