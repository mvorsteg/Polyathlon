using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class Waypoint : MonoBehaviour 
{
    public int seq;
    public Waypoint next;
    public Vector3 pos;
    private Color color;

    private void Awake()
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
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(transform.position, 1f);
        }
        
    }
}