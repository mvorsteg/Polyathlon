using UnityEngine;

public interface IWaypointable
{
    IWaypointable Next { get; set; }
    int Seq { get; set; }
    Vector3 GetPos(NPC npc);
    float GetHeight();
    WaypointChain[] GetFork();
}
