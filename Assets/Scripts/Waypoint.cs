using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class Waypoint : MonoBehaviour 
{
    public int seq;
    public Waypoint next;
    public Vector3 pos;
    public float height;
    public bool water = false;
    public bool forceToGround = false;
    public WaypointChain[] fork;
    private Color color;

    private void Awake()
    {
        if (!water)
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