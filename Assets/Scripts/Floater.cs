using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Floater : MonoBehaviour
{
    public bool bob = true;
    public float offset = 0f;

    //private bool underwater = false;
    private bool inWater = false;

    private float waterHeight;
    private float prevDrag;
    private float prevAngularDrag;

    private const float waterDrag = 10f;
    private const float airDrag = 1f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {  
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inWater)
        {
            Vector3 gravity = Physics.gravity;
            if (waterHeight > transform.position.y + offset)
            {
                rb.drag = waterDrag;
                gravity = -0.5f * Physics.gravity;
            }
            rb.AddForce(gravity * Mathf.Clamp(Mathf.Abs(waterHeight - (transform.position.y + offset)), 0, 1));
        }
        //float y = Mathf.SmoothStep(transform.position.)
        //transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void EnterWater(float height)
    {
        Debug.Log("enter");
        inWater = true;
        rb.useGravity = false;
        rb.angularDrag = 10f;
        waterHeight = height;

        prevDrag = rb.drag;
        prevAngularDrag = rb.angularDrag;

        Racer mov = GetComponent<Racer>();
        if (mov != null)
        {
            mov.SetMovementMode(Movement.Mode.Swimming);
        }

    }

    public void ExitWater()
    {
        Debug.Log("exit");

        inWater = false;
        rb.useGravity = true;
        rb.drag = prevDrag;
        rb.angularDrag = prevAngularDrag;


        Racer mov = GetComponent<Racer>();
        if (mov != null)
        {
            mov.SetMovementMode(Movement.Mode.Running);
        }
    }
}
