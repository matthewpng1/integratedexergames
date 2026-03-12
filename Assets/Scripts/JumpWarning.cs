using System.Collections;
using UnityEngine;
using TMPro; // If using TextMeshPro

public class JumpWarning : MonoBehaviour
{
    public GameObject warningText; // Assign the UI text in the Inspector

    private void Start()
    {
        warningText.SetActive(false); // Hide text at start
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            StartCoroutine(ShowWarning());
        }
    }

    private IEnumerator ShowWarning()
    {
        warningText.SetActive(true); // Show warning text
        //Time.timeScale = 0f; // Pause the game
        yield return new WaitForSecondsRealtime(2f); // Wait 2 seconds in real time
        //Time.timeScale = f; // Resume the game
        warningText.SetActive(false); // Hide warning text
    }
}
