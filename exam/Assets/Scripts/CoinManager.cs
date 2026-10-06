using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour {
    public static CoinManager Instance;

    [Header("UI")]
    public TextMeshProUGUI coinText;

    private int coinCount = 0;

    void Awake() {
        // Make this the only CoinManager instance.
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        UpdateCoinText();
    }

    public void AddCoin(int amount) {
        coinCount += amount;

        UpdateCoinText();
    }

    void UpdateCoinText() {
        if (coinText != null) {
            coinText.text = "Coins: " + coinCount;
        }
    }

    public int GetCoinCount() {
        return coinCount;
    }
}