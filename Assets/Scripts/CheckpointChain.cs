using UnityEngine;

public class CheckpointChain : MonoBehaviour
{
    private Checkpoint[] checkpoints;

    private void Awake()
    {
        checkpoints = GetComponentsInChildren<Checkpoint>();
        for (int i = checkpoints.Length - 1; i >= 0; i--)
        {
            checkpoints[i].seq = i;
            if (i < checkpoints.Length - 1)
                checkpoints[i].next = checkpoints[i + 1];
        }
    }

    public Checkpoint GetFirstCheckpoint()
    {
        return checkpoints[0];
    }
}