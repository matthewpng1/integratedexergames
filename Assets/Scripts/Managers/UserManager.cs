using UnityEngine;

/// <summary>
/// Simple singleton to manage the current player username across scenes.
/// Stores the current username in PlayerPrefs under key "GYG_CurrentUser".
/// </summary>
public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }
    private const string PrefKey = "GYG_CurrentUser";

    public string CurrentUser { get; private set; } = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUser();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentUser(string username)
    {
        if (string.IsNullOrEmpty(username)) return;
        CurrentUser = username;
        PlayerPrefs.SetString(PrefKey, username);
        PlayerPrefs.Save();
        Debug.Log($"UserManager: current user set to '{username}'");
    }

    public void LoadUser()
    {
        CurrentUser = PlayerPrefs.GetString(PrefKey, "");
        if (!string.IsNullOrEmpty(CurrentUser))
            Debug.Log($"UserManager: loaded current user '{CurrentUser}'");
    }

    public void Logout()
    {
        Debug.Log($"UserManager: logging out user '{CurrentUser}'");
        CurrentUser = "";
        PlayerPrefs.DeleteKey(PrefKey);
        PlayerPrefs.Save();
    }

    public bool HasUser()
    {
        return !string.IsNullOrEmpty(CurrentUser);
    }
}
