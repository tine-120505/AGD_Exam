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
        currentHealth -= damage;

        Debug.Log("Player Health: " + currentHealth);

        if (cameraController != null)
        {
            cameraController.ShakeCamera();
        }

        flashTimer = flashDuration;

        if (currentHealth <= 0)
        {
            Die();
        }
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

        Debug.Log(
            "Player Health Reset: " + currentHealth
        );
    }
}