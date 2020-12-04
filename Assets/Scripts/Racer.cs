using UnityEngine;
using System.Collections;

public class Racer : MonoBehaviour
{
    public Movement.Mode movementMode;
    public Movement[] movementOptions;

    public Transform characterMesh;
    public Transform hips;
    protected AnimatorOverrideController animOverride;
    protected PlayerAnimationEvents animEvents;

    protected Movement movement;
    protected Ragdoll ragdoll;
    protected Rigidbody rb;
    protected Animator anim;
    protected AudioSource audioSource;
    
    protected Vector2 move;
    protected Vector3 velocityBeforePhysicsUpdate;
    protected bool dead;
    protected bool canRevive; // when this is true, a dead racer can be revived.

    public int place;
    public float dieThreshold = 40f;
    public Checkpoint lastCheckpoint;
    public Checkpoint nextCheckpoint;


    protected virtual void Start() 
    {
        rb = GetComponent<Rigidbody>();
        ragdoll = characterMesh.GetComponent<Ragdoll>();
        ragdoll.SetRagdoll(false);
        anim = characterMesh.GetComponent<Animator>();
        animEvents = characterMesh.GetComponent<PlayerAnimationEvents>();
        //animOverride = GetComponent<AnimatorOverrideController>();
        audioSource = characterMesh.GetComponent<AudioSource>();
        SetMovementMode(movementMode, true);
        
    } 

    protected virtual void Update()
    {
        if (!dead)
            movement.AddMovement(move.x, move.y);
    }

    private void FixedUpdate() {
        velocityBeforePhysicsUpdate = rb.velocity;
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    public virtual void SetMovementMode(Movement.Mode mode, bool initial = false)
    {
        if (initial || mode != movementMode)
        { 
            movementMode = mode;
            if (movement != null)
                movement.enabled = false;
            switch (mode)
            {
                // case MovementMode.Walking:
                //     break;
                case Movement.Mode.Running:
                    movement = movementOptions[(int)Movement.Mode.Running];
                    break;
                case Movement.Mode.Jetpacking:
                    movement = movementOptions[(int)Movement.Mode.Jetpacking];
                    break;
                case Movement.Mode.Swimming:
                    movement = movementOptions[(int)Movement.Mode.Swimming];
                    break;
                case Movement.Mode.Biking:
                    movement = movementOptions[(int)Movement.Mode.Biking];
                    break;
            }
            movement.enabled = true;
            animEvents.movement = movement;
            anim.SetInteger("movement_mode", (int)movementMode);
        }
    }

    public virtual void Die()
    {
        anim.enabled = false;
        Vector3 momentum = Vector3.ClampMagnitude(velocityBeforePhysicsUpdate, 30);
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        ragdoll.SetRagdoll(true);
        ragdoll.AddMomentum(momentum);
        dead = true;
        canRevive = false;
        try
        {
            // Deactivate jetpack particles if we're jetpacking
            Jetpack jetpack = (Jetpack)movement;
            jetpack.SetParticles(false);
        }
        catch (System.Exception)
        {
            
        }
        StartCoroutine(RevivalEnabler());
    }

    protected virtual IEnumerator RevivalEnabler()
    {
        // Don't allow a revival until we stop moving on the ground
        yield return new WaitUntil(() => !ragdoll.IsMoving());
        canRevive = true;
    }

    public virtual void Revive()
    {
        if (dead && canRevive)
        {
            ragdoll.SetRagdoll(false);
            anim.enabled = true;
            rb.isKinematic = false;
            GetComponent<Collider>().enabled = true;
            transform.position = hips.position;
            hips.localPosition = Vector3.zero;
            dead = false;
        }
    }

    public void ArriveAtCheckpoint(Checkpoint checkpoint)
    {
        lastCheckpoint = checkpoint;
        if (checkpoint.next != null)
            nextCheckpoint = checkpoint.next;
    }

    /*  plays a miscellaneus animation that is NOT defined in the animation controller */
    public void PlayMiscAnimation(AnimationClip clip)
    {
        animOverride["miscAnimation"] = clip;
        anim.runtimeAnimatorController = animOverride;
        anim.SetTrigger("misc");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Pick up an item
        if (other.gameObject.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            item.Pickup(this);
        }
    }

    /*  check if we hit something too fast */
    private void OnCollisionEnter(Collision other)
    {
        float mag;
        if (other.rigidbody != null)    // if the other thing is moving
        {
          mag = (velocityBeforePhysicsUpdate - other.rigidbody.velocity).magnitude;
        }
        else    // if the other thing is stationary
        {
            mag = (velocityBeforePhysicsUpdate).magnitude;
        }
        if (mag > dieThreshold)
        {
            Debug.Log(gameObject.name + " hit " + other.gameObject.name + " at " + mag + " m/s and died");
            Die();
        }
    }

    // Returns whether or not this racer is currently "dead"
    public bool IsDead()
    {
        return dead;
    }
}