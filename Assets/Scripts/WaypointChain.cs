using UnityEngine;

public class WaypointChain : MonoBehaviour
{
    private Waypoint[] waypoints;

    private void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>();
        for (int i = waypoints.Length - 1; i >= 0; i--)
        {
            waypoints[i].seq = i;
            if (i < waypoints.Length - 1)
                waypoints[i].next = waypoints[i + 1];
        }
    }

    public Vector3 GetStartingWaypoint()
    {
        return waypoints[0].pos;
    }
}