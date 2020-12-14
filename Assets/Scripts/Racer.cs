using UnityEngine;
using System.Collections;

public class Racer : MonoBehaviour
{
    public new string name;
    public Movement.Mode movementMode;
    public Movement[] movementOptions;

    public Transform characterMesh;
    public Transform hips;

    protected Item item;
    
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
    public bool isFinished = false;

    [Header("Sound Effects")]
    public AudioClip bikeSound;
    public AudioClip waterSound;
    public AudioClip equipSound;


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
        if (!dead && RaceManager.IsRaceActive)
            movement.AddMovement(move.x, move.y);
    }

    public virtual void StartRace()
    {

    }

    public virtual void FinishRace(bool forced)
    {
        isFinished = true;
        float extraTime = 0;
        if (forced)
        {
            extraTime = Vector3.Distance(transform.position, nextCheckpoint.transform.position);
            Checkpoint c = nextCheckpoint;
            while (c.next != null)
            {
                extraTime += c.distance;
                c = c.next;
            }
            extraTime /= movement.maxSpeed;
        }
        RaceManager.FinishRace(this, extraTime);
    }

    public virtual void RaceIsOver()
    {
        
    }

    private void FixedUpdate() {
        velocityBeforePhysicsUpdate = rb.velocity;
    }

    public Transform GetHips()
    {
        return hips;
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
                    audioSource.clip = equipSound;
                    audioSource.Play();
                    break;
                case Movement.Mode.Swimming:
                    movement = movementOptions[(int)Movement.Mode.Swimming];
                    audioSource.clip = waterSound;
                    audioSource.Play();
                    break;
                case Movement.Mode.Biking:
                    movement = movementOptions[(int)Movement.Mode.Biking];
                    audioSource.clip = bikeSound;
                    audioSource.Play();
                    break;
                case Movement.Mode.GetOffTheBoat:
                    movement = movementOptions[(int)Movement.Mode.Running];
                    break;
                
            }
            movement.enabled = true;
            animEvents.movement = movement;
            anim.SetInteger("movement_mode", (int)movementMode % 4);
        }
    }

    // If emphasizeTorso is true, then extra force will be added to the racer's hips
    // when they ragdoll, preventing them from simply retaining their animation pose
    // if hit by a laser midair
    // When newMomentum is 0,0,0, the momentum used will be simply the character's current momentum
    public virtual void Die(bool emphasizeTorso, Vector3 newMomentum = default(Vector3))
    {
        anim.enabled = false;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        ragdoll.SetRagdoll(true);
        Vector3 momentum;
        if (newMomentum == Vector3.zero)
        {
            //momentum = Vector3.ClampMagnitude(velocityBeforePhysicsUpdate, 30);
            momentum = velocityBeforePhysicsUpdate;
        }
        else
        {
            momentum = newMomentum;
        }
        ragdoll.AddMomentum(momentum, emphasizeTorso);
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

    public virtual void EquipItem(Item item)
    {
        this.item = item;
    }

    public void SpeedBoost()
    {
        StartCoroutine(SpeedBoost(2f, 5f));
        EquipItem(null);
    }

    private IEnumerator SpeedBoost(float magnitude, float duration)
    {
        movement.BonusSpeed = magnitude;
        anim.speed = magnitude;
        yield return new WaitForSeconds(duration);
        movement.BonusSpeed = 1f;
        anim.speed = 1f;
    }

    public void DropItem()
    {
        Vector3 pos = transform.position - characterMesh.transform.forward + 0.5f * characterMesh.transform.up;
        Instantiate(item.Child, pos, Quaternion.identity);
        EquipItem(null);
    }

    public void ThrowItem()
    {
        Vector3 pos = transform.position + 2f * characterMesh.transform.forward + 0.5f * characterMesh.transform.up;
        GameObject obj = Instantiate(item.Child, pos, Quaternion.identity);
        Rigidbody itemRb = obj.GetComponent<Rigidbody>();
        itemRb.AddForce(1000 * (characterMesh.transform.forward + 0.1f * transform.up));
        EquipItem(null);
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
    protected virtual void OnCollisionEnter(Collision other)
    {
        
    }

    // Returns whether or not this racer is currently "dead"
    public bool IsDead()
    {
        return dead;
    }
}