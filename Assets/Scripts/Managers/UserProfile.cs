using UnityEngine;

/// <summary>
/// Singleton to manage user profile data (age, weight, gender) per user.
/// Stores data in PlayerPrefs under keys like "GYG_Profile:{username}:age".
/// Integrates with UserManager for current user.
/// </summary>
public class UserProfile : MonoBehaviour
{
    public static UserProfile Instance { get; private set; }

    private const string ProfilePrefix = "GYG_Profile:";

    // Profile data
    public int Age { get; private set; } = 0;
    public float Weight { get; private set; } = 0f; // in kg
    public string Gender { get; private set; } = ""; // e.g., "Male", "Female", "Other"

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProfile();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Set profile data for the current user
    public void SetProfile(int age, float weight, string gender)
    {
        Age = age;
        Weight = weight;
        Gender = gender;
        SaveProfile();
        Debug.Log($"UserProfile: Set profile for current user - Age: {age}, Weight: {weight}kg, Gender: {gender}");
    }

    // Load profile from PlayerPrefs for the current user
    public void LoadProfile()
    {
        string username = GetCurrentUsername();
        if (string.IsNullOrEmpty(username)) return;

        Age = PlayerPrefs.GetInt(ProfileKey(username, "age"), 0);
        Weight = PlayerPrefs.GetFloat(ProfileKey(username, "weight"), 0f);
        Gender = PlayerPrefs.GetString(ProfileKey(username, "gender"), "");
        Debug.Log($"UserProfile: Loaded profile for '{username}' - Age: {Age}, Weight: {Weight}kg, Gender: {Gender}");
    }

    // Save profile to PlayerPrefs for the current user
    private void SaveProfile()
    {
        string username = GetCurrentUsername();
        if (string.IsNullOrEmpty(username)) return;

        PlayerPrefs.SetInt(ProfileKey(username, "age"), Age);
        PlayerPrefs.SetFloat(ProfileKey(username, "weight"), Weight);
        PlayerPrefs.SetString(ProfileKey(username, "gender"), Gender);
        PlayerPrefs.Save();
        Debug.Log($"UserProfile: Saved profile for '{username}'");
    }

    // Clear profile for the current user (e.g., on logout)
    public void ClearProfile()
    {
        string username = GetCurrentUsername();
        if (string.IsNullOrEmpty(username)) return;

        PlayerPrefs.DeleteKey(ProfileKey(username, "age"));
        PlayerPrefs.DeleteKey(ProfileKey(username, "weight"));
        PlayerPrefs.DeleteKey(ProfileKey(username, "gender"));
        PlayerPrefs.Save();
        Age = 0;
        Weight = 0f;
        Gender = "";
        Debug.Log($"UserProfile: Cleared profile for '{username}'");
    }

    // Helper to get current username
    private string GetCurrentUsername()
    {
        if (UserManager.Instance != null && UserManager.Instance.HasUser())
            return UserManager.Instance.CurrentUser;
        return PlayerPrefs.GetString("GYG_CurrentUser", "");
    }

    // Helper to build PlayerPrefs key
    private string ProfileKey(string username, string field) => $"{ProfilePrefix}{username}:{field}";
}