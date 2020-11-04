using UnityEngine;
using UnityEngine.AI;

public class NPC : Racer
{
    public WaypointChain chain;

    protected NavMeshAgent agent;

    protected override void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(chain.GetStartingWaypoint());
        base.Start();
        //agent.updatePosition = false;
        foreach(Movement m in movementOptions)
        {
            m.enabled = false;
            m.CameraController = null;
        }
    }

    protected override void Update()
    {
        //move.x = agent.desiredVelocity.x;
        //move.y = agent.desiredVelocity.z;
        base.Update();
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