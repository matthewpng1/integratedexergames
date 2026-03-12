using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInitializer : MonoBehaviour
{
    void Start()
    {
        Slider slider = GameObject.Find("CollectibleProgressBar").GetComponent<Slider>();
        TMP_Text text = GameObject.Find("ProgressText").GetComponent<TMP_Text>();

        if (slider != null && text != null)
        {
            CollectibleManager.Instance.RegisterUI(slider, text);
            Debug.Log("UIInitializer: Registered UI successfully.");
        }
        else
        {
            Debug.LogError("UIInitializer: Could not find UI elements.");
        }
    }
}
