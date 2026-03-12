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
    public TMP_InputField ageInput;
    public TMP_InputField weightInput; // Note: user mentioned "resistance/weight", assuming weight
    public TMP_Dropdown genderDropdown; // Changed to dropdown for multiple choice
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
        if (UserProfile.Instance != null)
        {
            ageInput.text = UserProfile.Instance.Age > 0 ? UserProfile.Instance.Age.ToString() : "";
            weightInput.text = UserProfile.Instance.Weight > 0 ? UserProfile.Instance.Weight.ToString() : "";
            // Set dropdown based on gender
            if (genderDropdown != null)
            {
                int index = 1; // Default to Female
                if (UserProfile.Instance.Gender == "Male") index = 0;
                else if (UserProfile.Instance.Gender == "Female") index = 1;
                genderDropdown.value = index;
            }
        }
    }

    public void OnSaveProfile()
    {
        Debug.Log("OnSaveProfile: Button clicked");

        // Validate inputs
        if (string.IsNullOrEmpty(ageInput.text) || string.IsNullOrEmpty(weightInput.text))
        {
            Debug.LogWarning("ProfileSetup: Age and weight must be filled.");
            return;
        }

        int age;
        float weight;
        if (!int.TryParse(ageInput.text, out age) || age <= 0)
        {
            Debug.LogWarning("ProfileSetup: Age must be a positive integer.");
            return;
        }
        if (!float.TryParse(weightInput.text, out weight) || weight <= 0)
        {
            Debug.LogWarning("ProfileSetup: Weight must be a positive number.");
            return;
        }

        // Get gender from dropdown (always selected)
        string gender = genderDropdown.options[genderDropdown.value].text;
        Debug.Log($"OnSaveProfile: Saving age={age}, weight={weight}, gender={gender}");

        // Save profile
        UserProfile.Instance.SetProfile(age, weight, gender);

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

    public void TestSaveButton()
    {
        Debug.Log("TestSaveButton: Called from OnClick");
    }
}