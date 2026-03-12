using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameMap"); // Load GameMap scene
    }

    public void ExitGame()
    {
        Application.Quit(); // Exit game
    }
}
