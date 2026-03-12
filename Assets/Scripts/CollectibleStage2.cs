using UnityEngine;

public class CollectibleStage2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the "Player" tag
        {
            CollectibleManagerStage2.instance.AddCollectible();
            Destroy(gameObject); // Remove the collectible after collection
        }
    }
}
