using UnityEngine;
using TMPro;

public class Username : MonoBehaviour
{
    public TMP_Text usernameText;

    void OnEnable()
    {
        UpdateUsername();
    }

    void Start()
    {
        if (usernameText == null) usernameText = GetComponent<TMP_Text>();
        UpdateUsername();
    }

    public void UpdateUsername()
    {
        string user = "";
        if (UserManager.Instance != null && UserManager.Instance.HasUser())
        {
            user = UserManager.Instance.CurrentUser;
        }
        else
        {
            user = PlayerPrefs.GetString("GYG_CurrentUser", "Guest");
        }

        if (usernameText != null)
            usernameText.text = string.IsNullOrEmpty(user) ? "Guest" : user;
    }
}
