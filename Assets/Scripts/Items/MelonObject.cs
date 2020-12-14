using UnityEngine;
using System.Collections;

public class MelonObject : MonoBehaviour 
{
    private Rigidbody rb;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }

    /*  if you hit somebody, they die */
    private void OnCollisionEnter(Collision other)
    {
        Racer racer = other.transform.GetComponent<Racer>();
        if (racer != null && rb.velocity.magnitude > 5)
        {
            racer.Die(true);
        }
        else
        {
            MelonButton button = other.transform.GetComponent<MelonButton>();
            if (button != null)
            {
                StartCoroutine(button.Activate());
            }
        }
    }
}