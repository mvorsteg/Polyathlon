using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed;

    public bool circle = true;

    private Racer target;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (target == null)
        {
            // do flying stuff idk yet
        }    
        else
        {
            // chase target
            transform.LookAt(target.transform);
            rb.velocity = transform.forward * speed;
        }
    }

    public void SetTarget(Racer racer)
    {
        if (target == null)
        {
            target = racer;
            circle = false;
        }
    }
}