using UnityEngine;

public abstract class Movement : MonoBehaviour
{

    /*  enumeration of the possible ways the player can be moving */
    public enum Mode
    {
        //Walking,
        Running,
        Swimming,
        Biking,
    }

    // constants consist primarily of values for movement speeds
    protected const float rotationSpeed = 10f;
    protected const float walkSpeed = 2f;
    protected const float runSpeed = 5f;
    protected const float sprintSpeed = 7f;
    protected const float swimSpeed = 3f;
    protected const float bikeSpeed = 10f;
    protected const float jumpForce = 300f;

    protected float maxSpeed;

    protected CameraController cameraController;

    protected Transform characterMesh;

    protected Animator anim;
    protected Rigidbody rb;

    protected Vector3 velocity = Vector3.zero;
    protected Vector3 actualVelocity; // accounts for walking into walls
    protected Vector3 playerPosition; // position in previous frame

    // used to determine when jumping can occur
    protected bool grounded = true;
    protected bool falling = false;

    protected bool orientRotation = true;
    
    public Vector3 Velocity { get => actualVelocity; set => velocity = value; }
    public bool Falling { get => falling; set => falling = value; }
    public CameraController CameraController { get => cameraController; set => cameraController = value; }

    protected virtual void Start() 
    {
        rb = GetComponent<Rigidbody>();
        characterMesh = transform.GetChild(0);
        anim = characterMesh.GetComponent<Animator>();
        playerPosition = transform.position;
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
    public virtual void Jump()
    {

    }

    /*  grounds the player after a jump is complete */
    protected virtual void Land()
    {

    }

}