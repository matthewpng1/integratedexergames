using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    public class PlayerStopJump : Simulation.Event<PlayerStopJump>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player != null)
            {
                player.StopJump(); // ✅ Fixes `StopJump()` error
            }
        }
    }
}