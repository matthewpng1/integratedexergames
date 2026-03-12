using UnityEngine;
using UnityEngine.SceneManagement; // Import for scene management

public class ButtonToStage3 : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void GoToStage3Countries()
    {
        SceneManager.LoadScene("prequicktime 3"); // Ensure this matches the exact name of your scene in Build Settings
    }
}
