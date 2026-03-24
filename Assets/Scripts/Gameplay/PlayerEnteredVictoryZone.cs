using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{

    /// <summary>
    /// This event is triggered when the player character enters a trigger with a VictoryZone component.
    /// </summary>
    /// <typeparam name="PlayerEnteredVictoryZone"></typeparam>
    public class PlayerEnteredVictoryZone : Simulation.Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;
        public PlayerController player;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            if (player == null)
                player = model.player;
            if (player != null)
            {
                player.animator.SetTrigger("victory");
                player.controlEnabled = false;
            }

            // Mark this player as finished both for model and stats
            model.MarkPlayerFinished(player);
            if (GameSessionStats.Instance != null)
            {
                GameSessionStats.Instance.GetPlayerStats(player).finished = true;
            }

            bool shouldEnd;
            if (victoryZone == null)
                shouldEnd = false;
            else if (victoryZone.endOnFirstPlayer)
                shouldEnd = true;
            else
                shouldEnd = model.BothPlayersFinished();

            if (shouldEnd && victoryZone != null)
            {
                // ✅ NEW: Apply bonus when platformer level is completed
                // Platformer levels primarily use ankle weights (though grippers are involved)
                if (ProgressTracker.Instance != null)
                {
                    Debug.Log($"PlayerEnteredVictoryZone: Level completed - applying ankle bonus...");
                    ProgressTracker.Instance.LevelComplete(usedGripper: false, usedAnkle: true);
                }
                
                victoryZone.ShowVictoryCanvas();
            }
        }
    }
}