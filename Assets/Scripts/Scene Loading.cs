using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static string previousScene = "";

    private void Start()
    {
        // Load previous scene name from PlayerPrefs when starting
        previousScene = PlayerPrefs.GetString("PreviousScene", "");
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Button Clicked! Attempting to load scene: " + sceneName);
        
        // Store the current scene before switching
        previousScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("PreviousScene", previousScene);
        PlayerPrefs.Save(); // Save changes

        SceneManager.LoadScene(sceneName);
    }

    public void LoadPreviousScene()
    {
        if (SceneManager.GetActiveScene().name == "Stage 1") 
        {
            Debug.Log("At Stage 1, returning to Main Menu.");
            SceneManager.LoadScene("Menu"); 
        }
        else if (!string.IsNullOrEmpty(previousScene))
        {
            Debug.Log("Going back to previous scene: " + previousScene);
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.Log("No previous scene recorded.");
        }
    }
}
