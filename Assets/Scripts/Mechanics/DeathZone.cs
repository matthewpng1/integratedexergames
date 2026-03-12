using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.KillPlayer(); // ✅ Fixes missing `PlayerDeath`
        }
    }
}