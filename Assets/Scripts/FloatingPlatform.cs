using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    const float laserSpeed = 120f;
    const float propSpinSpeed = 500f;
    const float aimSpeed = 6f;
    float floatAmplitude;
    float floatSpeed;
    
    public Transform cannon;
    public GameObject projectile;
    public Rigidbody[] propellers;
    public AudioClip cannonFiring;
    public bool shootAtGroundedPeople = true;
    private AudioSource cannonAudio;
    private Racer[] racers;
    private Transform target;
    private Rigidbody rb;
    private GameObject contingencyItems;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        // see if there are contingency items in this scene and disable them if there are
        // for example, jetpacks
        try {
            contingencyItems = GameObject.FindWithTag("ContingencyItems");
            contingencyItems.SetActive(false);
        }
        catch (System.Exception)
        {

        }
        rb = GetComponent<Rigidbody>();
        
        // Set floatAmplitude and floatSpeed randomly so that each platform has unique motion
        floatAmplitude = Random.Range(1, 2.5f);
        floatSpeed = Random.Range(1, 3);

        // Start spinning the propellers and controlling the cannon
        StartCoroutine(Hover());

        // Only aim the cannon if it is active
        // (just set it as inactive if you wanna use this platform for peaceful purposes)
        if (cannon.gameObject.activeSelf)
        {
            cannonAudio = cannon.GetComponent<AudioSource>();
            // Get all the racers
            racers = FindObjectsOfType<Racer>();
            StartCoroutine(CannonControl());
        }
    }


    // Handle aiming the cannon
    private IEnumerator CannonControl()
    {
        while (true) // do this forever
        {
            // determine who to aim at
            // it'll be the closest racer who is also jetpacking
            float minDist = Mathf.Infinity;
            target = null;
            Rigidbody targetRb = null;
            for (int i = 0; i < racers.Length; i++)
            {
                float dist = Vector3.Distance(cannon.position, racers[i].transform.position);
                if (dist < minDist && (racers[i].movementMode == Movement.Mode.Jetpacking || racers[i].movementMode == Movement.Mode.Gliding) && ((shootAtGroundedPeople && racers[i].isGrounded()) || !racers[i].isGrounded()))
                {
                    minDist = dist;
                    target = racers[i].GetHips();
                    targetRb = racers[i].GetComponent<Rigidbody>();
                    // activate contingency items as soon as someone is targeted
                    if (!activated && contingencyItems != null)
                    {
                        activated = true;
                        contingencyItems.SetActive(true);
                    }
                }
            }
            if (target != null)
            {
                // Determine how long to aim for before firing the cannon
                float aimTime = Random.Range(1, 6);
                float currentAimTime = 0;
                while (currentAimTime < aimTime)
                {
                    // Estimate the target's future position by the time the laser reaches them based on their velocity and the laser's speed
                    Vector3 targetPosition = target.position + targetRb.linearVelocity * Vector3.Distance(target.position, cannon.position) / laserSpeed;
                    // Calculate where to aim
                    Quaternion direction = Quaternion.Slerp(cannon.rotation, Quaternion.LookRotation(targetPosition - cannon.position), Time.deltaTime * aimSpeed);
                    // Clamp the rotation so that the barrel doesn't clip through the platform
                    if (direction.eulerAngles.x < 240 || direction.eulerAngles.x > 325)
                    {
                        cannon.rotation = direction;
                    }
                    currentAimTime += Time.deltaTime;
                    yield return null;
                }
                // Fire the cannon
                cannonAudio.PlayOneShot(cannonFiring);
                GameObject laser = Instantiate(projectile, 2 * cannon.forward + cannon.position, cannon.rotation);
                laser.GetComponent<LaserBolt>().SetSpeed(laserSpeed);
            }
            yield return null;
        }
    }

    // Handle propeller spinning and vertical oscillation
    private IEnumerator Hover()
    {
        while (true)
        {
            // oscillate vertically
            rb.MovePosition(rb.position + Vector3.up * Mathf.Cos(Time.time * floatSpeed) * Time.fixedDeltaTime * floatAmplitude);
            // spin the propellers
            foreach(Rigidbody prop in propellers)
            {
                prop.MoveRotation(Quaternion.Euler(prop.rotation.eulerAngles.x, prop.rotation.eulerAngles.y + propSpinSpeed * Time.fixedDeltaTime, prop.rotation.eulerAngles.z));
                prop.MovePosition(prop.position + Vector3.up * Mathf.Cos(Time.time * floatSpeed) * Time.fixedDeltaTime * floatAmplitude);
            }
            yield return null;
        }
    }
}
