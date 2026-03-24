using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Assign PauseMenuCanvas in Inspector
    public GameObject pauseButton;  // Assign PauseButton in Inspector
    
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);  // Ensure menu is hidden at start
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;  // Freeze game
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;  // Resume game
    }

    // This method loads the "stage1countries" scene when the home button is pressed
    public void GoToHome()
    {
        Time.timeScale = 1f;  // Reset time scale before loading a scene
        SceneManager.LoadScene("Menu"); // Change "Stage1Countries" to your home scene name
    }
}

