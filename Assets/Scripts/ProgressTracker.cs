using UnityEngine;
using System;

public class ProgressTracker : MonoBehaviour
{
    public static ProgressTracker Instance;
    private int repsToday;
    private int gripsToday;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadRepsForToday();
        LoadGripsForToday();
    }

    // Call this when a rep is completed
    public void AddRep()
    {
        repsToday++;
        // Persist and update high score immediately
        SaveNow();
    }

    private void SaveRepsForToday()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string username = GetCurrentUsername();
        string key = string.IsNullOrEmpty(username) ? today : $"{today}_{username}";
        PlayerPrefs.SetInt(key, repsToday);
        PlayerPrefs.Save();
        Debug.Log($"ProgressTracker: Saved reps -> key='{key}' reps={repsToday}");
    }

    private void LoadRepsForToday()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string username = GetCurrentUsername();
        string key = string.IsNullOrEmpty(username) ? today : $"{today}_{username}";
        repsToday = PlayerPrefs.GetInt(key, 0);
        Debug.Log($"ProgressTracker: Loaded reps -> key='{key}' reps={repsToday}");
    }

    public int GetRepsToday()
    {
        return repsToday;
    }

    // Call this when a grip is completed
    public void AddGrip()
    {
        gripsToday++;
        // Persist and update high score immediately
        SaveNow();
    }

    private void SaveGripsForToday()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string username = GetCurrentUsername();
        string gripsKey = string.IsNullOrEmpty(username) ? today + "_grips" : $"{today}_{username}_grips";
        PlayerPrefs.SetInt(gripsKey, gripsToday);
        PlayerPrefs.Save();
        Debug.Log($"ProgressTracker: Saved grips -> key='{gripsKey}' grips={gripsToday}");
    }

    private void LoadGripsForToday()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string username = GetCurrentUsername();
        string gripsKey = string.IsNullOrEmpty(username) ? today + "_grips" : $"{today}_{username}_grips";
        gripsToday = PlayerPrefs.GetInt(gripsKey, 0);
        Debug.Log($"ProgressTracker: Loaded grips -> key='{gripsKey}' grips={gripsToday}");
    }

    private string GetCurrentUsername()
    {
        if (UserManager.Instance != null && UserManager.Instance.HasUser())
            return UserManager.Instance.CurrentUser;
        return PlayerPrefs.GetString("GYG_CurrentUser", "");
    }

    // Call this when user changes to reload data for the new user
    public void ReloadForNewUser()
    {
        LoadRepsForToday();
        LoadGripsForToday();
    }

    // General refresh: reload counters from PlayerPrefs (ignores username changes)
    public void Refresh()
    {
        LoadRepsForToday();
        LoadGripsForToday();
        Debug.Log($"ProgressTracker: Refresh called -> reps={repsToday} grips={gripsToday}");
    }

    // Force save both reps and grips to PlayerPrefs immediately.
    public void SaveNow()
    {
        SaveRepsForToday();
        SaveGripsForToday();
        Debug.Log($"ProgressTracker: SaveNow called - reps={repsToday} grips={gripsToday}");
        // also push the current totals into the high score manager
        string username = GetCurrentUsername();
        if (!string.IsNullOrEmpty(username))
        {
            if (HighScoreManager.Instance == null)
            {
                var go = new GameObject("HighScoreManager");
                go.AddComponent<HighScoreManager>();
            }
            HighScoreManager.Instance.SaveScore(username, DateTime.Now, repsToday, gripsToday);
            Debug.Log($"ProgressTracker: pushed to HighScoreManager for user={username} reps={repsToday} grips={gripsToday}");
        }
    }

    public int GetGripsToday()
    {
        return gripsToday;
    }
}
