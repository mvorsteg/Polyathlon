using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : Racer
{
    public CameraController cameraController;
    public float gamepadLookSensititvity = 20;
    public float mouseLookSensisitivity = 15;
    public float arrowKeysLookSensitivity = 20;
    private float keyboardSchemeSensitivity;
    private Vector2 look;

    private InputActions inputActions;
    private PlayerInput playerInput;
    private UI ui;
    private bool canMove;
    private bool canLook = true;
    private int playerNumber = -1;


    /* -------------- INPUT EVENTS ---------------- */

    // Running
    public void OnRunningMovement(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            if (ctx.performed)
                move = ctx.ReadValue<Vector2>();
            else if (ctx.canceled)
                move = Vector2.zero;
        }
    }

    public void OnRunningLook(InputAction.CallbackContext ctx)
    {
        if (canLook)
        {
            if (ctx.performed)
                look = ctx.ReadValue<Vector2>() * (ctx.control.device is Gamepad ? Time.deltaTime * gamepadLookSensititvity : keyboardSchemeSensitivity);
            else if (ctx.canceled)
                look = Vector2.zero;
        }
    }

    public void OnRunningJump(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            if (ctx.performed)
                movement.Jump(true);
            else if (ctx.canceled)
                movement.Jump(false);
        }
    }

    public void OnUseItem(InputAction.CallbackContext ctx)
    {
        if (RaceManager.IsTrainingCourse)
        {
            RaceManager.RespawnPlayer(this);
        }
        else if (!dead && canMove && item != null)
        {
            if (ctx.performed)
            {
                item.Use(this);
            }
        }
    }

    // Swimming
    public void OnSwimmingMovement(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            if (ctx.performed)
                move = ctx.ReadValue<Vector2>();
            else if (ctx.canceled)
                move = Vector2.zero;
        }
    }

    public void OnSwimmingLook(InputAction.CallbackContext ctx)
    {
        if (canLook)
        {
            if (ctx.performed)
                look = ctx.ReadValue<Vector2>() * (ctx.control.device is Gamepad ? Time.deltaTime * gamepadLookSensititvity : keyboardSchemeSensitivity);
            else if (ctx.canceled)
                look = Vector2.zero;
        }
    }

    // Biking
    public void OnBikingMovement(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            if (ctx.performed)
                move = ctx.ReadValue<Vector2>();
            else if (ctx.canceled)
                move = Vector2.zero;
        }
    }

    public void OnBikingLook(InputAction.CallbackContext ctx)
    {
        if (canLook)
        {
            if (ctx.performed)
                look = ctx.ReadValue<Vector2>() * (ctx.control.device is Gamepad ? Time.deltaTime * gamepadLookSensititvity : keyboardSchemeSensitivity);
            else if (ctx.canceled)
                look = Vector2.zero;
        }
    }

    // Jetpacking
    public void OnJetpackingMovement(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            if (ctx.performed)
                move = ctx.ReadValue<Vector2>();
            else if (ctx.canceled)
                move = Vector2.zero;
        }
    }

    public void OnJetpackingLook(InputAction.CallbackContext ctx)
    {
        if (canLook)
        {
            if (ctx.performed)
                look = ctx.ReadValue<Vector2>() * (ctx.control.device is Gamepad ? Time.deltaTime * gamepadLookSensititvity : keyboardSchemeSensitivity);
            else if (ctx.canceled)
                look = Vector2.zero;
        }
    }

    public void OnJetpackingJump(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            if (ctx.performed)
                movement.Jump(true);
            else if (ctx.canceled)
                movement.Jump(false);
        }
    }

    // Gliding
    public void OnGlidingMovement(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            if (ctx.performed)
                move = ctx.ReadValue<Vector2>();
            else if (ctx.canceled)
                move = Vector2.zero;
        }
    }

    public void OnGlidingLook(InputAction.CallbackContext ctx)
    {
        if (canLook)
        {
            if (ctx.performed)
                look = ctx.ReadValue<Vector2>() * (ctx.control.device is Gamepad ? Time.deltaTime * gamepadLookSensititvity : keyboardSchemeSensitivity);
            else if (ctx.canceled)
                look = Vector2.zero;
        }
    }

    public void OnGlidingJump(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            if (ctx.performed)
                movement.Jump(true);
            else if (ctx.canceled)
                movement.Jump(false);
        }
    }
    
    public void OnFinishAnyKeyPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnFinishAnyKeyPressed now!");
        RaceManager.ReturnToMenu();
    }

    // This is only for exiting training while using a gamepad
    public void OnStartButton(InputAction.CallbackContext ctx)
    {
        if (RaceManager.IsTrainingCourse)
            RaceManager.ReturnToMenu();
    }


    private void Awake() 
    {
        playerInput = GetComponent<PlayerInput>();
        //inputActions = new InputActions();
        //inputActions.Debug.Exit.performed += ctx => Application.Quit();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        keyboardSchemeSensitivity = mouseLookSensisitivity;
    }

    protected override void Start() 
    {
       // inputActions.Disable();
        canMove = false;
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
        // when we press an arrow key, change the sensitivity to expect looking with arrow keys
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            keyboardSchemeSensitivity = arrowKeysLookSensitivity;
        }
        // when we click the mouse, change the sensitivity to expect looking with mouse movement
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            keyboardSchemeSensitivity = mouseLookSensisitivity;
        }
        base.Update();
        movement.RotateCamera(look.x, look.y);
    }

    public void SetPlayerNumber(int newNum)
    {
        playerNumber = newNum;
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    protected override void ReviveText()
    {
        if (!isFinished)
        {
            ui.ReviveText(true);
        }
    }

    public override void EquipItem(Item item)
    {
        base.EquipItem(item);
        ui.SetItemImage(item != null ? item.icon : null);
    }

    /*  called when the race officially starts after the countdown 
        enables movement */
    public override void StartRace()
    {
        canMove = true;
        movement.enabled = true;
    }

    /*  called when the racer crosses the finish line 
        disables movement controls, displays text */
    public override void FinishRace(bool force)
    {
        base.FinishRace(force);
        // disable all movement controls
        canMove = false;
        move = Vector2.zero;
        movement.Jump(false);
        // display text
        StartCoroutine(ui.FinishRace());
    }

    public override void RaceIsOver()
    {
        // Enable FinishLine action map
        playerInput.currentActionMap = playerInput.actions.actionMaps[8];
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    public override void SetMovementMode(Movement.Mode mode, bool initial = false)
    {
        Vector2 movePreserve = move;
        base.SetMovementMode(mode, initial);
        playerInput.currentActionMap = playerInput.actions.actionMaps[(int)mode];
        move = movePreserve;
    }

    public override void Die(bool emphasizeTorso, Vector3 newMomentum = default(Vector3))
    {
        base.Die(emphasizeTorso, newMomentum);
        Debug.Log("die");
        // Have the camera start following the ragdoll
        StartCoroutine(cameraController.FollowRagdoll());
    }

    public override void Revive(bool forceRevive = false)
    {
        if (dead && (canRevive || forceRevive))
        {
            // have the camera stop following the ragdoll
            StartCoroutine(cameraController.FollowRagdoll());
        }
        ui.ReviveText(false);
        base.Revive(forceRevive);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 8) // ignore collisions with projectiles
        {
            base.OnCollisionEnter(other);
            float mag;
            if (other.rigidbody != null)    // if the other thing is moving
            {
                mag = (velocityBeforePhysicsUpdate - other.rigidbody.linearVelocity).magnitude;
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