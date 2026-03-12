using Platformer.Gameplay;
using UnityEngine;
using System.Collections;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class VictoryZone : MonoBehaviour
    {
        public GameObject victoryCanvas; // Drag VictoryCanvas in Inspector
        public float delayTime = 0.025f; // Set delay time here (in seconds)
        [Tooltip("If true the game ends when the first player reaches the zone; otherwise wait for both.")]
        public bool endOnFirstPlayer = true;

        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = Schedule<PlayerEnteredVictoryZone>();
                ev.victoryZone = this;
                ev.player = p;
            }
        }

        // Public method to show the victory canvas (called when both players finish)
        public void ShowVictoryCanvas()
        {
            StartCoroutine(ShowVictoryCanvasAfterDelay());
        }

        // Coroutine to delay showing the victory canvas
        private IEnumerator ShowVictoryCanvasAfterDelay()
        {
            // Wait for the specified delay time
            yield return new WaitForSeconds(delayTime);

            // Show the victory canvas and pause the game
            if (victoryCanvas != null)
            {
                victoryCanvas.SetActive(true);
                Time.timeScale = 0; // Pause the game
            }
        }
    }
}


