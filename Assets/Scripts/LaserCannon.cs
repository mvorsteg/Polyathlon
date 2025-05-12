using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : MonoBehaviour
{
    public enum TargetMode
    {
        Anyone,
        Airborne,
        AirborneIncludingGrounded
    }


    public Racer owner = null;
    protected Transform target;
    [SerializeField] protected Transform cannon;

    [SerializeField] protected GameObject projectile;
    [SerializeField] protected AudioClip cannonFiring;
    [SerializeField] protected TargetMode targetMode;
    protected AudioSource cannonAudio;

    [SerializeField] protected float laserSpeed = 120f;
    [SerializeField] protected float aimSpeed = 6f;

    private Vector3 targetPos;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        cannonAudio = GetComponent<AudioSource>();
    }

    public void SelectTarget()
    {
        // determine who to aim at
        // it'll be the closest racer who is also jetpacking
        target = null;
        
        IEnumerable<Racer> racers = RaceManager.GetRacersSortedByDistance(cannon.position); 
        foreach (Racer racer in racers)
        {
            if (!racer.IsDead() && racer != owner)
            {
                if (ValidateTarget(racer))
                {
                    target = racer.GetHips();
                    
                    if (racer.movementMode == Movement.Mode.Jetpacking || racer.movementMode == Movement.Mode.Gliding)
                    {
                        RaceManager.ActivateContingencyItems(true);
                    }
                    break;
                }
            }
        }
    }

    private bool ValidateTarget(Racer target)
    {
        switch(targetMode)
        {
            case TargetMode.Anyone:
            {
                return true;
            }
            case TargetMode.Airborne:
            {
                return (target.movementMode == Movement.Mode.Jetpacking || target.movementMode == Movement.Mode.Gliding) && !target.isGrounded();
            }
            case TargetMode.AirborneIncludingGrounded:
            {
                return target.movementMode == Movement.Mode.Jetpacking || target.movementMode == Movement.Mode.Gliding;
            }
            default:
            {
                return false;
            }
        }
    }

    public void AimAndShoot(int numberOfShots = -1)
    {
        StartCoroutine(AimAndShootCoroutine(numberOfShots));
    }

    public IEnumerator AimAndShootCoroutine(int numberOfShots)
    {
        for (int i = 0; i < numberOfShots; i++)
        {
            SelectTarget();

            if (target != null)
            {
                // Determine how long to aim for before firing the cannon
                float aimTime = Random.Range(1, 6);
                float currentAimTime = 0;
                while (currentAimTime < aimTime)
                {
                    // Estimate the target's future position by the time the laser reaches them based on their velocity and the laser's speed
                    Vector3 targetPosition = target.transform.position + target.GetComponent<Rigidbody>().linearVelocity * Vector3.Distance(target.transform.position, cannon.position) / laserSpeed;
                    // Calculate where to aim
                    Quaternion direction = Quaternion.Slerp(cannon.rotation, Quaternion.LookRotation(targetPosition - cannon.position), Time.deltaTime * aimSpeed);
                    // Clamp the rotation so that the barrel doesn't clip through the platform
                    if (direction.eulerAngles.x < 240 || direction.eulerAngles.x > 325)
                    {
                        cannon.rotation = direction;
                    }
                    currentAimTime += Time.deltaTime;
                    
                    Debug.DrawRay(2 * cannon.forward + cannon.position, cannon.forward * 50f, Color.red);
                    targetPos = targetPosition;
                    yield return null;
                }
                // Fire the cannon
                cannonAudio.PlayOneShot(cannonFiring);
                LaserBolt laser = Instantiate(projectile, 2 * cannon.forward + cannon.position, cannon.rotation).GetComponent<LaserBolt>();
                laser.Initialize(laserSpeed, owner);
            }
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(targetPos, 0.4f);
    }
}