using Platformer.Core;
using Platformer.Model;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    /// <summary>
    /// This event is fired when user input should be enabled.
    /// </summary>
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        // which player should be enabled; if null, fall back to model.player
        public PlayerController player;

        public override void Execute()
        {
            if (player == null)
                player = model.player;
            if (player != null)
                player.controlEnabled = true;
        }
    }
}