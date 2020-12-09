using UnityEngine;
using System.Collections;

public class Ragdoll : MonoBehaviour 
{
    //[SerializeField]
    private Rigidbody[] rigidbodies;
    //[SerializeField]
    private Collider[] colliders;

    /* Ocassionally someone will die and get their head caught on something
        making some weird collisions that give like 1 limb a tiny velocity
        forever. If rigidbodies still have velocity when maxDeadTime is exceeded,
        then IsMoving will return false so the game can continue. */
    private float maxDeadTime = 5;
    private float deadTime; // amount of time we've been dead for

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();    
    }

    /*  activates / deactivates the ragdoll's colliders and rigidbodies */
    public void SetRagdoll(bool value)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.collisionDetectionMode = value ? CollisionDetectionMode.Discrete : CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = !value;
        }

        foreach (Collider c in colliders)
        {
            c.enabled = value;
        }
        if (value)
        {
            deadTime = 0;
            StartCoroutine(CountDeadTime());
        }
    }

    // Set timesUp to true when we reach maxDeadTime
    private IEnumerator CountDeadTime()
    {
        while (deadTime < maxDeadTime)
        {
            deadTime += Time.deltaTime;
            yield return null;
        }
    }

    public void AddMomentum(Vector3 momentum)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.velocity = momentum;
        }
    }

    public bool IsMoving()
    {
        // check if we should just return false and get on with it if its taking too long
        if (deadTime <= maxDeadTime)
        {
            // check each rigidbody to see if it's still moving
            foreach (Rigidbody rb in rigidbodies)
            {
                if (rb.velocity != Vector3.zero)
                    return true;
            }
        }
        // reset our timer
        deadTime = 0;
        return false;
    }
}