using UnityEngine;
using TMPro;

public class LevelFinish : MonoBehaviour {
    [Header("Results Panel")]
    public GameObject resultsPanel;
    public TextMeshProUGUI coinsResultText;

    private bool levelFinished = false;

    private void Start() {
        // Make sure the results panel starts hidden.
        if (resultsPanel != null) {
            resultsPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Make sure only the player can activate the finish.
        if (!other.CompareTag("Player"))
            return;

        // Prevent the trigger from activating multiple times.
        if (levelFinished)
            return;

        levelFinished = true;

        ShowResults();
    }

    private void ShowResults() {
        // Get the current coin count.
        int collectedCoins = 0;

        if (CoinManager.Instance != null) {
            collectedCoins = CoinManager.Instance.GetCoinCount();
        }

        // Display the coin count.
        if (coinsResultText != null) {
            coinsResultText.text = "Coins Collected: " + collectedCoins;
        }

        // Show the results panel.
        if (resultsPanel != null) {
            resultsPanel.SetActive(true);
        }

        // Pause the game.
        Time.timeScale = 0f;

        Debug.Log("Level Complete! Coins collected: " + collectedCoins);
    }
}