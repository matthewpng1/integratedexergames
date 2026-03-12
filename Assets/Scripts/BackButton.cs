using UnityEngine;
using UnityEngine.SceneManagement; // Import for scene management

public class BackButton : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void GoBack()
    {
        SceneManager.LoadScene("Menu"); // Ensure this matches the exact name of your scene in Build Settings
    }
}
