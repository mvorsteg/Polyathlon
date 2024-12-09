using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    public AudioClip laserImpact;
    const float impactVelMax = 30f;
    private Rigidbody rb;
    private float speed;
    private AudioSource audioSource;
    private Transform laserChild;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = laserImpact;
        laserChild = transform.GetChild(0);
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        StartCoroutine(DestroyIfMissed());
    }

    // Kill the racer if we hit them
    void OnCollisionEnter(Collision other)
    {
        Racer racer = other.gameObject.GetComponentInParent<Racer>();
        if (racer != null)
        {
            racer.Die(true, Vector3.ClampMagnitude(rb.linearVelocity, impactVelMax));
        }
        Destroy(laserChild.gameObject);
        rb.linearVelocity = Vector3.zero;
        StartCoroutine(DestroyAfterPlayingSound());
    }

    // Make sure the laser impact sound effect plays before we destory this
    private IEnumerator DestroyAfterPlayingSound()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }

    // Destroy the laser if we very clearly missed the target.
    private IEnumerator DestroyIfMissed()
    {
        yield return new WaitForSeconds(8);
        Destroy(gameObject);
    }

    
}
