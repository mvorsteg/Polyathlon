using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class Waypoint : MonoBehaviour, IWaypointable 
{
    public IWaypointable Next { get { return next; } set { next = value; } }
    private IWaypointable next;
    public int Seq { get { return seq; } set { seq = value; } }
    private int seq;
    public Vector3 pos;
    public float height;
    public bool water = false;
    public bool forceToGround = false;
    // If this is the end of the chain, you can add other chains for NPCs to decide to follow
    public WaypointChain[] fork;
    private Color color;
    private Dictionary<NPC, Vector3> npcDestinations;

    private void Awake()
    {
        npcDestinations = new Dictionary<NPC, Vector3>();
        if (!water) // place the position firmly on the ground
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
            {
                pos = hit.position;
                color = new Color(0, 1, 0, 0.5f);
            }
            else
            {
                pos = transform.position;
                color = new Color(1, 0, 0, 0.5f);
            }
        }
        else 
        {
            pos = transform.position;
            color = new Color(1, 0, 0, 0.5f);
        }
        if (!forceToGround)
            height = Mathf.Max(transform.GetComponent<CapsuleCollider>().radius, transform.GetComponent<CapsuleCollider>().height / 2) / 2;
        else
            height = 0;
    }

    public Vector3 GetPos(NPC npc)
    {
        if (water)
        {
            if (!npcDestinations.ContainsKey(npc))
                npcDestinations[npc] = pos + Random.insideUnitSphere * transform.GetComponent<CapsuleCollider>().radius * 0.9f;
            return npcDestinations[npc];
        }
        else
            return pos;
    }

    public float GetHeight()
    {
        return height;
    }

    public WaypointChain[] GetFork()
    {
        return fork;
    }

    private void OnTriggerEnter(Collider other) 
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc != null)    // check to make sure agent is part of this route
        {
            //Debug.Log(other);
            npc.ArriveAtWaypoint(this);
        }
    }

    private void OnDrawGizmos()
    {
        
        if (Application.isPlaying)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(pos, 1f);
            Gizmos.DrawLine(pos, pos + height * Vector3.up * 2);
            Gizmos.DrawSphere(pos + height * Vector3.up, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.DrawLine(transform.position, transform.position + height * Vector3.up * 2);
            Gizmos.DrawSphere(transform.position + height * Vector3.up, 0.5f);
        }
        
    }
}