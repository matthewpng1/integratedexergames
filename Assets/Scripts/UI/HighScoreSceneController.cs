using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach to a GameObject in the HighScore scene.
/// Assign `dateHeader` (TMP_Text), `listContainer` (Transform with VerticalLayoutGroup/Content),
/// and `entryPrefab` (a prefab with `HighScoreEntryRow` component).
/// On Start it reads `HighScoreManager.Instance.GetSelectedDateString()` and populates rows.
/// </summary>
public class HighScoreSceneController : MonoBehaviour
{
    public TMP_Text dateHeader;
    public Transform listContainer;
    public GameObject entryPrefab;
    public string backSceneName = "GameScene";

    void Start()
    {
        if (HighScoreManager.Instance == null)
        {
            Debug.LogWarning("HighScoreSceneController: HighScoreManager not found.");
            return;
        }

        string dateKey = HighScoreManager.Instance.GetSelectedDateString();
        if (string.IsNullOrEmpty(dateKey))
        {
            dateKey = DateTime.Now.ToString("yyyy-MM-dd");
            Debug.LogWarning("HighScoreSceneController: no selected date, defaulting to today: " + dateKey);
        }

        DateTime date;
        if (!DateTime.TryParse(dateKey, out date))
        {
            // Try parsing yyyy-MM-dd explicitly
            try { date = DateTime.ParseExact(dateKey, "yyyy-MM-dd", null); }
            catch { date = DateTime.Now; }
        }

        if (dateHeader != null)
            dateHeader.text = date.ToString("MMMM dd, yyyy");

        PopulateList(date);
    }

    void PopulateList(DateTime date)
    {
        if (entryPrefab == null || listContainer == null)
        {
            Debug.LogWarning("HighScoreSceneController: entryPrefab or listContainer not assigned.");
            return;
        }

        // Clear previous
        for (int i = listContainer.childCount - 1; i >= 0; i--)
            Destroy(listContainer.GetChild(i).gameObject);

        var entries = HighScoreManager.Instance.GetTopScoresForDate(date, 10);
        if (entries == null || entries.Count == 0)
        {
            // Optionally show a placeholder row
            var go = Instantiate(entryPrefab, listContainer);
            var row = go.GetComponent<HighScoreEntryRow>();
            if (row != null) row.SetEmpty("No scores for this date");
            return;
        }

        foreach (var e in entries)
        {
            var go = Instantiate(entryPrefab, listContainer);
            var row = go.GetComponent<HighScoreEntryRow>();
            if (row != null) row.Fill(e);
            else
            {
                // Fallback: try to find TMP children
                var texts = go.GetComponentsInChildren<TMP_Text>();
                if (texts.Length >= 3)
                {
                    texts[0].text = e.username;
                    texts[1].text = e.reps.ToString();
                    texts[2].text = e.grips.ToString();
                }
            }
        }
    }

    public void OnBackPressed()
    {
        if (!string.IsNullOrEmpty(backSceneName))
            SceneManager.LoadScene(backSceneName);
    }
}
