using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPC : Racer
{
    public WaypointChain chain;

    protected Waypoint nextWaypoint;
    protected NavMeshAgent agent;

    protected const float jetpackVerticalForceOffset = 2f;
    protected const float jetpackHorizontalCorrection = 15f;
    

    protected override void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
        nextWaypoint = chain.GetStartingWaypoint();
        agent.SetDestination(nextWaypoint.pos);
        
        //agent.updatePosition = false;
        foreach(Movement m in movementOptions)
        {
            m.enabled = false;
            m.CameraController = null;
        }

        base.Start();
    }

    protected override void Update()
    {
        //move.x = agent.desiredVelocity.x;
        //move.y = agent.desiredVelocity.z;
        base.Update();

        if (movementMode == Movement.Mode.Jetpacking)
        {
            if (transform.position.y < nextWaypoint.pos.y + nextWaypoint.height - jetpackVerticalForceOffset)
            {
                SetNavMeshAgent(false);
                movement.Jump(true);
            }
            else if (!movement.Grounded)
            {
                movement.Jump(false);
            }
            if (!movement.Grounded)
            {
                // calculate offset to exit orbit
                Vector3 localVel = transform.InverseTransformDirection(rb.velocity);


                Vector3 targetDir = nextWaypoint.pos - transform.position - Vector3.Normalize(rb.velocity) * jetpackHorizontalCorrection;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 5f * Time.deltaTime, 0);
                newDir = new Vector3(newDir.x, 0, newDir.z);
                Debug.DrawRay(transform.position, newDir, Color.red);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
        else if (movementMode == Movement.Mode.Biking)
        {
            agent.acceleration = Mathf.Lerp(1.2f, 10f, rb.velocity.magnitude / movement.maxSpeed);
        }
        else if (movementMode == Movement.Mode.Swimming || movementMode == Movement.Mode.GetOffTheBoat)
        {
            Vector3 targetDir = (nextWaypoint.pos - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 5f * Time.deltaTime, 0);
            newDir = new Vector3(newDir.x, 0, newDir.z);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
            move = new Vector2(0, 1f);
        }        
    }

    public void Land()
    {
        if (movementMode == Movement.Mode.Jetpacking)
        {
            SetNavMeshAgent(true);
        }
    }

    public override void Die(bool emphasizeTorso, Vector3 newMomentum = default(Vector3))
    {
        base.Die(emphasizeTorso, newMomentum);
        agent.isStopped = true;
    }

    protected override IEnumerator RevivalEnabler()
    {
        // Don't allow a revival until we stop moving on the ground
        yield return new WaitUntil(() => !ragdoll.IsMoving());
        // then wait a bit before getting up...
        yield return new WaitForSeconds(Random.Range(0.5f, 2));
        canRevive = true;
        Revive();
    }

    public override void Revive()
    {
        base.Revive();
        if (agent.enabled)
            agent.isStopped = false;
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    public override void SetMovementMode(Movement.Mode mode, bool initial = false)
    {
        base.SetMovementMode(mode, initial);
        move = new Vector2(0, 0);
        if (mode == Movement.Mode.Swimming || mode == Movement.Mode.GetOffTheBoat)
        {
            SetNavMeshAgent(false);
        }
        else
        {
            SetNavMeshAgent(true);
        }
        agent.speed = movement.maxSpeed;
        agent.acceleration = movement.acceleration;
        agent.angularSpeed = movement.angularSpeed;
    }

    public void ArriveAtWaypoint(Waypoint waypoint)
    {
        if (waypoint.next != null)
        {
            nextWaypoint = waypoint.next;
            if (agent.enabled)
                agent.SetDestination(nextWaypoint.pos);
        }
    }

    protected void SetNavMeshAgent(bool active)
    {
        agent.enabled = active;
        if (active)
        {
            agent.isStopped = !active;
            agent.SetDestination(nextWaypoint.pos);
        }
    }
}