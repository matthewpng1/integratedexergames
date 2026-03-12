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
    public int total; // reps+grips+age+weight+genderValue

    public HighScoreEntry() { }
    public HighScoreEntry(string user, int r, int g)
    {
        username = user;
        reps = r;
        grips = g;
        total = r + g; // additional components added later
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
        return new HighScoreEntry(username, reps, grips);
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
            var entry = new HighScoreEntry(user, reps, grips);
            ApplyProfileBonus(entry);
            entries.Add(entry);
        }
        // Sort by total score (includes profile bonuses) desc, then reps, then grips
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

    // Read profile data (age, weight, gender) from PlayerPrefs and calculate total with multiplier
    // Score multiplier formula:
    // - Base multiplier = 1.0
    // - If female: +0.1
    // - For each kg above 36kg: +0.1
    // - For each year above 60: +0.1
    // Final total = (reps + grips) * multiplier
    private void ApplyProfileBonus(HighScoreEntry entry)
    {
        if (entry == null || string.IsNullOrEmpty(entry.username)) return;
        string user = entry.username;
        int age = PlayerPrefs.GetInt($"GYG_Profile:{user}:age", 0);
        float weightF = PlayerPrefs.GetFloat($"GYG_Profile:{user}:weight", 0f);
        int weight = Mathf.RoundToInt(weightF);
        string gender = PlayerPrefs.GetString($"GYG_Profile:{user}:gender", "");
        
        // Calculate base score
        int baseScore = entry.reps + entry.grips;
        
        // Calculate multiplier
        float multiplier = 1.0f;
        if (gender == "Female") multiplier += 0.1f;
        if (weight > 36) multiplier += 0.1f * (weight - 36);
        if (age > 60) multiplier += 0.1f * (age - 60);
        
        // Apply multiplier
        entry.total = (int)(baseScore * multiplier);
        
        Debug.Log($"HighScoreManager: ApplyProfileBonus for user={user} reps={entry.reps} grips={entry.grips} gender={gender} age={age} weight={weight} baseScore={baseScore} multiplier={multiplier:F2} -> total={entry.total}");
    }

    public void ClearScoresForDate(DateTime date)
    {
        string dk = DateKey(date);
        string listKey = UsersListKey(dk);
        
        // Load and clear all user scores for this date
        var list = LoadUserList(listKey);
        foreach (var user in list.users)
        {
            PlayerPrefs.DeleteKey(RepsKey(dk, user));
            PlayerPrefs.DeleteKey(GripsKey(dk, user));
        }

        // Clear the user list itself
        PlayerPrefs.DeleteKey(listKey);
        PlayerPrefs.Save();

        Debug.Log($"HighScoreManager: Cleared all scores for {dk}");
    }
}
