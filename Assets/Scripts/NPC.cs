using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPC : Racer
{
    protected WaypointChain chain;
    protected IWaypointable nextWaypoint;
    protected NavMeshAgent agent;

    protected const float jetpackVerticalForceOffset = 2f;
    protected const float jetpackHorizontalCorrection = 15f;


    protected override void Start() 
    {
        chain = GameObject.FindWithTag("WaypointChainStart").GetComponent<WaypointChain>();
        agent = GetComponent<NavMeshAgent>();
        nextWaypoint = chain.GetStartingWaypoint();
        SetNavMeshAgent(false);
        
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
            if (transform.position.y < nextWaypoint.GetPos(this).y + nextWaypoint.GetHeight() - jetpackVerticalForceOffset)
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
                Vector3 targetDir = nextWaypoint.GetPos(this) - transform.position - Vector3.Normalize(rb.velocity) * jetpackHorizontalCorrection;
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
            Vector3 targetDir = (nextWaypoint.GetPos(this) - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 5f * Time.deltaTime, 0);
            newDir = new Vector3(newDir.x, 0, newDir.z);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
            move = new Vector2(0, 1f);
        }        
    }

    void FixedUpdate()
    {
        if (movementMode == Movement.Mode.Running || movementMode == Movement.Mode.Jetpacking && movement.Grounded)
        {
            if (movement.BonusSpeed == 1)
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 20);
        }
    }

    public override void EquipItem(Item item)
    {
        Debug.Log("Equip");
        base.EquipItem(item);
        if (item != null)
        {
            StartCoroutine(UseItem());
        }
    }

    private IEnumerator UseItem()
    {
        Debug.Log("Use");
        float s = Random.Range(0.2f, 10f);
        yield return new WaitForSeconds(s);
        if (item != null)
            item.Use(this);
    }

    /*  called by the RaceManager, makes the NPC start moving
        the NPC will not officially start the race until this is called */
    public override void StartRace()
    {
        Debug.Log("go");
        base.StartRace();
        SetMovementMode(movementMode, true);
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
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    protected override IEnumerator RevivalEnabler()
    {
        // Don't allow a revival until we stop moving on the ground
        yield return new WaitUntil(() => !ragdoll.IsMoving());
        // then wait a bit before getting up...
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        canRevive = true;
        Revive();
    }

    public override void Revive()
    {
        base.Revive();
        if (agent.enabled && agent.isOnNavMesh)
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

    public void ArriveAtWaypoint(IWaypointable waypoint)
    {
        if (waypoint.GetFork().Length > 0)
        {
            nextWaypoint = waypoint.GetFork()[Random.Range(0, waypoint.GetFork().Length)].GetStartingWaypoint();
            if (agent.enabled && agent.isOnNavMesh)
                agent.SetDestination(nextWaypoint.GetPos(this));
        }
        else if (waypoint.Next != null)
        {
            nextWaypoint = waypoint.Next;
            if (agent.enabled && agent.isOnNavMesh)
                agent.SetDestination(nextWaypoint.GetPos(this));
        }
    }

    protected void SetNavMeshAgent(bool active)
    {
        agent.enabled = active;
        if (active && agent.isOnNavMesh && RaceManager.IsRaceActive)
        {
            agent.isStopped = !active;
            agent.SetDestination(nextWaypoint.GetPos(this));
        }
    }
}