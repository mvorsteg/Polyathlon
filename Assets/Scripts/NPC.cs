using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class NPC : Racer
{
    protected WaypointChain chain;
    protected IWaypointable nextWaypoint;
    protected NavMeshAgent agent;

    protected const float jetpackVerticalForceOffset = 2f;
    protected const float jetpackHorizontalCorrection = 15f;

    protected const float agentSpeed = 5;

    private bool gliderControlActive = false;
    private bool isGoingToJetpack = false;

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
                Vector3 targetDir = nextWaypoint.GetPos(this) - transform.position - Vector3.Normalize(rb.linearVelocity) * jetpackHorizontalCorrection;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 5f * Time.deltaTime, 0);
                newDir = new Vector3(newDir.x, 0, newDir.z);
                Debug.DrawRay(transform.position, newDir, Color.red);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
        else if (movementMode == Movement.Mode.Gliding)
        {
            // TODO: Detect if we're gliding and figure out how to pilot the glider
            if (!movement.Grounded)
            {
                if (!gliderControlActive)
                    StartCoroutine(GliderControl());
            }
        }
        else if (movementMode == Movement.Mode.Biking)
        {
            agent.acceleration = Mathf.Lerp(1.2f, 10f, rb.linearVelocity.magnitude / movement.maxSpeed);
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
                rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 20);
        }
        else if (movementMode == Movement.Mode.Biking && movement.BonusSpeed == 1)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 50);
        }
    }


    IEnumerator GliderControl()
    {
        gliderControlActive = true;
        bool landOnHead = Random.Range(0, 2) == 1;
        float stallSpeed = Random.Range(15f, 25f);
        bool begunTurning = false;
        while(!movement.Grounded)
        {
            // Roll
            Vector3 nextWaypointPos = nextWaypoint.GetPos(this);
            Vector3 desiredDirection = new Vector3(nextWaypointPos.x, 0, nextWaypointPos.z) - new Vector3(transform.position.x, 0, transform.position.z);
            move = new Vector2(0, 0);
            // Don't let them start rolling too early or they won't have enough lift and they'll land prematurely
            if (begunTurning || rb.linearVelocity.magnitude > Mathf.Max(stallSpeed - 5f, 15f))
            {
                begunTurning = true;
                Vector3 velocityDirection = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).normalized;
                Vector3 cross = Vector3.Cross(velocityDirection, desiredDirection);
                // Target is to the right
                if (cross.y > 0 && (characterMesh.right.x > 0.3f || characterMesh.right.y > 0))
                {
                    move = new Vector2(0.1f, 0f);
                }
                // Target is to the left
                else if (cross.y < 0 && (characterMesh.right.x > 0.3f || characterMesh.right.y < 0))
                {
                    move = new Vector2(-0.1f, 0f);
                }
            }

            // Pitch
            float estimatedTimeToLanding = rb.linearVelocity.y / (transform.position.y - nextWaypointPos.y);
            Vector3 predictedLandingLocation = transform.position + (rb.linearVelocity * estimatedTimeToLanding);
            float distanceToLanding = Vector3.Distance(transform.position, predictedLandingLocation);
            float distanceToTarget = Vector3.Distance(transform.position, nextWaypointPos);

            // Also determine whether the target point is already behind us.
            // When targetAngle is positive, it's ahead, when negative, it's behind
            float targetAngle = Vector3.SignedAngle(desiredDirection, transform.forward, transform.right);

            // We passed it! Land immediately!
            if (targetAngle >= 90)
            {
                // Land on head
                if (landOnHead) {
                    if (characterMesh.up.z < 0.9f) {
                        move += new Vector2(0, 1f);
                    }
                } else {
                    // Land on feet
                    if (characterMesh.up.z > -0.9f) {
                        move += new Vector2(0, -1f);
                    }
                }
            }
            else
            {
                if (distanceToLanding < distanceToTarget)
                {
                    if (rb.linearVelocity.magnitude < stallSpeed && characterMesh.up.z < 0.5f)
                    {
                        move += new Vector2(0, 0.1f);
                    }
                    else if (rb.linearVelocity.magnitude > stallSpeed && characterMesh.up.z > -0.5f)
                    {
                        move += new Vector2(0, -0.1f);
                    }
                }
                else if ((distanceToLanding > distanceToTarget && (characterMesh.up.z > -0.5f)))
                {
                    
                    if (rb.linearVelocity.magnitude < stallSpeed && characterMesh.up.z > -0.5f)
                    {
                        move += new Vector2(0, -0.1f);
                    }
                    else if (rb.linearVelocity.magnitude > stallSpeed && characterMesh.up.z < 0.5f)
                    {
                        move += new Vector2(0, 0.1f);
                    }
                }
            }
            yield return null;
        } 
        move = new Vector2(0,0);
        gliderControlActive = false;
    }
    

    public override void EquipItem(Item item)
    {
        base.EquipItem(item);
        if (item != null)
        {
            StartCoroutine(UseItem());
        }
    }

    private IEnumerator UseItem()
    {
        float s = Random.Range(0.2f, 10f);
        yield return new WaitForSeconds(s);
        if (item != null)
            item.Use(this);
    }

    protected override IEnumerator SpeedBoost(float magnitude, float duration)
    {
        movement.BonusSpeed = magnitude;
        anim.speed = magnitude;
        agent.speed = agentSpeed * magnitude;
        yield return new WaitForSeconds(duration);
        movement.BonusSpeed = 1f;
        anim.speed = 1f;
        agent.speed = agentSpeed;
    }

    /*  called by the RaceManager, makes the NPC start moving
        the NPC will not officially start the race until this is called */
    public override void StartRace()
    {
        base.StartRace();
        SetMovementMode(movementMode, true);
    }

    public void Land()
    {
        if (movementMode == Movement.Mode.Jetpacking || movementMode == Movement.Mode.Gliding)
        {
            SetNavMeshAgent(true);
            if (movementMode == Movement.Mode.Gliding)
            {
                characterMesh.localEulerAngles = new Vector3(0,0,0);
            }
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
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        canRevive = true;
        Revive();
    }

    public override void Revive(bool forceRevive = false)
    {
        base.Revive(forceRevive);
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
        if (((MonoBehaviour)waypoint).gameObject.CompareTag("FinalWaypoint"))
        {
            isGoingToJetpack = true;
        }
        if (waypoint.GetFork().Length == 0 && waypoint.Next == null)
        {
            if (agent.isOnNavMesh)
                agent.ResetPath();
        }
        else if (waypoint.GetFork().Length > 0)
        {
            nextWaypoint = waypoint.GetFork()[Random.Range(0, waypoint.GetFork().Length)];
        }
        else if (waypoint.Next != null)
        {
            nextWaypoint = waypoint.Next;
        }
        if (waypoint is JumpOffWaypoint)
        {
            SetNavMeshAgent(false);
            // Some NPCs should be smart and make beelines for target
            if (Random.Range(0,20) < 5)
            {
                Vector3 nextWaypointPos = nextWaypoint.GetPos(this);
                Vector3 targetDir = (new Vector3(nextWaypointPos.x, 0, nextWaypointPos.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 4, 0);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
            movement.Jump(true);
        }
        else
        {
            if (agent.enabled && agent.isOnNavMesh)
                agent.SetDestination(nextWaypoint.GetPos(this));
        }
    }

    // When you end up not where you'd like to be, the best way out is a jetpack. Or the finish line.
    public void GoToNearestJetpack()
    {
        if (!isGoingToJetpack && !isFinished)
            StartCoroutine(GoToNearestJetpackOnceOnNavMesh());
    }

    private IEnumerator GoToNearestJetpackOnceOnNavMesh()
    {
        isGoingToJetpack = true;
        while(!(agent.enabled && agent.isOnNavMesh))
        {
            yield return null;
        }
        bool success = false;

        while (!success)
        {
            // TODO: Change this when upgrading Unity version later to FindObjectsByType
            var jetpackItems = Object.FindObjectsOfType(typeof(JetpackItem));
            Dictionary <float, IWaypointable> nearestJetpacks = new Dictionary<float, IWaypointable>();
            foreach (JetpackItem jetpackItem in jetpackItems)
            {
                NavMeshPath path = new NavMeshPath();
                if (GetPath(path, transform.position, jetpackItem.transform.position, NavMesh.AllAreas))
                {
                    float pathLength = GetPathLength(path);
                    if (pathLength >= 0 && !nearestJetpacks.Keys.Contains(pathLength))
                        nearestJetpacks.Add(pathLength, jetpackItem.transform.parent.GetComponent<IWaypointable>());
                }    
            }
            GameObject finishWaypoint = GameObject.FindGameObjectWithTag("FinalWaypoint");
            if (finishWaypoint != null)
            {
                NavMeshPath path = new NavMeshPath();
                if (GetPath(path, transform.position, finishWaypoint.transform.position, NavMesh.AllAreas))
                {
                    float pathLength = GetPathLength(path);
                    if (pathLength >= 0 && !nearestJetpacks.Keys.Contains(pathLength))
                        nearestJetpacks.Add(pathLength, finishWaypoint.GetComponent<IWaypointable>());
                }
            }
            if (nearestJetpacks.Keys.Count > 0)
            {
                float closestDistance = nearestJetpacks.Keys.Min();
                nextWaypoint = nearestJetpacks[closestDistance];
                while(!(agent.enabled && agent.isOnNavMesh))
                {
                    yield return null;
                }
                agent.SetDestination(nextWaypoint.GetPos(this));
                success = true;
            } else
            {
                yield return null;
            }
        }
        isGoingToJetpack = false;
    }

    protected void SetNavMeshAgent(bool active)
    {
        agent.enabled = active;
        if (active && agent.isOnNavMesh && RaceManager.IsRaceActive)
        {
            agent.isStopped = false;
            agent.SetDestination(nextWaypoint.GetPos(this));
        }
    }

    // Credit to https://discussions.unity.com/t/getting-the-distance-in-nav-mesh/574818/6
    public static bool GetPath(NavMeshPath path, Vector3 fromPos, Vector3 toPos, int passableMask)
    {
        path.ClearCorners();
        if (NavMesh.CalculatePath(fromPos, toPos, passableMask, path) == false)
            return false;
       
        return true;
    }
       
    public static float GetPathLength(NavMeshPath path)
    {
        float pathLength = 0.0f;
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            for (int i = 1; i < path.corners.Length; ++i)
            {
                pathLength += Vector3.Distance(path.corners[i-1], path.corners[i]);
            }
        }
        else
        {
            return -1f;
        }
        return pathLength;
    }
}