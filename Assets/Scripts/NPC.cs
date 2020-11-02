using UnityEngine;

public class NPC : MonoBehaviour
{

    public Movement.Mode movementMode;
    public Movement[] movementOptions;

    public CameraController cameraController;
    public PlayerAnimationEvents animEvents;

    public Transform characterMesh;
    public AnimatorOverrideController animOverride;

    private Movement movement;
    private Animator anim;
    private AudioSource audioSource;
    
    private Vector2 move;
    private Vector2 look;

    private void Start() 
    {
        anim = characterMesh.GetComponent<Animator>();
        //animOverride = GetComponent<AnimatorOverrideController>();
        audioSource = characterMesh.GetComponent<AudioSource>();

        foreach(Movement m in movementOptions)
        {
            m.enabled = false;
            m.CameraController = cameraController;
        }
        SetMovementMode(movementMode);
    }

    private void Update()
    {
        movement.AddMovement(move.x, move.y);
        movement.RotateCamera(look.x, look.y);
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    public void SetMovementMode(Movement.Mode mode)
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
        animEvents.movement = movement;
        anim.SetInteger("movement_mode", (int)movementMode);
    }

    /*  plays a miscellaneus animation that is NOT defined in the animation controller */
    public void PlayMiscAnimation(AnimationClip clip)
    {
        animOverride["miscAnimation"] = clip;
        anim.runtimeAnimatorController = animOverride;
        anim.SetTrigger("misc");
    }
}