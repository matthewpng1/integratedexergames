using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuEvents : MonoBehaviour
{
    public void LoadLevel(int index) 
    {    
        SceneManager.LoadScene(index); // Ensure you have the correct scene index
    }

    public void Logout()
    {
        // Reset in-memory counters
        if (ProgressTracker.Instance != null)
        {
            ProgressTracker.Instance.ReloadForNewUser(); // Reset to 0
        }

        if (CollectibleManager.Instance != null)
        {
            // Reset collectedCount by destroying and recreating (or add a Reset method if needed)
            Destroy(CollectibleManager.Instance.gameObject);
        }

        // Clear the current user
        if (UserManager.Instance != null)
        {
            UserManager.Instance.Logout();
        }

        Debug.Log("MenuEvents: User logged out, reps/grips/collectibles reset.");
    }
}
