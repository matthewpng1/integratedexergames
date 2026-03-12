using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    public class HealthIsZero : Simulation.Event<HealthIsZero>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player != null && !player.IsAlive)
            {
                Simulation.Schedule<PlayerDeath>();
            }
        }
    }
}