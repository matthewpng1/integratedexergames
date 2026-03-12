using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public PlayerController player;
        public EnemyController enemy; // ✅ Fix: Add missing `enemy` reference

        public override void Execute()
        {
            if (player != null && player.IsAlive)
            {
                player.KillPlayer(); 
            }
        }
    }
}