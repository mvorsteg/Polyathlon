using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPC : Racer
{
    public WaypointChain chain;

    protected NavMeshAgent agent;

    protected override void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(chain.GetStartingWaypoint());
        
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
    }

    public override void Die()
    {
        base.Die();
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
        agent.isStopped = false;
    }

    /*  updates player's movement mode and maxSpeed/locomotion accordingly */
    public override void SetMovementMode(Movement.Mode mode)
    {
        base.SetMovementMode(mode);
        agent.speed = movement.maxSpeed;
        agent.acceleration = movement.acceleration;
    }

    public void ArriveAtWaypoint(Waypoint waypoint)
    {
        if (waypoint.next != null)
        {
            agent.SetDestination(waypoint.next.transform.position);
        }
    }
    
}