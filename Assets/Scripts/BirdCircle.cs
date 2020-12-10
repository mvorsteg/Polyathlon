using UnityEngine;

public class BirdCircle : MonoBehaviour
{
    private Bird bird;
    public float circleSpeed = 1f;

    private void Start()
    {
        bird = transform.GetChild(0).GetComponent<Bird>();    
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
}