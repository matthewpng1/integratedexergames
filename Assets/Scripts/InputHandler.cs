using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class InputHandler : MonoBehaviour
{
    public Spawner spawner; // Reference to the Spawner script
    public int score = 0; // Player's score
    public float dropSpeed = 10f; // Speed at which the object drops
    public float destroyY = -10f; // Y position below which the object is destroyed
    public TextMeshProUGUI scoreText; // UI text to display the score
    public TextMeshProUGUI timerText; // UI text to display the timer
    public GameObject endGameUI; // End game UI panel
    public TextMeshProUGUI finalScoreText; // Text to display final score in end UI
    public float gameTime = 30f; // Total game time in seconds
    public string nextSceneName = "MainMenu"; // Name of the scene to load after game ends
    public AudioSource audioSource; // Audio source to stop when game ends
    public AudioClip destroySound; // Sound to play when object is destroyed
    public GameObject destroyParticleEffect; // Particle effect prefab to instantiate on destroy
    public int maxButtonPresses = 15; // Maximum button presses before game ends
    
    private float timeLeft;
    private bool gameRunning = true;
    private bool waitingForInput = false;
    private int buttonPressCount = 0; // Counter for button presses

    void Start()
    {
        // Ensure the game isn't paused when this scene loads
        Time.timeScale = 1f;

        timeLeft = gameTime;
        if (endGameUI != null) endGameUI.SetActive(false);

        // Refresh the progress tracker in case another scene modified it
        if (ProgressTracker.Instance != null)
        {
            ProgressTracker.Instance.Refresh();
            Debug.Log($"InputHandler: Start mini-game; tracker reps={ProgressTracker.Instance.GetRepsToday()} grips={ProgressTracker.Instance.GetGripsToday()}");
        }

        // Validate spawner reference; try to find one in the scene if not set in inspector
        if (spawner == null)
        {
            spawner = FindObjectOfType<Spawner>();
            if (spawner == null)
            {
                Debug.LogWarning("InputHandler: Spawner reference not set and none found in scene.");
            }
            else
            {
                Debug.Log("InputHandler: Spawner found via FindObjectOfType.");
            }
        }
        else
        {
            Debug.Log("InputHandler: Spawner assigned in inspector.");
        }

        if (spawner != null)
        {
            Debug.Log("InputHandler: spawner.gameRunning=" + spawner.gameRunning);
        }
    }

    void Update()
    {
        // Check for any key press to load next scene after game ends
        if (waitingForInput && Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextSceneName);
        }

        if (!gameRunning) return;

        // Update timer
        timeLeft -= Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timeLeft).ToString();
        }

        // Check for game end
        if (timeLeft <= 0)
        {
            EndGame();
            return;
        }

        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Increment grips every time spacebar is pressed
            ProgressTracker.Instance.AddGrip();

            if (spawner.currentObject != null)
            {
                MovingObject mo = spawner.currentObject.GetComponent<MovingObject>();
                if (mo != null && mo.isInZone && !mo.hasBeenScored)
                {
                    // Increment score and update UI
                    score++;
                    if (scoreText != null)
                    {
                        scoreText.text = "Score: " + score.ToString();
                    }

                    // Increment collectible count
                    CollectibleManager.Instance.CollectiblePicked();

                    // Increment button press count
                    buttonPressCount++;

                    // Check if max button presses reached
                    if (buttonPressCount >= maxButtonPresses)
                    {
                        EndGame();
                        return;
                    }

                    // Mark as scored to prevent multiple increments
                    mo.hasBeenScored = true;

                    // Make the object drop
                    Rigidbody2D rb = spawner.currentObject.GetComponent<Rigidbody2D>();
                    if (rb == null)
                    {
                        rb = spawner.currentObject.AddComponent<Rigidbody2D>();
                    }
                    rb.gravityScale = 1f;
                    rb.linearVelocity = Vector2.down * dropSpeed;

                    // Disable the MovingObject script
                    mo.enabled = false;
                }
            }
        }

        // Check if the dropped object has fallen behind the bag and destroy it
        if (spawner.currentObject != null && spawner.currentObject.transform.position.y < destroyY)
        {
            // Play destroy sound before destroying the object
            if (audioSource != null && destroySound != null)
            {
                audioSource.PlayOneShot(destroySound);
            }
            
            // Instantiate particle effect at the object's position
            if (destroyParticleEffect != null)
            {
                GameObject particle = Instantiate(destroyParticleEffect, spawner.currentObject.transform.position, Quaternion.identity);
                ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop(true); // Stop and clear particles to prevent ongoing jobs
                    Destroy(particle, Mathf.Max(ps.main.duration, 0.1f)); // Ensure at least 0.1s delay
                }
                else
                {
                    Destroy(particle, 1f); // Fallback for non-particle objects
                }
            }
            
            Destroy(spawner.currentObject);
            spawner.currentObject = null;
        }
    }

    void EndGame()
    {
        gameRunning = false;
        spawner.gameRunning = false; // Stop spawning

        // Stop the audio
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // Show end game UI
        if (endGameUI != null)
        {
            endGameUI.SetActive(true);
        }
        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + score.ToString();
        }

        // Save player's score (reps + grips) to HighScoreManager for today's date
        string username = "";
        if (UserManager.Instance != null && UserManager.Instance.HasUser())
            username = UserManager.Instance.CurrentUser;
        else
            username = PlayerPrefs.GetString("GYG_CurrentUser", "");

        if (!string.IsNullOrEmpty(username))
        {
            int reps = 0;
            int grips = 0;
            if (ProgressTracker.Instance != null)
            {
                // log values before force-save
                Debug.Log($"InputHandler: EndGame called; before SaveNow reps={ProgressTracker.Instance.GetRepsToday()} grips={ProgressTracker.Instance.GetGripsToday()}");
                // Ensure the tracker writes any in-memory counters to PlayerPrefs before saving to HighScoreManager
                ProgressTracker.Instance.SaveNow();
                reps = ProgressTracker.Instance.GetRepsToday();
                grips = ProgressTracker.Instance.GetGripsToday();
            }

            if (HighScoreManager.Instance == null)
            {
                var go = new GameObject("HighScoreManager");
                go.AddComponent<HighScoreManager>();
            }

            HighScoreManager.Instance.SaveScore(username, DateTime.Now, reps, grips);
            Debug.Log($"InputHandler: saved highscore for '{username}' reps={reps} grips={grips}");
        }

        waitingForInput = true;
    }
}