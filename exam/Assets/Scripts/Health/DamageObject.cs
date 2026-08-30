using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage = 1;
    public float damageCooldown = 1f;

    private float nextDamageTime;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            return;

        if (Time.time < nextDamageTime)
            return;

        playerHealth.TakeDamage(damage);

        nextDamageTime = Time.time + damageCooldown;
    }
}