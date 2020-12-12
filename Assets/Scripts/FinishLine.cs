using UnityEngine;

public class FinishLine : MonoBehaviour 
{
    /*  when a racer crosses the finish line, we need to call their FinishRace method */
    private void OnTriggerEnter(Collider other)
    {
        Racer racer = other.transform.GetComponent<Racer>();
        if (racer != null)
        {
            racer.FinishRace();
        }
    }
}