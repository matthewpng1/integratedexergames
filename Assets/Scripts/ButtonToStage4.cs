using UnityEngine;
using UnityEngine.SceneManagement; // Import for scene management

public class ButtonToStage4 : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void GoToStage4Countries()
    {
        SceneManager.LoadScene("prequicktime 4"); // Ensure this matches the exact name of your scene in Build Settings
    }
}
