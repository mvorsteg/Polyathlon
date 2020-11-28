using UnityEngine;

public class PlayerController : Racer
{
    public CameraController cameraController;

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

        inputActions.Running.Jump.performed += ctx => movement.Jump(true);
        inputActions.Running.Jump.canceled += ctx => movement.Jump(false);

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

        // jetpacking
        inputActions.Jetpacking.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        inputActions.Jetpacking.Movement.canceled += ctx => move = Vector2.zero;

        inputActions.Jetpacking.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        inputActions.Jetpacking.Look.canceled += ctx => look = Vector2.zero;

        inputActions.Jetpacking.Jump.performed += ctx => movement.Jump(true);
        inputActions.Jetpacking.Jump.canceled += ctx => movement.Jump(false);

        //  Debug actions
        inputActions.Debug.SlowTime.performed += ctx => SlowTime();
        //inputActions.Debug.Die.performed += ctx => Revive();
        inputActions.Debug.Enable();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void Start() 
    {
        foreach(Movement m in movementOptions)
        {
            m.enabled = false;
            m.CameraController = cameraController;
        }
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        movement.RotateCamera(look.x, look.y);
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    public override void SetMovementMode(Movement.Mode mode)
    {
        base.SetMovementMode(mode);
        inputActions.Running.Disable();
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
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("die");
        
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