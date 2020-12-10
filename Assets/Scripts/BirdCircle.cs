using UnityEngine;

public class BirdCircle : MonoBehaviour
{
    private Bird bird;
    private float circleSpeed;  // rotations per second

    private void Start()
    {
        bird = transform.GetChild(0).GetComponent<Bird>();
        // calculate angular speed required to make bird go at its speed   
        circleSpeed = (180f / Mathf.PI) * (bird.speed / (GetComponent<SphereCollider>().radius));
    }

    private void Update()
    {
        if (bird.circle)
            transform.Rotate(0f, -1 * circleSpeed * Time.deltaTime, 0f);    
    }

    private void OnTriggerEnter(Collider other)
    {
        Racer racer = other.transform.GetComponent<Racer>();
        if (racer != null)
        {
            bird.SetTarget(racer);
        }   
    }

    private void OnDrawGizmos()
    {    
        float r = GetComponent<SphereCollider>().radius;
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        Gizmos.DrawSphere(transform.position, r);    
    }
}