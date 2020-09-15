using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    /*  enumeration of the possible ways the player can be moving */
    enum MovementMode
    {
        Walking,
        Running,
        Sprinting,
        Crouching,
        Cover,
        CoverCrouching,
        Swimming
    }

    #pragma warning disable 0649
        private Animator anim;
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip landSound;
        [SerializeField] private Transform characterMesh;
    #pragma warning restore 0649

    private AnimatorOverrideController animOverride;

    private Vector3 velocity = Vector3.zero;
    private Vector3 actualVelocity; // accounts for walking into walls
    private Vector3 playerPosition; // position in previous frame

    [SerializeField]private MovementMode movementMode;

    public CameraController cameraController;
    
    private AudioSource audioSource;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private Vector2 movement;
    private Vector2 look;

    // enables if player has control of movement
    private bool hasControl;

    // used to smooth out speed transition an animation
    private float speed = 0;
    private float maxSpeed;
    private float smoothSpeed = 0;

    private float bonusSpeed = 1f;

    // used to determine when jumping can occur
    private bool grounded = true;
    private bool falling = false;

    private bool orientRotation = true;

    // constants consist primarily of values for movement speeds
    private const float rotationSpeed = 10f;
    private const float walkSpeed = 2f;
    private const float runSpeed = 5f;
    private const float sprintSpeed = 7f;
    private const float crouchSpeed = 2f;
    private const float coverSpeed = 1.7f;
    private const float coverCrouchSpeed = 1.5f;
    private const float swimSpeed = 8f;
    private const float aimSpeed = 1.2f;
    private const float jumpForce = 300f;

    private const float dampTime = 0.05f; // reduce jittering in animator by providing dampening

    public Vector2 Movement { get => movement; set => movement = value; }
    public Vector2 Look { get => look; set => look = value; }
    public Vector3 Velocity { get => actualVelocity; set => velocity = value; }
    public float Direction { get => actualVelocity == Vector3.zero ? 0f : Mathf.Abs(Quaternion.LookRotation(actualVelocity, Vector3.up).eulerAngles.y - characterMesh.transform.rotation.eulerAngles.y); }
    public float BonusSpeed { get => bonusSpeed; set => bonusSpeed = value; }
    public bool Falling { get => falling; set => falling = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        characterMesh = transform.GetChild(0);

        playerPosition = transform.position;

        animOverride = new AnimatorOverrideController();
        //animOverride.runtimeAnimatorController = anim.runtimeAnimatorController;

        SetMovementMode(movementMode);//PlayerMovement.MovementMode.Running);
    }

    void Update()
    {
        if (true)//PlayerUI.IsUnpaused())
        {
            actualVelocity = Vector3.Lerp(actualVelocity, (transform.position - playerPosition) / Time.deltaTime, Time.deltaTime * 10);
            playerPosition = transform.position;
            cameraController.Rotate(look.x, look.y);
            
            // move the player every frame. if there was any movement, rotate the model to face current direction and update velocity
            AddMovement(movement.x, movement.y);
            if (velocity.magnitude > 0)
            {
                rb.velocity = new Vector3(velocity.normalized.x * smoothSpeed, rb.velocity.y, velocity.normalized.z * smoothSpeed);
                smoothSpeed = Mathf.Lerp(smoothSpeed, maxSpeed * bonusSpeed, Time.deltaTime);
                // rotate the character mesh if enabled
                if (orientRotation)
                {
                    characterMesh.rotation = Quaternion.Lerp(characterMesh.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotationSpeed);
                }
            }
            else
            {
                smoothSpeed = Mathf.Lerp(smoothSpeed, 0, Time.deltaTime*8);
            }
            // if the player landed, enable another jump
            if (!grounded)
            {
                RaycastHit hit;
                if (falling && Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.2f, 0), out hit))
                {
                    falling = false;
                    Land();
                }
            }
            // blend speed in animator to match pace of footsteps
            // normal movement (character moves independent of camera)
            if (orientRotation)
            {
                speed = Mathf.SmoothStep(speed, actualVelocity.magnitude, Time.deltaTime * 20);
            }
            // movement when aiming (straft left/right/forward/backward)
            else
            {
                float speedY = Vector3.Dot(actualVelocity, anim.transform.right);
                anim.SetFloat("speedY", speedY, dampTime, Time.deltaTime);
                speed = Vector3.Dot(actualVelocity, anim.transform.forward);
            }
            anim.SetFloat("speed", speed, dampTime, Time.deltaTime);
        }
    }

    /*  plays a miscellaneus animation that is NOT defined in the animation controller */
    public void PlayMiscAnimation(AnimationClip clip)
    {
        animOverride["miscAnimation"] = clip;
        anim.runtimeAnimatorController = animOverride;
        anim.SetTrigger("misc");
    }

    /*  moves the player rigidbody */
    private void AddMovement(float forward, float right)
    {
        Vector3 translation = Vector3.zero;
        if (!orientRotation)
        {
            translation += right * characterMesh.transform.forward;;
            translation += forward * characterMesh.transform.right;
        }
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
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    private void SetMovementMode(PlayerMovement.MovementMode mode)
    {
        movementMode = mode;
        switch (mode)
        {
            case PlayerMovement.MovementMode.Walking:
                maxSpeed = walkSpeed;
                break;
            case PlayerMovement.MovementMode.Running:
                maxSpeed = runSpeed;
                break;
            case PlayerMovement.MovementMode.Sprinting:
                maxSpeed = sprintSpeed;
                break;
            case PlayerMovement.MovementMode.Crouching:
                maxSpeed = crouchSpeed;
                break;
            case PlayerMovement.MovementMode.Cover:
                maxSpeed = coverSpeed;
                break;
            case PlayerMovement.MovementMode.CoverCrouching:
                maxSpeed = coverCrouchSpeed;
                break;
            case PlayerMovement.MovementMode.Swimming:
                maxSpeed = swimSpeed;
                break;
        }
    }
    /*  causes the player to jump */
    public void Jump()
    {
        if (orientRotation && grounded)
        {
            if (Physics.Linecast(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(0, -0.1f, 0)))
            {
                rb.AddForce(Vector3.up * jumpForce);
                grounded = false;
                anim.SetTrigger("jump");
            }
        }
    }

    /*  grounds the player after a jump is complete */
    private void Land()
    {
        grounded = true;
        anim.SetTrigger("land");
    }

    /*  changes between walking and running movement modes */
    public void ToggleRun()
    {
        if (movementMode == PlayerMovement.MovementMode.Walking)
        {
            SetMovementMode(PlayerMovement.MovementMode.Running);
        }
        else if (movementMode == PlayerMovement.MovementMode.Running)
        {
            SetMovementMode(PlayerMovement.MovementMode.Walking);
        }
    }

    /*  changes between crouching and non-crouching movement */
    public void ToggleSprint()
    {
        if (movementMode == PlayerMovement.MovementMode.Sprinting)
        {
            SetMovementMode(PlayerMovement.MovementMode.Running);
        }
        else if (movementMode != PlayerMovement.MovementMode.Swimming)
        {
            SetMovementMode(PlayerMovement.MovementMode.Sprinting);
        }
    }
}
