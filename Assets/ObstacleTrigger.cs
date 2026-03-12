using UnityEngine;
using TMPro;
using Platformer.Mechanics;

public class ObstacleTrigger : MonoBehaviour
{
    public GameObject jumpMessage; // UI Message for "Jump!"
    public PlayerController playerController; // Reference to PlayerController script
    public bool isPracticeMode = true; // ✅ Enable only for practice stage
    private bool isPaused = false; // Prevent multiple triggers

    void Start()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        if (jumpMessage != null)
        {
            jumpMessage.SetActive(false); // Hide message at start
        }
        else
        {
            Debug.LogError("❌ JumpMessage is NOT assigned in the Inspector!");
        }

        if (playerController == null)
        {
            Debug.LogError("❌ PlayerController is NOT assigned in the Inspector!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"🛑 Triggered by: {other.gameObject.name}"); // ✅ Debug log

        if (playerController == null)
        {
            playerController = other.GetComponent<PlayerController>();
        }

        if (other.CompareTag("Player") && isPracticeMode && !isPaused)
        {
            Debug.Log("✅ Player detected! Stopping movement and showing message.");
            isPaused = true;
            PausePlayer(); // ✅ Stop movement
            if (jumpMessage != null)
            {
                jumpMessage.SetActive(true); // ✅ Show message
            }
        }
    }

    void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("✅ Jump detected! Resuming game...");
            isPaused = false;
            ResumePlayer(); // ✅ Resume movement
            playerController.TriggerJump(); // Trigger the jump for tutorial
            if (jumpMessage != null)
            {
                jumpMessage.SetActive(false); // ✅ Hide message
            }
        }
    }

    // // ✅ Function for Arduino to trigger game continuation
    // public bool PlayerJumpDetected()
    // {
    //     return false; // 🔹 Modify this to return true when Arduino detects a jump
    // }

    private void PausePlayer()
    {
        if (playerController != null)
        {
            Debug.Log("🛑 Player movement paused!"); // ✅ Debugging
            playerController.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Stops movement
            playerController.enabled = false; // Disables player movement
        }
        else
        {
            Debug.LogError("❌ PlayerController reference is missing!");
        }
    }

    private void ResumePlayer()
    {
        if (playerController != null)
        {
            Debug.Log("▶️ Player movement resumed!"); // ✅ Debugging
            playerController.enabled = true; // Enables movement again
        }
        else
        {
            Debug.LogError("❌ PlayerController reference is missing!");
        }
    }
}
