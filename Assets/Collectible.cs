using UnityEngine;
using Platformer.Mechanics;

public class Collectible : MonoBehaviour
{
    public float bounceHeight = 0.3f; // How high it bounces
    public float bounceSpeed = 4f; // Speed of the bounce
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // Store the original position
    }

    private void Update()
    {
        // Make the collectible move up and down smoothly
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * bounceSpeed) * bounceHeight, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the Player has the "Player" tag
        {
            // Get the player controller and pass it to the manager
            PlayerController player = other.GetComponent<PlayerController>();
            if (CollectibleManager.Instance != null)
            {
                CollectibleManager.Instance.CollectiblePicked(player);
            }
            Destroy(gameObject); // Remove the collectible after collection
        }
    }
}
