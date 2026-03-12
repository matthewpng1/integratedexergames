using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach this to the Start Menu Canvas. Wire the `nameInput` and the Start button's OnClick to `OnStartPressed()`.
/// The Start button will set the current user via `UserManager` and load the ProfileSetup scene.
/// A Logout button can be wired to `OnLogoutPressed()` to clear the saved user.
/// </summary>
public class StartMenuController : MonoBehaviour
{
    public TMP_InputField nameInput;
    public string profileSetupSceneName = "ProfileSetupScene"; // Scene for profile setup
    public GameObject startPanel; // optional panel to hide/show

    void Start()
    {
        // If a user already exists, prefill the input
        if (UserManager.Instance != null && UserManager.Instance.HasUser())
        {
            if (nameInput != null)
                nameInput.text = UserManager.Instance.CurrentUser;
        }
    }

    public void OnStartPressed()
    {
        if (nameInput == null)
        {
            Debug.LogWarning("StartMenuController: nameInput not set.");
            return;
        }

        string username = nameInput.text.Trim();
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("StartMenuController: username is empty. Please enter a name.");
            return;
        }

        // Ensure a UserManager exists (either placed in scene or created at runtime)
        if (UserManager.Instance == null)
        {
            var go = new GameObject("UserManager");
            go.AddComponent<UserManager>();
            Debug.Log("StartMenuController: Created UserManager at runtime.");
        }
        UserManager.Instance.SetCurrentUser(username);

        // Reload ProgressTracker data for the new user
        if (ProgressTracker.Instance != null)
        {
            ProgressTracker.Instance.ReloadForNewUser();
        }

        if (!string.IsNullOrEmpty(profileSetupSceneName))
        {
            SceneManager.LoadScene(profileSetupSceneName);
        }
        else
        {
            Debug.LogWarning("StartMenuController: profileSetupSceneName not configured.");
        }
    }

    public void OnLogoutPressed()
    {
        if (UserManager.Instance != null)
        {
            UserManager.Instance.Logout();
        }

        if (nameInput != null)
            nameInput.text = string.Empty;
    }
}
