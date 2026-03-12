using UnityEngine;

public class CollectibleStage3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has the "Player" tag
        {
            CollectibleManagerStage3.instance.AddCollectible();
            Destroy(gameObject); // Remove the collectible after collection
        }
    }
}
