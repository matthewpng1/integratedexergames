using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for the Multiplayer ProfileSetup scene.
/// Handles input validation for two players, saving profile data, and navigation.
/// Attach to a GameObject in the MultiplayerProfileSetup scene.
/// </summary>
public class MultiplayerProfileSetupController : MonoBehaviour
{
    // Player 1 fields
    public TMP_Dropdown genderDropdown1;
    public TMP_Dropdown difficultyDropdown1;
    public TMP_Dropdown exerciseTypeDropdown1;

    // Player 2 fields
    public TMP_Dropdown genderDropdown2;
    public TMP_Dropdown difficultyDropdown2;

    public Button saveProfileButton;
    public Button homeButton;

    public string nextSceneName = "MultiplayerGameScene"; // Scene to load after saving profile
    public string startMenuSceneName = "StartMenu"; // Scene for home

    void Start()
    {
        // Load existing multiplayer profile if available
        LoadExistingMultiplayerProfile();

        Debug.Log("MultiplayerProfileSetup: Start() completed");
    }

    void LoadExistingMultiplayerProfile()
    {
        // Load from PlayerPrefs
        int gender1 = PlayerPrefs.GetInt("MultiplayerPlayer1Gender", 1); // Default Female
        genderDropdown1.value = gender1;
        int difficulty1 = PlayerPrefs.GetInt("MultiplayerPlayer1Difficulty", 0); // Default Easy
        if (difficultyDropdown1 != null)
            difficultyDropdown1.value = difficulty1;
        int exerciseType = PlayerPrefs.GetInt("MultiplayerExerciseType", 0); // Default Gripper
        if (exerciseTypeDropdown1 != null)
            exerciseTypeDropdown1.value = exerciseType;

        int gender2 = PlayerPrefs.GetInt("MultiplayerPlayer2Gender", 1); // Default Female
        genderDropdown2.value = gender2;
        int difficulty2 = PlayerPrefs.GetInt("MultiplayerPlayer2Difficulty", 0); // Default Easy
        if (difficultyDropdown2 != null)
            difficultyDropdown2.value = difficulty2;
    }

    public void OnSaveProfile()
    {
        Debug.Log("OnSaveProfile: Button clicked");

        // Validate all inputs - genders and difficulties are always selected
        // No validation needed as dropdowns have defaults

        // Get genders
        string gender1 = genderDropdown1.options[genderDropdown1.value].text;
        string gender2 = genderDropdown2.options[genderDropdown2.value].text;

        // Get difficulties
        int difficulty1 = difficultyDropdown1 != null ? difficultyDropdown1.value : 0;
        int difficulty2 = difficultyDropdown2 != null ? difficultyDropdown2.value : 0;

        // Get exercise type
        int exerciseType = exerciseTypeDropdown1 != null ? exerciseTypeDropdown1.value : 0;

        Debug.Log($"OnSaveProfile: Saving Player1: gender={gender1}, difficulty={difficulty1}; " +
                  $"Player2: gender={gender2}, difficulty={difficulty2}; exerciseType={exerciseType}");

        // Save to PlayerPrefs
        PlayerPrefs.SetInt("MultiplayerPlayer1Gender", genderDropdown1.value);
        PlayerPrefs.SetInt("MultiplayerPlayer1Difficulty", difficulty1);

        PlayerPrefs.SetInt("MultiplayerPlayer2Gender", genderDropdown2.value);
        PlayerPrefs.SetInt("MultiplayerPlayer2Difficulty", difficulty2);

        PlayerPrefs.SetInt("MultiplayerExerciseType", exerciseType);

        PlayerPrefs.Save();

        // Load next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("MultiplayerProfileSetup: nextSceneName not set.");
        }
    }

    public void OnHome()
    {
        // Reset all fields
        genderDropdown1.value = 1; // Default Female
        genderDropdown2.value = 1; // Default Female
        if (difficultyDropdown1 != null)
            difficultyDropdown1.value = 0; // Default Easy
        if (difficultyDropdown2 != null)
            difficultyDropdown2.value = 0; // Default Easy
        if (exerciseTypeDropdown1 != null)
            exerciseTypeDropdown1.value = 0; // Default Gripper
        PlayerPrefs.SetInt("MultiplayerExerciseType", 0);

        // Load start menu
        if (!string.IsNullOrEmpty(startMenuSceneName))
        {
            SceneManager.LoadScene(startMenuSceneName);
        }
        else
        {
            Debug.LogError("MultiplayerProfileSetup: startMenuSceneName not set.");
        }
    }
}