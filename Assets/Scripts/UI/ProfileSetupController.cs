using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for the ProfileSetup scene.
/// Handles input validation, saving profile data, and navigation.
/// Attach to a GameObject in the ProfileSetup scene.
/// </summary>
public class ProfileSetupController : MonoBehaviour
{
    public TMP_Dropdown genderDropdown;
    public TMP_Dropdown gripperDifficultyDropdown; // Gripper difficulty
    public TMP_Dropdown ankleDifficultyDropdown; // Ankle difficulty
    public Button saveProfileButton;
    public Button logoutButton;

    public string nextSceneName = "GameScene"; // Scene to load after saving profile
    public string startMenuSceneName = "StartMenu"; // Scene for logout

    void Start()
    {
        // Ensure UserManager and UserProfile exist
        if (UserManager.Instance == null)
        {
            var go = new GameObject("UserManager");
            go.AddComponent<UserManager>();
        }
        if (UserProfile.Instance == null)
        {
            var go = new GameObject("UserProfile");
            go.AddComponent<UserProfile>();
        }

        // Load existing profile if available
        LoadExistingProfile();

        Debug.Log("ProfileSetup: Start() completed");
    }

    void LoadExistingProfile()
    {
        // Set gender dropdown based on existing profile
        if (genderDropdown != null)
        {
            int index = 1; // Default to Female
            if (UserProfile.Instance != null)
            {
                if (UserProfile.Instance.Gender == "Male") index = 0;
                else if (UserProfile.Instance.Gender == "Female") index = 1;
            }
            genderDropdown.value = index;
        }

        // Load difficulty settings from PlayerPrefs
        string username = UserManager.Instance != null && UserManager.Instance.HasUser() ? UserManager.Instance.CurrentUser : "";
        if (!string.IsNullOrEmpty(username))
        {
            int gripperDifficulty = PlayerPrefs.GetInt($"GYG_Profile:{username}:gripperDifficulty", 0);
            int ankleDifficulty = PlayerPrefs.GetInt($"GYG_Profile:{username}:ankleDifficulty", 0);
            if (gripperDifficultyDropdown != null)
                gripperDifficultyDropdown.value = gripperDifficulty;
            if (ankleDifficultyDropdown != null)
                ankleDifficultyDropdown.value = ankleDifficulty;
        }
    }

    public void OnSaveProfile()
    {
        Debug.Log("OnSaveProfile: Button clicked");

        // Get values from dropdowns (always have defaults)
        string gender = genderDropdown.options[genderDropdown.value].text;
        int gripperDifficulty = gripperDifficultyDropdown != null ? gripperDifficultyDropdown.value : 0;
        int ankleDifficulty = ankleDifficultyDropdown != null ? ankleDifficultyDropdown.value : 0;

        Debug.Log($"OnSaveProfile: Saving gender={gender}, gripperDifficulty={gripperDifficulty}, ankleDifficulty={ankleDifficulty}");

        // Save profile (age set to 0 as it's no longer used for scoring)
        UserProfile.Instance.SetProfile(0, 0, gender);

        // Save difficulty settings to PlayerPrefs
        string username = UserManager.Instance != null && UserManager.Instance.HasUser() ? UserManager.Instance.CurrentUser : "";
        if (!string.IsNullOrEmpty(username))
        {
            PlayerPrefs.SetInt($"GYG_Profile:{username}:gripperDifficulty", gripperDifficulty);
            PlayerPrefs.SetInt($"GYG_Profile:{username}:ankleDifficulty", ankleDifficulty);
            
            // ✅ FIXED: Only initialize round counts if they don't already exist (don't reset on re-save!)
            if (!PlayerPrefs.HasKey($"GYG_Progress:{username}:gripperRounds"))
            {
                PlayerPrefs.SetInt($"GYG_Progress:{username}:gripperRounds", 0);
                Debug.Log($"ProfileSetupController: Initialized gripperRounds for {username}");
            }
            if (!PlayerPrefs.HasKey($"GYG_Progress:{username}:ankleRounds"))
            {
                PlayerPrefs.SetInt($"GYG_Progress:{username}:ankleRounds", 0);
                Debug.Log($"ProfileSetupController: Initialized ankleRounds for {username}");
            }
            
            PlayerPrefs.Save();
            Debug.Log($"ProfileSetupController: Saved profile for {username}");
        }

        // Load next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("ProfileSetup: nextSceneName not set.");
        }
    }

    public void OnLogout()
    {
        // Clear user and profile
        if (UserManager.Instance != null)
            UserManager.Instance.Logout();
        if (UserProfile.Instance != null)
            UserProfile.Instance.ClearProfile();

        // Clear difficulty and progress settings from PlayerPrefs
        string username = UserManager.Instance != null && UserManager.Instance.HasUser() ? UserManager.Instance.CurrentUser : "";
        if (!string.IsNullOrEmpty(username))
        {
            PlayerPrefs.DeleteKey($"GYG_Profile:{username}:gripperDifficulty");
            PlayerPrefs.DeleteKey($"GYG_Profile:{username}:ankleDifficulty");
            PlayerPrefs.DeleteKey($"GYG_Progress:{username}:gripperRounds");
            PlayerPrefs.DeleteKey($"GYG_Progress:{username}:ankleRounds");
            PlayerPrefs.Save();
        }

        // Load start menu
        if (!string.IsNullOrEmpty(startMenuSceneName))
        {
            SceneManager.LoadScene(startMenuSceneName);
        }
        else
        {
            Debug.LogError("ProfileSetup: startMenuSceneName not set.");
        }
    }
}