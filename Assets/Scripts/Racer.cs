using UnityEngine;

public class Racer : MonoBehaviour
{
    public Movement.Mode movementMode;
    public Movement[] movementOptions;

    public Transform characterMesh;
    protected AnimatorOverrideController animOverride;

    protected Movement movement;
    protected Ragdoll ragdoll;
    protected Rigidbody rb;
    protected Animator anim;
    protected AudioSource audioSource;
    
    protected Vector2 move;

    public int place;
    public Checkpoint lastCheckpoint;
    public Checkpoint nextCheckpoint;

    protected virtual void Start() 
    {
        rb = GetComponent<Rigidbody>();
        ragdoll = characterMesh.GetComponent<Ragdoll>();
        ragdoll.SetRagdoll(false);
        anim = characterMesh.GetComponent<Animator>();
        //animOverride = GetComponent<AnimatorOverrideController>();
        audioSource = characterMesh.GetComponent<AudioSource>();
        SetMovementMode(movementMode);
    } 

    protected virtual void Update()
    {
        movement.AddMovement(move.x, move.y);
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    public virtual void SetMovementMode(Movement.Mode mode)
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
            case Movement.Mode.Swimming:
                movement = movementOptions[(int)Movement.Mode.Swimming];
                break;
            case Movement.Mode.Biking:
                movement = movementOptions[(int)Movement.Mode.Biking];
                break;
        }
        movement.enabled = true;
        anim.SetInteger("movement_mode", (int)movementMode);
    }

    public virtual void Die()
    {
        anim.enabled = false;
        Vector3 momentum = rb.velocity;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        ragdoll.SetRagdoll(true);
        ragdoll.AddMomentum(momentum);
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
}