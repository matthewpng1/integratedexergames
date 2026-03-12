using UnityEngine;
using TMPro;

/// <summary>
/// Attach to the entry prefab. Provide four TMP_Text fields (username, reps, grips, total) and call `Fill`.
/// </summary>
public class HighScoreEntryRow : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text repsText;
    public TMP_Text gripsText;
    public TMP_Text totalText;

    public void Fill(HighScoreEntry entry)
    {
        if (entry == null) return;
        if (usernameText != null) usernameText.text = entry.username;
        if (repsText != null) repsText.text = entry.reps.ToString();
        if (gripsText != null) gripsText.text = entry.grips.ToString();
        if (totalText != null) totalText.text = entry.total.ToString();
    }

    public void SetEmpty(string message)
    {
        if (usernameText != null) usernameText.text = message;
        if (repsText != null) repsText.text = "-";
        if (gripsText != null) gripsText.text = "-";
        if (totalText != null) totalText.text = "-";
    }
}
