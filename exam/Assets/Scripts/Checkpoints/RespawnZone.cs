using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonPlayer player =
            other.GetComponent<ThirdPersonPlayer>();

        if (player != null)
        {
            PlayerHealth health =
                player.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.Respawn();
            }
            else
            {
                player.Respawn();
            }
        }
    }
}