using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    public class PlayerLanded : Simulation.Event<PlayerLanded>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player == null) return;
            player.jumpState = PlayerController.JumpState.Grounded;
        }
    }
}
