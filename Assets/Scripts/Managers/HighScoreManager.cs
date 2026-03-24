using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HighScoreEntry
{
    public string username;
    public int reps;
    public int grips;
    public int repBonus; // Bonus applied to reps (e.g., ankle rounds)
    public int gripBonus; // Bonus applied to grips (e.g., gripper rounds)
    public int total; // reps + grips + repBonus + gripBonus

    public HighScoreEntry() { }
    public HighScoreEntry(string user, int r, int g, int repB = 0, int gripB = 0)
    {
        username = user;
        reps = r;
        grips = g;
        repBonus = repB;
        gripBonus = gripB;
        total = r + g + repB + gripB;
    }
}

[Serializable]
class UserListWrapper
{
    public List<string> users = new List<string>();
}

/// <summary>
/// Manages saving and loading per-date, per-user reps and grips using PlayerPrefs.
/// Keys used:
/// - "GYG_HS:{date}:{username}:reps"
/// - "GYG_HS:{date}:{username}:grips"
/// - "GYG_HS:users:{date}" -> JSON list of usernames
/// </summary>
public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }
    const string ScorePrefix = "GYG_HS:";
    const string UserListPrefix = "GYG_HS:users:";
    // Selected date key (yyyy-MM-dd) chosen by calendar tile click
    private string selectedDateKey = "";

    public void SetSelectedDate(DateTime date)
    {
        selectedDateKey = DateKey(date);
    }

    public void SetSelectedDateString(string dateKey)
    {
        selectedDateKey = dateKey;
    }

    public string GetSelectedDateString()
    {
        return selectedDateKey;
    }

    void Awake()
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
    }

    string DateKey(DateTime date) => date.ToString("yyyy-MM-dd");

    string RepsKey(string dateKey, string username) => $"{ScorePrefix}{dateKey}:{username}:reps";
    string GripsKey(string dateKey, string username) => $"{ScorePrefix}{dateKey}:{username}:grips";
    string UsersListKey(string dateKey) => $"{UserListPrefix}{dateKey}";

    UserListWrapper LoadUserList(string listKey)
    {
        string json = PlayerPrefs.GetString(listKey, "");
        if (string.IsNullOrEmpty(json)) return new UserListWrapper();
        try
        {
            return JsonUtility.FromJson<UserListWrapper>(json) ?? new UserListWrapper();
        }
        catch
        {
            return new UserListWrapper();
        }
    }

    void SaveUserList(string listKey, UserListWrapper list)
    {
        string json = JsonUtility.ToJson(list);
        PlayerPrefs.SetString(listKey, json);
        PlayerPrefs.Save();
    }

    public void SaveScore(string username, DateTime date, int reps, int grips)
    {
        if (string.IsNullOrEmpty(username)) return;
        string dk = DateKey(date);
        int prevReps = PlayerPrefs.GetInt(RepsKey(dk, username), -1);
        int prevGrips = PlayerPrefs.GetInt(GripsKey(dk, username), -1);
        PlayerPrefs.SetInt(RepsKey(dk, username), reps);
        PlayerPrefs.SetInt(GripsKey(dk, username), grips);
        PlayerPrefs.Save();
        Debug.Log($"HighScoreManager: SaveScore date={dk} user={username} prevReps={prevReps} prevGrips={prevGrips} -> reps={reps} grips={grips}");

        string listKey = UsersListKey(dk);
        var list = LoadUserList(listKey);
        if (!list.users.Contains(username))
        {
            list.users.Add(username);
            SaveUserList(listKey, list);
            Debug.Log($"HighScoreManager: Added user '{username}' to list for {dk}");
        }
    }

    public HighScoreEntry GetScoreForUser(string username, DateTime date)
    {
        if (string.IsNullOrEmpty(username)) return null;
        string dk = DateKey(date);
        int reps = PlayerPrefs.GetInt(RepsKey(dk, username), 0);
        int grips = PlayerPrefs.GetInt(GripsKey(dk, username), 0);

        // Split bonus into rep and grip components for clearer display.
        int repBonus = SinglePlayerScoring.GetTotalAnkleBonusEarned(username);
        int gripBonus = SinglePlayerScoring.GetTotalGripperBonusEarned(username);

        return new HighScoreEntry(username, reps, grips, repBonus, gripBonus);
    }

    public List<HighScoreEntry> GetScoresForDate(DateTime date)
    {
        string dk = DateKey(date);
        string listKey = UsersListKey(dk);
        var list = LoadUserList(listKey);
        var entries = new List<HighScoreEntry>();
        foreach (var user in list.users)
        {
            int reps = PlayerPrefs.GetInt(RepsKey(dk, user), 0);
            int grips = PlayerPrefs.GetInt(GripsKey(dk, user), 0);

            int repBonus = SinglePlayerScoring.GetTotalAnkleBonusEarned(user);
            int gripBonus = SinglePlayerScoring.GetTotalGripperBonusEarned(user);

            var entry = new HighScoreEntry(user, reps, grips, repBonus, gripBonus);
            entries.Add(entry);
        }
        // Sort by total score desc, then reps, then grips
        entries.Sort((a, b) =>
        {
            int cmp = b.total.CompareTo(a.total);
            if (cmp != 0) return cmp;
            cmp = b.reps.CompareTo(a.reps);
            if (cmp != 0) return cmp;
            return b.grips.CompareTo(a.grips);
        });
        return entries;
    }

    public List<HighScoreEntry> GetTopScoresForDate(DateTime date, int topN = 10)
    {
        var all = GetScoresForDate(date);
        // already sorted by total in GetScoresForDate(), so no additional sort needed
        // but ensure we return topN entries

        if (all.Count <= topN) return all;
        return all.GetRange(0, topN);
    }

    // Helper: returns the date string used as key
    public string GetDateStringKey(DateTime date) => DateKey(date);

    public void ClearScoresForDate(DateTime date)
    {
        string dk = DateKey(date);
        string listKey = UsersListKey(dk);

        // Clear legacy (date-only) keys so old values don't persist
        PlayerPrefs.DeleteKey(dk);
        PlayerPrefs.DeleteKey(dk + "_grips");

        // Load and clear all per-user scores for this date
        var list = LoadUserList(listKey);
        foreach (var user in list.users)
        {
            PlayerPrefs.DeleteKey(RepsKey(dk, user));
            PlayerPrefs.DeleteKey(GripsKey(dk, user));
        }

        // Clear the user list itself
        PlayerPrefs.DeleteKey(listKey);
        PlayerPrefs.Save();

        Debug.Log($"HighScoreManager: Cleared all scores for {dk} (including legacy keys)");
    }
}
