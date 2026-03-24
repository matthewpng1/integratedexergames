using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for returning to the home screen and resetting multiplayer profile data.
/// Can be used in scenes without the MultiplayerProfileSetupController.
/// Attach to a GameObject and call ResetAndReturnHome() from a button or event.
/// </summary>
public class MultiplayerHomeController : MonoBehaviour
{
    public string startMenuSceneName = "StartMenu"; // Scene to load for home

    public void ResetAndReturnHome()
    {
        // Reset multiplayer profile data
        PlayerPrefs.DeleteKey("MultiplayerPlayer1Age");
        PlayerPrefs.DeleteKey("MultiplayerPlayer1Weight");
        PlayerPrefs.DeleteKey("MultiplayerPlayer1Gender");
        PlayerPrefs.DeleteKey("MultiplayerPlayer1Difficulty");
        PlayerPrefs.DeleteKey("MultiplayerPlayer2Age");
        PlayerPrefs.DeleteKey("MultiplayerPlayer2Weight");
        PlayerPrefs.DeleteKey("MultiplayerPlayer2Gender");
        PlayerPrefs.DeleteKey("MultiplayerPlayer2Difficulty");
        PlayerPrefs.DeleteKey("MultiplayerExerciseType");
        PlayerPrefs.Save();

        Debug.Log("MultiplayerHomeController: Multiplayer data reset");

        // Load start menu
        if (!string.IsNullOrEmpty(startMenuSceneName))
        {
            SceneManager.LoadScene(startMenuSceneName);
        }
        else
        {
            Debug.LogError("MultiplayerHomeController: startMenuSceneName not set.");
        }
    }
}