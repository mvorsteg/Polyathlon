using UnityEngine;

public abstract class Movement : MonoBehaviour
{

    /*  enumeration of the possible ways the player can be moving */
    public enum Mode
    {
        //Walking,
        Running,
        Jetpacking,
        Swimming,
        Biking,
        GetOffTheBoat,
    }

    // constants consist primarily of values for movement speeds
    protected const float rotationSpeed = 10f;
    protected const float walkSpeed = 2f;
    protected const float runSpeed = 5f;
    protected const float sprintSpeed = 7f;
    protected const float jetpackSpeed = 20f;
    protected const float swimSpeed = 3f;
    protected const float bikeSpeed = 10f;
    protected const float jumpForce = 300f;
    protected const float jetpackForce = 1500f;

    protected const float runAcceleration = 2.5f;
    protected const float swimAcceleration = 5f;
    protected const float bikeAcceleration = 1.2f;

    public float maxSpeed;
    public float acceleration;
    public float angularSpeed;

    protected CameraController cameraController;

    protected Transform characterMesh;

    protected Animator anim;
    protected Rigidbody rb;
    protected Racer racer;

    protected Vector3 velocity = Vector3.zero;
    protected Vector3 actualVelocity; // accounts for walking into walls
    protected Vector3 playerPosition; // position in previous frame

    // used to smooth out speed transition an animation
    protected float speed = 0;
    protected float smoothSpeed = 0;
    protected const float dampTime = 0.05f; // reduce jittering in animator by providing dampening

    protected float bonusSpeed = 1f;

    // keep track of these because some movement modes will change these
    private float defaultMass;
    private float defaultDrag;
    private RigidbodyConstraints defaultConstraints;
    private float defaultAngularDrag;
    private Vector3 defaultCenterOfMass;
    private Vector3 defaultCharacterMeshPos;
    private Vector3 defaultCharacterMeshRot;


    // used to determine when jumping can occur
    protected bool grounded = true;
    protected bool falling = false;
    
    public Vector3 Velocity { get => actualVelocity; set => velocity = value; }
    public bool Falling { get => falling; set => falling = value; } 
    public bool Grounded { get => grounded; set => grounded = value; }
    public float BonusSpeed { get => bonusSpeed; set => bonusSpeed = value; }
    public CameraController CameraController { get => cameraController; set => cameraController = value; }

    protected virtual void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        defaultMass = rb.mass;
        defaultDrag = rb.drag;
        defaultAngularDrag = rb.angularDrag;
        defaultConstraints = rb.constraints;
        defaultCenterOfMass = rb.centerOfMass;
        racer = GetComponent<Racer>();
        characterMesh = transform.GetChild(0);
        defaultCharacterMeshPos = characterMesh.localPosition;
        defaultCharacterMeshRot = characterMesh.localEulerAngles;
        anim = characterMesh.GetComponent<Animator>();
        playerPosition = transform.position;
    }

    protected virtual void OnDisable()
    {
        // Reset these to what they should be by default
        rb.mass = defaultMass;
        rb.drag = defaultDrag;
        rb.angularDrag = defaultAngularDrag;
        rb.constraints = defaultConstraints;
        rb.centerOfMass = defaultCenterOfMass;
        characterMesh.localPosition = defaultCharacterMeshPos;
        characterMesh.localEulerAngles = defaultCharacterMeshRot;
        if (cameraController != null)
        {
            cameraController.ResetXMinMax();
        }
    }

    /*  rotate the camera around the player */
    public void RotateCamera(float x, float y)
    {
        cameraController.Rotate(x, y);
    }

    /*  moves the player rigidbody */
    public virtual void AddMovement(float forward, float right)
    {
        actualVelocity = Vector3.Lerp(actualVelocity, (transform.position - playerPosition) / Time.deltaTime, Time.deltaTime * 10);
        playerPosition = transform.position;
    }

    /*  causes the player to jump */
    public virtual void Jump(bool hold)
    {
        racer.Revive();
    }

    /*  grounds the player after a jump is complete */
    public virtual void Land()
    {

    }


    protected virtual void LateUpdate()
    {
        //Debug.Log(falling);
        // Prevent short jumps or jetpack flights from happening twice due to a lingering trigger
        anim.ResetTrigger("jump");
    }
}