using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use if using TextMeshPro

public class CollectibleManagerStage2 : MonoBehaviour
{
    public static CollectibleManagerStage2 instance; // Singleton instance

    public TextMeshProUGUI collectibleText; // Counter Text (Use "Text" instead if not using TMP)
    public Slider progressBar; // Progress bar
    public int totalCollectibles = 10; // Set this based on the total collectibles in your level

    private int collectibleCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        progressBar.maxValue = totalCollectibles;
        UpdateUI();
    }

    public void AddCollectible()
    {
        collectibleCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"Collectibles: {collectibleCount} / {totalCollectibles}";
        }

        if (progressBar != null)
        {
            progressBar.value = collectibleCount;
        }
    }
}

