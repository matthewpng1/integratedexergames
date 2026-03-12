using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Platformer.Mechanics;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    [Header("Collectibles")]
    public int totalCollectibles = 126;
    private int collectedCount = 0;

    [Header("UI")]
    public Slider progressBar;
    public TMP_Text progressText;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterUI(Slider bar, TMP_Text text)
    {
        Debug.Log("RegisterUI called");
        progressBar = bar;
        progressText = text;

        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = totalCollectibles;
            Debug.Log("Progress bar maxValue set to " + totalCollectibles);
        }

        UpdateProgressUI();
    }

    public void CollectiblePicked(PlayerController player = null)
    {
        collectedCount = Mathf.Clamp(collectedCount + 1, 0, totalCollectibles);
        
        // Track in session stats if a player is provided
        if (player != null && GameSessionStats.Instance != null)
        {
            GameSessionStats.Instance.AddCollectible(player);
        }
        
        UpdateProgressUI();
    }

    private void UpdateProgressUI()
    {
        if (progressBar != null)
            progressBar.value = collectedCount;

        if (progressText != null)
            progressText.text = $"{collectedCount} / {totalCollectibles}";
    }

    public int GetCollectedCount()
    {
        return collectedCount;
    }
}
