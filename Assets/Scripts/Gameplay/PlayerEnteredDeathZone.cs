using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerEnteredDeathZone : Simulation.Event<PlayerEnteredDeathZone>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player != null)
            {
                Simulation.Schedule<PlayerDeath>(0f); // ✅ Immediately trigger PlayerDeath
            }
        }
    }
}
