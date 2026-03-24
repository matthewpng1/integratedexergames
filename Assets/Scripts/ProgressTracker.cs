using UnityEngine;
using System;

public class ProgressTracker : MonoBehaviour
{
    public static ProgressTracker Instance;
    public bool IsTracking = true;
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

    // Call this when a rep is completed (ankle weight exercise)
    public void AddRep()
    {
        if (!IsTracking) return;
        repsToday++;
        // NOTE: Bonus is NO LONGER applied here - it's applied when LevelComplete is called
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

    // Call this when a grip is completed (gripper exercise)
    public void AddGrip()
    {
        if (!IsTracking) return;
        gripsToday++;
        // NOTE: Bonus is NO LONGER applied here - it's applied when LevelComplete is called
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
        IsTracking = true;
        LoadRepsForToday();
        LoadGripsForToday();
        
        // ✅ FIXED: Initialize bonus round counters ONLY if they don't already exist
        string username = GetCurrentUsername();
        if (!string.IsNullOrEmpty(username))
        {
            if (!PlayerPrefs.HasKey($"GYG_Progress:{username}:gripperRounds"))
            {
                PlayerPrefs.SetInt($"GYG_Progress:{username}:gripperRounds", 0);
                PlayerPrefs.SetInt($"GYG_Progress:{username}:ankleRounds", 0);
                PlayerPrefs.Save();
                Debug.Log($"ProgressTracker: Initialized bonus rounds for new user {username}");
            }
            else
            {
                Debug.Log($"ProgressTracker: Bonus rounds already exist for user {username}, not resetting");
            }
        }
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

    // Called when a level is completed (victory zone reached or mini-game ended)
    // This applies bonuses based on which exercises were used in the level
    public void LevelComplete(bool usedGripper = true, bool usedAnkle = false)
    {
        if (!IsTracking) return;
        
        string username = GetCurrentUsername();
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("ProgressTracker: LevelComplete called but no username available");
            return;
        }

        Debug.Log($"ProgressTracker: LEVEL COMPLETE - usedGripper={usedGripper}, usedAnkle={usedAnkle}, username={username}");

        // Apply gripper bonus if the level used grippers
        if (usedGripper)
        {
            int totalBefore = SinglePlayerScoring.GetTotalBonusEarned(username);
            SinglePlayerScoring.CompleteGripperRound(username);
            int totalAfter = SinglePlayerScoring.GetTotalBonusEarned(username);
            Debug.Log($"ProgressTracker: Gripper bonus applied. Total bonus: {totalBefore} -> {totalAfter}");
        }

        // Apply ankle bonus if the level used ankle weights
        if (usedAnkle)
        {
            int totalBefore = SinglePlayerScoring.GetTotalBonusEarned(username);
            SinglePlayerScoring.CompleteAnkleRound(username);
            int totalAfter = SinglePlayerScoring.GetTotalBonusEarned(username);
            Debug.Log($"ProgressTracker: Ankle bonus applied. Total bonus: {totalBefore} -> {totalAfter}");
        }

        // Ensure HighScoreManager is updated with new bonus
        if (HighScoreManager.Instance == null)
        {
            var go = new GameObject("HighScoreManager");
            go.AddComponent<HighScoreManager>();
        }
        HighScoreManager.Instance.SaveScore(username, DateTime.Now, repsToday, gripsToday);
        Debug.Log($"ProgressTracker: HighScoreManager updated for {username} on {DateTime.Now:yyyy-MM-dd} with reps={repsToday}, grips={gripsToday}");
    }
}
