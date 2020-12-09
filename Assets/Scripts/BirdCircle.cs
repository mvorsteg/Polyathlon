using UnityEngine;

public class BirdCircle : MonoBehaviour
{
    public Bird bird;
    public float circleSpeed = 1f;

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