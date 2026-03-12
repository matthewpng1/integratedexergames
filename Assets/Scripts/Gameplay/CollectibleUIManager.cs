using UnityEngine;
using TMPro;

public class CollectiblesUIManager : MonoBehaviour
{
    public TextMeshProUGUI collectiblesText;
    private int collectibleCount = 0;

    void Start()
    {
        UpdateCollectibleUI();
    }

    public void IncreaseCollectibleCount()
    {
        collectibleCount++;
        UpdateCollectibleUI();
    }

    private void UpdateCollectibleUI()
    {
        if (collectiblesText != null)
        {
            collectiblesText.text = "Collectibles: " + collectibleCount;
        }
        else
        {
            Debug.LogError("CollectiblesText is missing! Ensure it is assigned in the Inspector.");
        }
    }
}