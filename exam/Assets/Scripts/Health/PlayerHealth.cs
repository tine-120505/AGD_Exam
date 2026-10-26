
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;

    [Header("Camera Shake")]
    public ThirdPersonCamera cameraController;

    [Header("Red Vignette")]
    public RawImage redVignette;

    public float lowHealthOpacity = 0.45f;
    public float damageFlashOpacity = 0.8f;
    public float flashDuration = 0.15f;

    private int currentHealth;
    private float flashTimer;

    public int CurrentHealth => currentHealth;

    void Start()
    {
        ResetHealth();
    }

    void Update()
    {
        UpdateVignette();

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        // Ignore invalid damage.
        if (damage <= 0)
            return;

        // Check whether the player has an active Shield power-up.
        PlayerPowerUps powerUps = GetComponent<PlayerPowerUps>();

        if (powerUps != null && powerUps.IsShieldActive)
        {
            Debug.Log("Shield blocked the damage!");
            return;
        }

        // Apply damage.
        currentHealth = Mathf.Clamp(
            currentHealth - damage,
            0,
            maxHealth
        );

        Debug.Log("Player Health: " + currentHealth);

        // Shake the camera when damaged.
        if (cameraController != null)
        {
            cameraController.ShakeCamera();
        }

        // Trigger the damage flash.
        flashTimer = flashDuration;

        // Handle death.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
            return;

        int previousHealth = currentHealth;

        currentHealth = Mathf.Clamp(
            currentHealth + amount,
            0,
            maxHealth
        );

        Debug.Log(
            "Player Healed: " +
            previousHealth + " -> " + currentHealth
        );
    }

    void UpdateVignette()
    {
        if (redVignette == null)
            return;

        float healthLost =
            1f - ((float)currentHealth / maxHealth);

        float healthOpacity =
            healthLost * lowHealthOpacity;

        float flashOpacity = 0f;

        if (flashTimer > 0f)
        {
            flashOpacity = damageFlashOpacity;
        }

        float finalOpacity =
            Mathf.Max(healthOpacity, flashOpacity);

        Color color = redVignette.color;
        color.a = finalOpacity;
        redVignette.color = color;
    }

    void Die()
    {
        Respawn();
    }

    public void Respawn()
    {
        // Reset health.
        ResetHealth();

        // Find the ThirdPersonPlayer component.
        ThirdPersonPlayer player =
            GetComponent<ThirdPersonPlayer>();

        if (player != null)
        {
            player.Respawn();
        }
        else
        {
            Debug.LogWarning(
                "PlayerHealth could not find ThirdPersonPlayer!"
            );
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        flashTimer = 0f;

        UpdateVignette();

        Debug.Log(
            "Player Health Reset: " + currentHealth
        );
    }
}
