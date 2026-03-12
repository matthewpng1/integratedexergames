using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        // the controller that should respawn; filled by caller
        public PlayerController player;

        public override void Execute()
        {
            // fall back to model.player if nobody supplied one (legacy single‑player)
            if (player == null)
                player = model.player;

            if (player == null)
            {
                Debug.LogError("Player reference is missing in PlayerSpawn script!");
                return;
            }

            if (!player.IsRespawning) return; // ✅ Prevent duplicate respawns

            player.collider2d.enabled = true;
            player.controlEnabled = false;

            if (player.audioSource && player.respawnAudio)
                player.audioSource.PlayOneShot(player.respawnAudio);

            // ✅ **Set respawn position to be the player's last safe position**
            // For two-player mode, respawn at the last safe position of the player in first place (larger x position, further ahead)
            Vector3 player1Safe = model.player.GetLastSafePosition();
            Vector3 player2Safe = model.player2.GetLastSafePosition();
            Vector3 respawnPosition = (player1Safe.x < player2Safe.x) ? player1Safe : player2Safe;

            // add a small backward buffer so the character doesn't spawn right on the edge
            const float respawnBuffer = 2.0f; // tweak this value to taste
            respawnPosition.x -= respawnBuffer;

            // Set respawn position and update the player's position
            player.transform.position = respawnPosition;

            player.jumpState = PlayerController.JumpState.Grounded;
            player.animator.SetBool("dead", false);

            // ✅ **Instantly snap this player's camera to their respawn position**
            if (player.vcam != null)
            {
                var vcam = player.vcam;
                vcam.Follow = null;
                vcam.LookAt = null;
                vcam.transform.position = new Vector3(respawnPosition.x, respawnPosition.y, vcam.transform.position.z);
                vcam.Follow = player.transform;
                vcam.LookAt = player.transform;
            }

            player.IsRespawning = false; // ✅ Fixes the double respawn glitch

            var evt = Simulation.Schedule<EnablePlayerInput>(0.1f);
            evt.player = player;
        }
    }
}