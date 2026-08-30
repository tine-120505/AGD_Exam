using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThirdPersonPlayer>() != null)
        {
            CheckpointManager.SetCheckpoint(transform);
            
            Debug.Log("Checkpoint Reached!");
        }
    }
}