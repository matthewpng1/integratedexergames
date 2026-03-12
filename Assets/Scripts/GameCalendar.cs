using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameCalendar : MonoBehaviour
{
    public GameObject dayPrefab; // Prefab for each calendar box
    public Transform calendarGrid; // Parent where boxes are instantiated
    public GameObject calendarPanel; // UI Panel for the calendar
    public TMP_Text monthHeaderText; // Reference to UI text for month name

    private DateTime currentMonth; // Stores the currently displayed month
    public string highScoreSceneName = "HighScoreScene"; // scene to open when a day is clicked

    void Start()
    {
        currentMonth = DateTime.Now; // Start with the current month
        GenerateCalendar(); // Create calendar when game starts
    }

    void GenerateCalendar()
    {
        // Update the Month Header Text
        if (monthHeaderText != null)
        {
            monthHeaderText.text = currentMonth.ToString("MMMM yyyy"); // Example: "March 2025"
        }

        // Clear previous calendar entries
        foreach (Transform child in calendarGrid)
        {
            Destroy(child.gameObject);
        }

        // Get first and last day of the current month
        int year = currentMonth.Year;
        int month = currentMonth.Month;
        DateTime startDate = new DateTime(year, month, 1);
        int totalDays = DateTime.DaysInMonth(year, month); // Get the correct number of days for the month
        int totalBoxes = 31; // Maximum 31 days in a month

        Debug.Log($"📅 Generating Calendar for {currentMonth:MMMM yyyy}...");

        for (int i = 1; i <= totalBoxes; i++)
        {
            GameObject day = Instantiate(dayPrefab, calendarGrid);
            // Ensure the instantiated day is active (some setups instantiate inactive prefabs)
            day.SetActive(true);
            Image img = day.GetComponent<Image>(); // Get Image component

            // ✅ Ensure Image is enabled
            if (img != null)
            {
                img.enabled = true;
            }

            // ✅ Find or Add TMP_Text Component
            TMP_Text textComponent = day.GetComponentInChildren<TMP_Text>(true);
            if (textComponent == null)
            {
                GameObject textObject = new GameObject("DateText");
                textObject.transform.SetParent(day.transform);
                textObject.transform.localPosition = Vector3.zero; // Center text
                textComponent = textObject.AddComponent<TMP_Text>();

                // Set Text Properties
                textComponent.color = Color.black;
                textComponent.alignment = TextAlignmentOptions.Center;
                textComponent.enableAutoSizing = true;
                textComponent.rectTransform.sizeDelta = new Vector2(100, 100);
            }

            // ✅ Force-enable TMP_Text GameObject
            textComponent.gameObject.SetActive(true);
            textComponent.enabled = true;
            textComponent.fontSize = 15; // ✅ Set font size to 15

            // ✅ Assign Dates & Reps
            if (i <= totalDays)
            {
                DateTime currentDate = new DateTime(year, month, i);
                string dateKey = currentDate.ToString("yyyy-MM-dd"); // Base date key

                // If a user is logged in, use per-user keys: "yyyy-MM-dd_username" and "yyyy-MM-dd_username_grips"
                string userSuffix = null;
                if (UserManager.Instance != null && UserManager.Instance.HasUser())
                {
                    userSuffix = "_" + UserManager.Instance.CurrentUser;
                }

                string prefsKey = dateKey + (userSuffix ?? "");
                int reps = PlayerPrefs.GetInt(prefsKey, 0); // Try per-user (or legacy) reps
                int grips = PlayerPrefs.GetInt(prefsKey + "_grips", 0); // Try per-user (or legacy) grips

                // Fallback to legacy date-only keys if no per-user value exists
                if (!string.IsNullOrEmpty(userSuffix))
                {
                    if (reps == 0)
                        reps = PlayerPrefs.GetInt(dateKey, 0);
                    if (grips == 0)
                        grips = PlayerPrefs.GetInt(dateKey + "_grips", 0);
                }

                // Compute total score with multiplier
                int total = reps + grips;
                if (UserManager.Instance != null && UserManager.Instance.HasUser())
                {
                    string user = UserManager.Instance.CurrentUser;
                    int age = PlayerPrefs.GetInt($"GYG_Profile:{user}:age", 0);
                    float weightF = PlayerPrefs.GetFloat($"GYG_Profile:{user}:weight", 0f);
                    int weight = Mathf.RoundToInt(weightF);
                    string gender = PlayerPrefs.GetString($"GYG_Profile:{user}:gender", "");
                    
                    // Apply multiplier-based scoring
                    float multiplier = 1.0f;
                    if (gender == "Female") multiplier += 0.1f;
                    if (weight > 36) multiplier += 0.1f * (weight - 36);
                    if (age > 60) multiplier += 0.1f * (age - 60);
                    
                    total = (int)(total * multiplier);
                }

                textComponent.text = $"{currentDate:dd}\n{reps} reps\n{grips} grips\nscore: {total}"; // ✅ Display date + reps + grips + total

                // Make the day tile clickable: ensure there's a Button and add listener
                var btn = day.GetComponent<UnityEngine.UI.Button>();
                if (btn == null)
                {
                    btn = day.AddComponent<UnityEngine.UI.Button>();
                }

                // Ensure button GameObject/component is active and interactable
                btn.gameObject.SetActive(true);
                btn.enabled = true;
                btn.interactable = true;

                // If targetGraphic is missing, assign the Image on the same GameObject
                if (btn.targetGraphic == null && img != null)
                    btn.targetGraphic = img;

                string dk = dateKey; // capture for lambda
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnDayClicked(dk));
            }
            else
            {
                textComponent.text = ""; // ✅ Keep extra boxes empty
            }

            // ✅ Highlight today's date
            if (img != null)
            {
                img.color = (i <= totalDays && new DateTime(year, month, i).Date == DateTime.Now.Date) ? Color.yellow : Color.grey;
            }
        }

        Debug.Log($"✅ Calendar for {currentMonth:MMMM yyyy} generated successfully!");
    }

    public void ToggleCalendar()
    {
        calendarPanel.SetActive(!calendarPanel.activeSelf);
    }

    void OnDayClicked(string dateKey)
    {
        Debug.Log($"GameCalendar: day clicked {dateKey}");
        // Ensure HighScoreManager exists
        if (HighScoreManager.Instance == null)
        {
            var go = new GameObject("HighScoreManager");
            go.AddComponent<HighScoreManager>();
            Debug.Log("GameCalendar: Created HighScoreManager at runtime.");
        }

        HighScoreManager.Instance.SetSelectedDateString(dateKey);

        if (!string.IsNullOrEmpty(highScoreSceneName))
        {
            SceneManager.LoadScene(highScoreSceneName);
        }
        else
        {
            Debug.LogWarning("GameCalendar: highScoreSceneName not set.");
        }
    }

    public void UpdateReps(int reps)
    {
        string dateKey = DateTime.Now.ToString("yyyy-MM-dd");
        string prefsKey = dateKey;
        if (UserManager.Instance != null && UserManager.Instance.HasUser())
        {
            prefsKey = dateKey + "_" + UserManager.Instance.CurrentUser;
        }

        int totalReps = PlayerPrefs.GetInt(prefsKey, 0) + reps;
        PlayerPrefs.SetInt(prefsKey, totalReps);
        PlayerPrefs.Save();

        GenerateCalendar(); // Refresh UI after updating reps
    }

    public void NextMonth()
    {
        currentMonth = currentMonth.AddMonths(1); // Move to the next month
        GenerateCalendar(); // Refresh calendar
    }

    public void PreviousMonth()
    {
        currentMonth = currentMonth.AddMonths(-1); // Move to the previous month
        GenerateCalendar(); // Refresh calendar
    }
}
