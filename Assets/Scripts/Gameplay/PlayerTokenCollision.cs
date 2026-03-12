using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerTokenCollision : Simulation.Event<PlayerTokenCollision>
    {
        public PlayerController player;
        public TokenInstance token;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            // Play token collection sound
            AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);

            // Update Collectibles UI
            CollectiblesUIManager uiManager = GameObject.FindObjectOfType<CollectiblesUIManager>();
            if (uiManager != null)
            {
                uiManager.IncreaseCollectibleCount();
            }

            // Disable the collected token
            token.gameObject.SetActive(false);
        }
    }
}