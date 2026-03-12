using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        public PlayerController player;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            if (player == null) return;

            player.controlEnabled = false;
            player.animator.SetBool("dead", true);
            player.collider2d.enabled = false;

            Simulation.Schedule<PlayerSpawn>(2f); // ✅ Schedule respawn after 2 seconds
        }
    }
}