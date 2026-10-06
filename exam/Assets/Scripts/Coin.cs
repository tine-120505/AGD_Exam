using UnityEngine;

public class Coin : MonoBehaviour {
    [Header("Coin Settings")]
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other) {
        // Check if the player collected the coin.
        if (other.CompareTag("Player")) {
            // Add the coin to the player's total.
            if (CoinManager.Instance != null) {
                CoinManager.Instance.AddCoin(coinValue);
            }

            // Remove the coin from the scene.
            Destroy(gameObject);
        }
    }
}