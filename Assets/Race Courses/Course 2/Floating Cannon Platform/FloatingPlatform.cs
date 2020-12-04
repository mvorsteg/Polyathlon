using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    const float propSpinSpeed = 500f;
    const float aimSpeed = 6f;
    const float floatAmplitude = 2f;
    const float floatSpeed = 2f;
    
    public Transform cannon;
    public GameObject missile;
    public Rigidbody[] propellers;
    private Racer[] racers;
    private Transform target;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Find the parent GameObject that all racers are a child of
        GameObject racersParent = GameObject.FindWithTag("Racers");

        // Get all the racers
        racers = racersParent.GetComponentsInChildren<Racer>();

        // Start spinning the propellers and controlling the cannon
        StartCoroutine(Hover());
        StartCoroutine(CannonControl());
    }


    // Handle aiming the cannon
    private IEnumerator CannonControl()
    {
        target = racers[0].transform;
        while(true)
        {
            // Calculate where to aim
            Quaternion direction = Quaternion.Slerp(cannon.rotation, Quaternion.LookRotation(target.position - cannon.position), Time.deltaTime * aimSpeed);
            // Clamp the rotation so that the barrel doesn't clip through the platform
            if (direction.eulerAngles.x < 240 || direction.eulerAngles.x > 325)
            {
                cannon.rotation = direction;
            }
            yield return null;
        }
    }

    // Handle propeller spinning and vertical oscillation
    private IEnumerator Hover()
    {
        while (true)
        {
            rb.MovePosition(rb.position + Vector3.up * Mathf.Cos(Time.time * floatSpeed) * Time.fixedDeltaTime * floatAmplitude);
            foreach(Rigidbody prop in propellers)
            {
                prop.MoveRotation(Quaternion.Euler(prop.rotation.eulerAngles.x, prop.rotation.eulerAngles.y + propSpinSpeed * Time.fixedDeltaTime, prop.rotation.eulerAngles.z));
                prop.MovePosition(prop.position + Vector3.up * Mathf.Cos(Time.time * floatSpeed) * Time.fixedDeltaTime * floatAmplitude);
            }
            yield return null;
        }
    }
}
