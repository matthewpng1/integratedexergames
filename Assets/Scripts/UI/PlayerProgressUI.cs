using UnityEngine;
using UnityEngine.UI;
using Platformer.Core;

/// <summary>
/// Shows a simple progress bar at the bottom of the screen indicating
/// where each player is relative to the level start and the victory zone.
/// Place this component on a UI object (for example a Slider or plain Image)
/// and assign the two handle RectTransforms to the little markers that
/// represent Player 1 and Player 2.  The bar rect should be the area inside
/// which the handles move.
/// </summary>
public class PlayerProgressUI : MonoBehaviour
{
    [Tooltip("Background rect of the progress bar (usually the Slider fill area)")]
    [SerializeField] private RectTransform bar;

    [Tooltip("Small icon/handle that will slide along the bar for player 1")]
    [SerializeField] private RectTransform player1Handle;

    [Tooltip("Small icon/handle that will slide along the bar for player 2")]
    [SerializeField] private RectTransform player2Handle;

    [Tooltip("Optional override for the world-space minimum X value. If left blank, spawn point X is used.")]
    [SerializeField] private float worldMinX;

    [Tooltip("Optional override for the world-space maximum X value. If left blank, first VictoryZone X is used.")]
    [SerializeField] private float worldMaxX;

    private Platformer.Model.PlatformerModel model;

    void Start()
    {
        // grab model if not wired up in inspector
        model = Simulation.GetModel<Platformer.Model.PlatformerModel>();

        if (model == null)
        {
            Debug.LogWarning("PlayerProgressUI: PlatformerModel not found in scene.");
            return;
        }

        if (worldMinX == 0 && model.spawnPoint != null)
            worldMinX = model.spawnPoint.position.x;

        if (worldMaxX == 0)
        {
            var vz = FindObjectOfType<Platformer.Mechanics.VictoryZone>();
            if (vz != null)
                worldMaxX = vz.transform.position.x;
        }

        // fallbacks in case neither spawn nor victory exists
        if (worldMaxX <= worldMinX)
            worldMaxX = worldMinX + 100f;
    }

    void Update()
    {
        if (model == null) return;

        if (player1Handle != null && model.player != null)
            UpdateHandle(player1Handle, model.player.transform.position.x);

        if (player2Handle != null && model.player2 != null)
            UpdateHandle(player2Handle, model.player2.transform.position.x);
    }

    private void UpdateHandle(RectTransform handle, float worldX)
    {
        float t = Mathf.InverseLerp(worldMinX, worldMaxX, worldX);
        t = Mathf.Clamp01(t);
        float width = bar.rect.width;
        // assume bar pivot is centre; adjust if different
        Vector2 pos = handle.anchoredPosition;
        pos.x = t * width - (width * 0.5f);
        handle.anchoredPosition = pos;
    }
}
