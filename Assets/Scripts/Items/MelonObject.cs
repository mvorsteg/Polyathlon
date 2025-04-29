using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MelonObject : MonoBehaviour 
{
    protected Rigidbody rb;
    protected AudioSource audioSource;
    public Rigidbody target;
    public Rigidbody source;
    
    [SerializeField]
    protected List<AudioClip> impactSounds;

    protected virtual void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    protected virtual void Start()
    {
        StartCoroutine(Despawn());
    }

    /*  if you hit somebody, they die */
    protected virtual void OnCollisionEnter(Collision other)
    {
        Racer racer = other.transform.GetComponent<Racer>();
        if (racer != null && other.relativeVelocity.magnitude > 7)
        {
            racer.Die(true);        
            if (impactSounds.Count > 0)
            {
                AudioClip clip = impactSounds[Random.Range(0, impactSounds.Count)];
                audioSource.PlayOneShot(clip);
            }
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

    public IEnumerator Despawn()
    {
        yield return new WaitForSeconds(90);
        Destroy(gameObject);
    }
}