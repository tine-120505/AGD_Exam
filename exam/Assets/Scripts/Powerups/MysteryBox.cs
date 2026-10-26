
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    [Header("Collection Settings")]
    public bool destroyOnCollect = true;

    private bool collected;

    void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        PlayerPowerUps powerUps =
            other.GetComponentInParent<PlayerPowerUps>();

        if (powerUps == null)
            return;

        collected = true;

        powerUps.ActivateRandomPowerUp();

        Debug.Log("Mystery Box collected!");

        if (destroyOnCollect)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
