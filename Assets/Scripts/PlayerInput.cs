using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]

public class PlayerInput : MonoBehaviour
{

    //private Player player;
    private InputActions inputActions;
    private PlayerMovement playerMovement;
    //public PlayerUI playerUI;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        //player = GetComponent<Player>();
    }

    private void Awake()
    {
        inputActions = new InputActions();

        inputActions.Running.Movement.performed += ctx => playerMovement.Movement = ctx.ReadValue<Vector2>();
        inputActions.Running.Movement.canceled += ctx => playerMovement.Movement = Vector2.zero;

        inputActions.Running.Look.performed += ctx => playerMovement.Look = ctx.ReadValue<Vector2>();
        inputActions.Running.Look.canceled += ctx => playerMovement.Look = Vector2.zero;

        inputActions.Running.Jump.performed += ctx => playerMovement.Jump();

        //  Debug actions
        inputActions.Debug.SlowTime.performed += ctx => SlowTime();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputActions.Running.Enable();
        inputActions.Debug.Enable();    // THIS LINE MUST BE REMOVED FOR RELEASE
        //inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.Running.Disable();
        //inputActions.UI.Disable();
    }

    // public void TogglePause()
    // {
    //     
    // }

    /*  debug method */
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
