using UnityEngine;

public class Ragdoll : MonoBehaviour 
{
    //[SerializeField]
    private Rigidbody[] rigidbodies;
    //[SerializeField]
    private Collider[] colliders;

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
            rb.isKinematic = !value;
        }

        foreach (Collider c in colliders)
        {
            c.enabled = value;
        }
    }

    public void AddMomentum(Vector3 momentum)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.velocity = momentum;
        }
    }
}