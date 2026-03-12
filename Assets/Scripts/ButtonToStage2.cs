using UnityEngine;
using UnityEngine.SceneManagement; // Import for scene management

public class ButtonToStage2 : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void GoToStage2Countries()
    {
        SceneManager.LoadScene("prequicktime 2"); // Ensure this matches the exact name of your scene in Build Settings
    }
}
