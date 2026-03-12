using System;
using UnityEngine;

/// <summary>
/// Utility script to clear high-score entries for a specific date.
/// Attach to any GameObject or button and call ClearTodayScores() via button OnClick.
/// </summary>
public class HighScoreCleaner : MonoBehaviour
{
    public void ClearTodayScores()
    {
        if (HighScoreManager.Instance == null)
        {
            var go = new GameObject("HighScoreManager");
            go.AddComponent<HighScoreManager>();
        }

        HighScoreManager.Instance.ClearScoresForDate(DateTime.Now);
        Debug.Log("HighScoreCleaner: Cleared scores for today.");
    }

    public void ClearScoresForDate(string dateString) // e.g., "2026-02-19"
    {
        DateTime date;
        if (!DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
        {
            Debug.LogError($"HighScoreCleaner: Invalid date format '{dateString}'. Use yyyy-MM-dd.");
            return;
        }

        if (HighScoreManager.Instance == null)
        {
            var go = new GameObject("HighScoreManager");
            go.AddComponent<HighScoreManager>();
        }

        HighScoreManager.Instance.ClearScoresForDate(date);
        Debug.Log($"HighScoreCleaner: Cleared scores for {dateString}.");
    }

    public void ClearAllScores()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("HighScoreCleaner: Cleared ALL PlayerPrefs data.");
    }
}
