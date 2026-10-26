
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerUps : MonoBehaviour
{
    public enum PowerUpType
    {
        Speed,
        Shield,
        HP,
        JumpBoost
    }

    [Header("Power-Up Settings")]
    public float speedMultiplier = 1.8f;
    public float jumpMultiplier = 1.8f;
    public float powerUpDuration = 8f;
    public int healthRestoreAmount = 2;

    [Header("Duration Bar UI")]
    public GameObject durationBarPanel;
    public Image durationBarFill;

    [Header("Power-Up Colors")]
    public Color speedColor = Color.cyan;
    public Color shieldColor = new Color(0.5f, 0.3f, 1f);
    public Color hpColor = Color.green;
    public Color jumpColor = new Color(1f, 0.65f, 0f);

    [Header("Power-Up Visual Effects")]
    public GameObject speedBarrier;
    public GameObject shieldBarrier;
    public GameObject jumpBarrier;

    private ThirdPersonPlayer player;
    private PlayerHealth playerHealth;

    private float originalSpeed;
    private float originalJumpHeight;

    private float remainingTime;
    private float activeDuration;

    private bool isPowerUpActive;
    private PowerUpType activePowerUp;

    public bool IsShieldActive { get; private set; }

    void Awake()
    {
        player = GetComponent<ThirdPersonPlayer>();
        playerHealth = GetComponent<PlayerHealth>();

        if (player != null)
        {
            originalSpeed = player.moveSpeed;
            originalJumpHeight = player.jumpHeight;
        }

        HideAllBarriers();

        if (durationBarPanel != null)
            durationBarPanel.SetActive(false);
    }

    void Update()
    {
        if (!isPowerUpActive)
            return;

        remainingTime -= Time.deltaTime;

        if (durationBarFill != null && activeDuration > 0f)
        {
            durationBarFill.fillAmount =
                Mathf.Clamp01(remainingTime / activeDuration);
        }

        if (remainingTime <= 0f)
        {
            EndPowerUp();
        }
    }

    public void ActivateRandomPowerUp()
    {
        PowerUpType randomPowerUp =
            (PowerUpType)Random.Range(0, 4);

        ActivatePowerUp(randomPowerUp);
    }

    public void ActivatePowerUp(PowerUpType type)
    {
        // End any previously active timed power-up.
        if (isPowerUpActive)
            EndPowerUp();

        // Remove any leftover visual effects.
        HideAllBarriers();

        activePowerUp = type;

        switch (type)
        {
            case PowerUpType.Speed:
                if (player == null)
                    return;

                player.moveSpeed =
                    originalSpeed * speedMultiplier;

                ShowBarrier(speedBarrier);
                StartTimedPowerUp(speedColor);

                Debug.Log("Speed Boost activated!");
                break;

            case PowerUpType.Shield:
                IsShieldActive = true;

                ShowBarrier(shieldBarrier);
                StartTimedPowerUp(shieldColor);

                Debug.Log("Shield activated!");
                break;

            case PowerUpType.HP:
                if (playerHealth != null)
                {
                    playerHealth.Heal(healthRestoreAmount);
                }

                Debug.Log("HP restored!");
                break;

            case PowerUpType.JumpBoost:
                if (player == null)
                    return;

                player.jumpHeight =
                    originalJumpHeight * jumpMultiplier;

                ShowBarrier(jumpBarrier);
                StartTimedPowerUp(jumpColor);

                Debug.Log("Jump Boost activated!");
                break;
        }
    }

    void StartTimedPowerUp(Color barColor)
    {
        activeDuration = powerUpDuration;
        remainingTime = activeDuration;
        isPowerUpActive = true;

        if (durationBarPanel != null)
            durationBarPanel.SetActive(true);

        if (durationBarFill != null)
        {
            durationBarFill.color = barColor;
            durationBarFill.fillAmount = 1f;
        }
    }

    void EndPowerUp()
    {
        // Restore the player's original movement stats.
        if (player != null)
        {
            player.moveSpeed = originalSpeed;
            player.jumpHeight = originalJumpHeight;
        }

        // Disable Shield.
        IsShieldActive = false;

        // Stop the timer.
        isPowerUpActive = false;
        remainingTime = 0f;

        // Hide the duration bar.
        if (durationBarPanel != null)
            durationBarPanel.SetActive(false);

        // Hide all barrier models.
        HideAllBarriers();

        Debug.Log(activePowerUp + " power-up ended.");
    }

    void ShowBarrier(GameObject barrier)
    {
        HideAllBarriers();

        if (barrier != null)
            barrier.SetActive(true);
    }

    void HideAllBarriers()
    {
        if (speedBarrier != null)
            speedBarrier.SetActive(false);

        if (shieldBarrier != null)
            shieldBarrier.SetActive(false);

        if (jumpBarrier != null)
            jumpBarrier.SetActive(false);
    }
}
