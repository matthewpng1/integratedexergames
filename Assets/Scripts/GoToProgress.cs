using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene loading

public class GoToProgress : MonoBehaviour
{
    // Method to load the "TrackerScene"
    public void LoadTracker()
    {
        SceneManager.LoadScene("Tracker");  // Replace "TrackerScene" with your actual scene name
    }
}
