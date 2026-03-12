using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class GameInstructions : MonoBehaviour
{
    public TextMeshProUGUI instructionText; // Reference to UI text
    public Image instructionImage; // UI Image
    public Sprite[] instructionSprites; // Array of images
    private int step = 0;

    void Start()
    {
        ShowInstruction(); // Display first instruction
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Replace with actual hand gripper input
        {
            NextStep();
        }
    }

    void ShowInstruction()
    {
        switch (step)
        {
            case 0:
                instructionText.text = "STEP 1: Hold onto the hand gripper with either your left or right hand";
                instructionImage.sprite = instructionSprites[0]; // Change image
                break;
            case 1:
                instructionText.text = "STEP 2: Squeeze the hand gripper <color=#A00000>ONCE</color> to jump";
                //instructionImage.sprite = instructionSprites[1]; // Change image
                break;
            case 2:
                instructionText.text = "STEP 3: Squeeze the hand gripper <color=#A00000>TWICE</color> to perform a <color=#A00000>double jump</color>";
                //instructionImage.sprite = instructionSprites[2]; // Change image
                break;
            case 3:
                instructionText.text = "YOUR MISSION: Squeeze the hand gripper to <color=#A00000>avoid pitholes and collect items</color>";
                instructionImage.sprite = instructionSprites[3]; // Change image
                break;
            
            case 4:
                instructionText.text = "TIP: Collect items on <color=#A00000>floating platforms</color> for bonus points!";
                instructionImage.sprite = instructionSprites[4]; // Change image
                break;
            default:
                instructionText.text = "<color=#A00000>ARE YOU READY?</color> Click the arrow below to play";
                instructionImage.enabled = false; // Hide image after instructions
                break;
        }
    }

    void NextStep()
    {
        if (step < instructionSprites.Length)
        {
            step++;
            ShowInstruction();
        }
    }
}
