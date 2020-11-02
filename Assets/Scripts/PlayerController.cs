using UnityEngine;

public class PlayerController : MonoBehaviour
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

    private InputActions inputActions;

    private void Awake() 
    {
        inputActions = new InputActions();

        // running
        inputActions.Running.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        inputActions.Running.Movement.canceled += ctx => move = Vector2.zero;

        inputActions.Running.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        inputActions.Running.Look.canceled += ctx => look = Vector2.zero;

        inputActions.Running.Jump.performed += ctx => movement.Jump();

        // swimming
        inputActions.Swimming.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        inputActions.Swimming.Movement.canceled += ctx => move = Vector2.zero;

        inputActions.Swimming.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        inputActions.Swimming.Look.canceled += ctx => look = Vector2.zero;

        // biking
        inputActions.Biking.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        inputActions.Biking.Movement.canceled += ctx => move = Vector2.zero;

        inputActions.Biking.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        inputActions.Biking.Look.canceled += ctx => look = Vector2.zero;

        //  Debug actions
        inputActions.Debug.SlowTime.performed += ctx => SlowTime();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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
        inputActions.Running.Disable();
        inputActions.Swimming.Disable();
        inputActions.Biking.Disable();
        switch (mode)
        {
            // case MovementMode.Walking:
            //     break;
            case Movement.Mode.Running:
                movement = movementOptions[(int)Movement.Mode.Running];
                inputActions.Running.Enable();
                break;
            case Movement.Mode.Swimming:
                movement = movementOptions[(int)Movement.Mode.Swimming];
                inputActions.Swimming.Enable();
                break;
            case Movement.Mode.Biking:
                movement = movementOptions[(int)Movement.Mode.Biking];
                inputActions.Biking.Enable();
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

    /*  debug method
        toggles time between 20% and 100% */
    private void SlowTime()
    {
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.2f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}