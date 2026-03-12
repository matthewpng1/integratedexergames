using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopMenuEvents : MonoBehaviour
{
    private string homeScene = "MainMenu"; // Change this to your actual Main Menu scene name
    private string gameScene = "GameMap"; // Change this to your actual Game Map scene name

    // Loads the game scene when Play is pressed
    public void LoadGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    // Returns to the Main Menu when Back is pressed
    public void LoadHomePage()
    {
        SceneManager.LoadScene(homeScene);
    }
}
