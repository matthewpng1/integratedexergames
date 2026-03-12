using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Model
{
    /// <summary>
    /// The main model containing needed data to implement a platformer style 
    /// game. This class should only contain data, and methods that operate 
    /// on the data. It is initialised with data in the GameController class.
    /// </summary>
    [System.Serializable]
    public class PlatformerModel
    {
        /// <summary>
        /// The virtual camera in the scene.
        /// </summary>
        public Unity.Cinemachine.CinemachineCamera virtualCamera;

        /// <summary>
        /// The main component which controls the player sprite, controlled 
        /// by the user.
        /// </summary>
        public PlayerController player;

        /// <summary>
        /// The second player controller (for two-player mode).
        /// </summary>
        public PlayerController player2;

        /// <summary>
        /// The spawn point in the scene.
        /// </summary>
        public Transform spawnPoint;

        /// <summary>
        /// A global jump modifier applied to all initial jump velocities.
        /// </summary>
        public float jumpModifier = 1.5f;

        /// <summary>
        /// A global jump modifier applied to slow down an active jump when 
        /// the user releases the jump input.
        /// </summary>
        public float jumpDeceleration = 0.5f;

        /// <summary>
        /// Tracks which players have reached the victory zone (for two-player coordination).
        /// </summary>
        private bool player1Finished = false;
        private bool player2Finished = false;

        public void MarkPlayerFinished(PlayerController pc)
        {
            if (pc == player)
                player1Finished = true;
            else if (pc == player2)
                player2Finished = true;
        }

        public bool BothPlayersFinished()
        {
            // if only one player in scene, check just that one
            if (player2 == null)
                return player1Finished;
            return player1Finished && player2Finished;
        }

        public void ResetVictoryState()
        {
            player1Finished = false;
            player2Finished = false;
        }
    }
}