using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Racer
{
    public CameraController cameraController;
    public float gamepadLookSensititvity = 20;
    public float mouseLookSensisitivity = 15;
    private Vector2 look;

    private InputActions inputActions;
    private UI ui;

    private void Awake() 
    {
        inputActions = new InputActions();

        // running
        inputActions.Running.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        inputActions.Running.Movement.canceled += ctx => move = Vector2.zero;

        inputActions.Running.Look.performed += ctx => look = ctx.ReadValue<Vector2>() * Time.deltaTime * (ctx.control.device is Gamepad ?
                                                                                            gamepadLookSensititvity : mouseLookSensisitivity);
        inputActions.Running.Look.canceled += ctx => look = Vector2.zero;

        inputActions.Running.Jump.performed += ctx => movement.Jump(true);
        inputActions.Running.Jump.canceled += ctx => movement.Jump(false);

        // swimming
        inputActions.Swimming.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        inputActions.Swimming.Movement.canceled += ctx => move = Vector2.zero;

        inputActions.Swimming.Look.performed += ctx => look = ctx.ReadValue<Vector2>() * Time.deltaTime * (ctx.control.device is Gamepad ?
                                                                                            gamepadLookSensititvity : mouseLookSensisitivity);
        inputActions.Swimming.Look.canceled += ctx => look = Vector2.zero;

        // biking
        inputActions.Biking.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        inputActions.Biking.Movement.canceled += ctx => move = Vector2.zero;

        inputActions.Biking.Look.performed += ctx => look = ctx.ReadValue<Vector2>() * Time.deltaTime * (ctx.control.device is Gamepad ?
                                                                                            gamepadLookSensititvity : mouseLookSensisitivity);
        inputActions.Biking.Look.canceled += ctx => look = Vector2.zero;

        // jetpacking
        inputActions.Jetpacking.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        inputActions.Jetpacking.Movement.canceled += ctx => move = Vector2.zero;

        inputActions.Jetpacking.Look.performed += ctx => look = ctx.ReadValue<Vector2>() * Time.deltaTime * (ctx.control.device is Gamepad ?
                                                                                            gamepadLookSensititvity : mouseLookSensisitivity);
        inputActions.Jetpacking.Look.canceled += ctx => look = Vector2.zero;

        inputActions.Jetpacking.Jump.performed += ctx => movement.Jump(true);
        inputActions.Jetpacking.Jump.canceled += ctx => movement.Jump(false);

        //  Debug actions
        inputActions.Debug.SlowTime.performed += ctx => SlowTime();
        inputActions.Debug.Exit.performed += ctx => Application.Quit();
        //inputActions.Debug.Die.performed += ctx => Revive();
        inputActions.Debug.Enable();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void Start() 
    {
        inputActions.Disable();
        foreach(Movement m in movementOptions)
        {
            m.enabled = false;
            m.CameraController = cameraController;
        }
        ui = transform.GetComponentInChildren<UI>();

        base.Start();
        movement.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
        movement.RotateCamera(look.x, look.y);
    }

    /*  called when the race officially starts after the countdown 
        enables movement */
    public override void StartRace()
    {
        movement.enabled = true;
    }

    /*  called when the racer crosses the finish line 
        disables movement controls, displays text */
    public override void FinishRace()
    {
        base.FinishRace();
        // disable all movement controls
        inputActions.Running.Disable();
        inputActions.Jetpacking.Disable();
        inputActions.Swimming.Disable();
        inputActions.Biking.Disable();
        // display text
        StartCoroutine(ui.FinishRace());
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    public override void SetMovementMode(Movement.Mode mode, bool initial = false)
    {
        Vector2 movePreserve = move;
        base.SetMovementMode(mode, initial);
        inputActions.Running.Disable();
        inputActions.Jetpacking.Disable();
        inputActions.Swimming.Disable();
        inputActions.Biking.Disable();
        switch (mode)
        {
            case Movement.Mode.Running:
                inputActions.Running.Enable();
                break;
            case Movement.Mode.Jetpacking:
                inputActions.Jetpacking.Enable();
                break;
            case Movement.Mode.Swimming:
                inputActions.Swimming.Enable();
                break;
            case Movement.Mode.Biking:
                inputActions.Biking.Enable();
                break;
        }
        move = movePreserve;
    }

    public override void Die(bool emphasizeTorso, Vector3 newMomentum = default(Vector3))
    {
        base.Die(emphasizeTorso, newMomentum);
        Debug.Log("die");
        // Have the camera start following the ragdoll
        StartCoroutine(cameraController.FollowRagdoll());
    }

    public override void Revive()
    {
        if (dead && canRevive)
        {
            // have the camera stop following the ragdoll
            StartCoroutine(cameraController.FollowRagdoll());
        }
        base.Revive();
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 8) // ignore collisions with projectiles
        {
            base.OnCollisionEnter(other);
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
                Die(false);
            }
        }
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