using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static Transform currentCheckpoint;

    public static void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;

        Debug.Log("New Checkpoint Set: " + checkpoint.name);
    }
}